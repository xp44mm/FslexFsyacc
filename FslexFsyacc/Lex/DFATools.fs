namespace FslexFsyacc.Lex

open FSharp.Idioms

module DFATools =
    /// DFA nextState(s,a)
    let getNextStates (transitions:Set<uint32*'tag*uint32>)=
        let result:Map<uint32,Map<'tag,uint32>> = 
            Set.toUniqueJaggedMap transitions
        result

    ////鍵：單個的接受狀態。-> 值：接受状态键对应的lexeme State集合。
    //let getLexemesFromFinal(finalLexemes:list<Set<uint32>*Set<uint32>>)=
    //    let result:Map<uint32,Set<uint32>> =
    //        finalLexemes
    //        |> List.filter(snd >> Set.isEmpty >> not) // 不包含沒有lookahead的接受狀態。
    //        |> List.collect(fun(finals,lexemes) ->
    //            finals
    //            |> Set.toList
    //            |> List.map(fun e -> e, lexemes)
    //        )
    //        |> Map.ofList
    //    result

    /////所有的finals狀態
    //let getUniversalFinals (finalLexemes:list<Set<uint32>*_>):Set<uint32> = 
    //    finalLexemes
    //    |> Seq.map fst
    //    |> Set.unionMany

    /////键：接受状态，值：接受状态对应的索引，索引是匹配模式的索引
    //let getIndeciesFromFinal(finalLexemes:list<Set<uint32>*_>):Map<uint32,int> =
    //    finalLexemes
    //    |> List.map fst
    //    |> List.mapi(fun i st ->
    //        st
    //        |> Set.map(fun e -> e,i)
    //        |> Set.toList)
    //    |> List.concat
    //    |> Map.ofList

    ///// string tag -> char tag
    //let convert (states:Map<uint32,Map<string,uint32>>) =
    //    states
    //    |> Map.map(fun st mp ->
    //        mp 
    //        |> Map.mapKey(fun tag tgt -> tag.[0]) 
    //        |> Map.map(fun ch sq -> Seq.exactlyOne sq)
    //    )
