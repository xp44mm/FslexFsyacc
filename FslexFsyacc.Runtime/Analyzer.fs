namespace FslexFsyacc.Runtime

open System
open FSharp.Idioms
open FSharp.Literals

///解析带数据的对象
//final状态是包括向前看的最长状态。
//lexeme状态是回退后最终匹配的较短状态。
type Analyzer<'tok,'u>
    (
        nextStates:Map<uint32,Map<string,uint32>>, // state -> tag -> state
        lexemesFromFinal:Map<uint32,Set<uint32>>, // final -> lexemes
        universalFinals:Set<uint32>,
        finalMappers: Map<uint32,('tok list -> 'u)> // final -> mapper
    ) =

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

    member this.analyze(inputs:seq<'tok>, getTag:'tok -> string) =
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