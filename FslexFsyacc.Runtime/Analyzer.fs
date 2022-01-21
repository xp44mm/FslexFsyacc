namespace FslexFsyacc.Runtime

open FSharp.Idioms

/// 解析带数据的对象
type Analyzer<'tok,'u>
    (
        nextStates: (uint32*(string*uint32)[])[], // state -> tag -> state
        rules: (uint32[]*uint32[]*('tok list -> 'u))[]
    ) =

    // state -> tag -> state
    let nextStates = 
        nextStates
        |> Map.ofArray
        |> Map.map(fun _ arr -> Map.ofArray arr)

    /// final状态是包括向前看的最长状态。
    let finals =
        rules
        |> Array.map Triple.first

    let universalFinals = 
        finals
        |> Array.concat
        |> Set.ofArray

    /// lexeme状态是回退后，实际取词状态。
    // final -> lexemes
    let lexemesFromFinal =
        rules
        |> Array.filter(fun(_,lxms,_)-> lxms.Length>0) // 不包含沒有lookahead的接受狀態。
        |> Array.collect(fun(finals,lexemes,_) ->
            finals
            |> Array.map(fun e -> e, Set.ofArray lexemes)
        )
        |> Map.ofArray

    let indicesFromFinal =
        finals
        |> Array.mapi(fun i fs -> fs |> Array.map(fun f -> f,i))
        |> Array.concat
        |> Map.ofArray

    let finalMappers =
        indicesFromFinal
        |> Map.map(fun final i -> Triple.last rules.[i])
    
    let tryNextState state symbol =
        if nextStates.ContainsKey(state) && nextStates.[state].ContainsKey(symbol) then
            Some nextStates.[state].[symbol]
        else None

    //无副作用，根据 states 从后向前找到final,lexeme的位置。
    let retractFinalAndLexemeSate (revStates) =
        let finalStates =
            revStates
            |> List.skipWhile(universalFinals.Contains>>not)
        let finalState = finalStates.Head

        let lexemeStates =
            if lexemesFromFinal.ContainsKey(finalState) then
                let lexemes = lexemesFromFinal.[finalState]
                let lexemeStates =
                    finalStates.Tail
                    |> List.skipWhile(lexemes.Contains>>not)
                if lexemeStates.IsEmpty then
                    failwithf "no found: %A" revStates
                lexemeStates
            else finalStates
        finalState, lexemeStates.Length

    member _.analyze(inputs:seq<'tok>, getTag:'tok -> string) =
        let iterator =
            let tagTokenPairs =
                inputs
                |> Seq.map(fun tok -> getTag tok,tok)
            RetractableIterator(tagTokenPairs.GetEnumerator())

        let rec forward state states =
            let nextStates = state::states
            match iterator.tryNext() with
            | None -> nextStates // 没有取到符号
            | Some (tag, tok) ->
                match tryNextState state tag with
                | None -> nextStates //没有下一状态
                | Some nextState ->
                    forward nextState nextStates

        ///division是相邻token,or division的容器。
        let getDivision () =
            let revStates = forward 0u []
            let finalState,stateCount =
                try
                    retractFinalAndLexemeSate(revStates)
                with _ ->
                    let buffer = iterator.dequeue(revStates.Length-1)
                    failwithf "FslexFsyacc analyzer:%A" buffer

            let lexeme = iterator.dequeue(stateCount-1)
            let mapper = finalMappers.[finalState]
            let lexbuf = lexeme |> Array.map snd |> Array.toList
            mapper lexbuf

        seq {
            while iterator.allDone() |> not do
                yield getDivision()
        }