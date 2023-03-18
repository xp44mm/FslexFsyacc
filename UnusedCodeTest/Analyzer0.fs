namespace FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Literals.Literal
/// 解析带数据的对象
type Analyzer0<'tok,'u>
    (
        nextStates: (uint32*(string*uint32)list)list, // state -> tag -> state
        rules: (uint32 list*uint32 list*('tok list -> 'u))list
    ) =

    // state -> tag -> state
    let nextStates =
        nextStates
        |> Map.ofList
        |> Map.map(fun _ ls -> Map.ofList ls)

    /// final状态是包括向前看的最长状态。
    let finals =
        rules
        |> List.map Triple.first

    let universalFinals =
        finals
        |> List.concat
        |> Set.ofList

    /// lexeme状态是回退后，实际取词状态。
    // final -> lexemes
    let lexemesFromFinal =
        rules
        |> List.filter(fun(_,lxms,_)-> lxms.Length>0) // 不包含沒有lookahead的接受狀態。
        |> List.collect(fun(finals,lexemes,_) ->
            finals
            |> List.map(fun e -> e, Set.ofList lexemes)
        )
        |> Map.ofList

    let indicesFromFinal =
        finals
        |> List.mapi(fun i fs -> fs |> List.map(fun f -> f,i))
        |> List.concat
        |> Map.ofList

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
                    failwith $"no found final state in: {stringify revStates}"
                lexemeStates
            else finalStates
        finalState, lexemeStates.Length

    member _.analyze(inputs:seq<'tok>, getTag:'tok -> string) =
        let iterator =
            inputs 
            |> Seq.map(fun tok -> getTag tok,tok)
            |> RetractableIterator

        let rec tryForwardIterator state states =
            match iterator.tryNext() with
            | None ->
                match states with
                | [] -> None
                | _ -> Some(state::states)
            | Some (tag, tok) ->
                match tryNextState state tag with
                | None -> Some(state::states)
                | Some nextState ->
                    tryForwardIterator nextState (state::states)

        ///division是相邻token,or division的容器。
        let backwardIterator (revStates) =
            let finalState,stateCount =
                try
                retractFinalAndLexemeSate(revStates)
                with _ ->
                let buffer = iterator.dequeue(revStates.Length-1)
                failwith $"FslexFsyacc analyzer:retract was not able to find an accepted status in {stringify buffer}"

            let lexeme = iterator.dequeue(stateCount-1)
            let mapper = finalMappers.[finalState]
            let lexbuf = lexeme |> Array.map snd |> Array.toList
            mapper lexbuf

        //let rec forward state states =
        //    let nextStates = state::states
        //    match iterator.tryNext() with
        //    | None -> nextStates // 没有取到符号
        //    | Some (tag, tok) ->
        //        match tryNextState state tag with
        //        | None -> nextStates //没有下一状态
        //        | Some nextState ->
        //            forward nextState nextStates

        /////division是相邻token,or division的容器。
        //let getDivision () =
        //    let revStates = forward 0u []

        //    let finalState,stateCount =
        //        try
        //        retractFinalAndLexemeSate(revStates)
        //        with _ ->
        //        let buffer = iterator.dequeue(revStates.Length-1)
        //        failwith $"FslexFsyacc analyzer:retract was not able to find an accepted status in {stringify buffer}"

        //    let lexeme = iterator.dequeue(stateCount-1)
        //    let mapper = finalMappers.[finalState]
        //    let lexbuf = lexeme |> Array.map snd |> Array.toList
        //    mapper lexbuf

        seq {
            while iterator.ongoing() do
                match tryForwardIterator 0u [] with
                | None -> ()
                | Some revStates ->
                    yield backwardIterator revStates
        }
