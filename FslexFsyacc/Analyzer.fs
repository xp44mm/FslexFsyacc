namespace FslexFsyacc.Runtime

open FSharp.Idioms

/// 解析带数据的对象
type Analyzer<'tok,'u>
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

    let (|Lookahead|_|) finalState =
        if lexemesFromFinal.ContainsKey(finalState) then
            Some lexemesFromFinal.[finalState]
        else None

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

    let retract (revStates:uint32 list) (revTokens:'a list) =
        let finalStates,finalTokens =
            AnalyzerUtils.skipUntilFoundIn universalFinals (revStates) (revTokens)

        let finalState = finalStates.Head

        let revLexemeTokens =
            match finalState with
            | Lookahead lexemes ->
                AnalyzerUtils.skipUntilFoundIn lexemes finalStates.Tail finalTokens.Tail
                |> snd
            | _ -> finalTokens
        finalState,revLexemeTokens

    member _.analyze(inputs:seq<'tok>, getTag:'tok -> string) =
        let iterator =
            inputs
            |> Seq.map(fun tok -> getTag tok,tok)
            |> BufferIterator

        // todo:状态加索引，作用是缓存count
        let rec tryForwardIterator (states:uint32 list) tokens =
            match iterator.tryNext() with
            | None ->
                match tokens with
                | [] -> None
                | _ -> Some(states,tokens)
            | Some (tag, tok) ->
                match tryNextState states.Head tag with
                | Some state ->
                    tryForwardIterator (state::states) (tok::tokens)
                | None -> Some(states,tokens)

        seq {
            while iterator.ongoing() do
                match tryForwardIterator [0u] [] with
                | None -> ()
                | Some (revStates,revTokens) ->
                    let finalState,revLexemeTokens =
                        retract revStates revTokens
                    let mapper = finalMappers.[finalState]
                    // todo: 颠倒顺序同时计算长度
                    let len,lexbuf =
                        revLexemeTokens
                        |> List.countRev
                    iterator.dequeue(len)
                    yield mapper lexbuf
        }
