module FslexFsyacc.Yacc.LALRCollectionCrewUtils
open FSharp.Idioms
open FslexFsyacc.Runtime

let getLALRCollectionCrew (grammar:ItemCoresCrew) =

    let closures =
        GrammarCrewUtils.getClosureCollection grammar

    //反查kernel的编号
    let kernels =
        closures
        |> Set.map fst

    // kernel -> closure
    let closures =
        closures
        |> Set.toList
        |> List.mapi(fun i (kernels,closure) -> //需要检查状态编号是否是从0开始
            ///展开lookaheads的closure
            let spreadedClosure =
                closure
                |> Set.toList
                |> List.collect(fun (itemCore,lookaheads) ->
                    let itemCoreCrew = grammar.itemCoreCrews.[itemCore]
                    let lookaheads =
                        if itemCoreCrew.dotmax then
                            lookaheads
                        else
                            //GOTO的nextSymbol分为两种情况：
                            //当nextSymbol是terminal时用于侦察冲突
                            //当nextSymbol是nonterminal时仅用于占位
                            Set.singleton (ItemCoreCrewUtils.getNextSymbol itemCoreCrew)
                    lookaheads
                    |> Set.toList
                    |> List.map(fun lookahead -> lookahead, itemCore)
                )
                |> Set.ofList
            i,spreadedClosure
        )
        |> Map.ofList
    // kernel -> string -> kernel
    let gotos =
        closures
        |> Map.map(fun i closure ->
            closure
            |> Set.filter(fun(la,item)-> 
                let ic = grammar.itemCoreCrews.[item]
                not ic.dotmax)
            |> Seq.groupBy(fun (la,_) -> la)
            |> Seq.map(fun(la,pairs)->
                let kernel =
                    pairs
                    |> Seq.map(fun(la,itemcore) ->
                        {itemcore with dot = itemcore.dot + 1}
                    )
                    |> Set.ofSeq
                la, kernel
            )
            |> Map.ofSeq
        )
    LALRCollectionCrew(grammar,kernels,closures,gotos)
