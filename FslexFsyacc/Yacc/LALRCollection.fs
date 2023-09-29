namespace FslexFsyacc.Yacc
open FslexFsyacc.Runtime

///
type LALRCollection =
    {
        grammar : Grammar
        kernels : Map<Set<ItemCore>,int> // kernel -> index
        closures: Map<int,Set<string*ItemCore>> // index -> lookahead*action
    }

    [<System.Obsolete("LALRCollectionCrewUtils.getLALRCollectionCrew")>]
    static member create(mainProductions:string list list) =
        let grammar = Grammar.from mainProductions
        let itemCores = ItemCoreFactory.make grammar.productions
        let itemCoreAttributes =
            ItemCoreAttributeFactory.make grammar.nonterminals grammar.nullables grammar.firsts itemCores
        let closures =
            CollectionFactory.make itemCores itemCoreAttributes grammar.productions
        //反查kernel的编号
        let kernels =
            closures
            |> Set.map fst
            |> Set.toList
            |> List.mapi(fun i k -> k,i)
            |> Map.ofList

        let closures =
            closures
            |> Set.toList
            |> List.mapi(fun i (kernels,closure) -> //需要检查状态编号是否是从0开始
                ///展开lookaheads的closure
                let spreadedClosure =
                    closure
                    |> Set.toList
                    |> List.collect(fun (itemCore,lookaheads) ->
                        let lookaheads =
                            if (ItemCoreUtils.dotmax itemCore) then
                                lookaheads
                            else
                                //GOTO的nextSymbol分为两种情况：
                                //当nextSymbol是terminal时用于侦察冲突
                                //当nextSymbol是nonterminal时仅用于占位
                                Set.singleton (ItemCoreUtils.nextSymbol itemCore) 
                        lookaheads
                        |> Set.toList
                        |> List.map(fun lookahead -> lookahead, itemCore)
                    )
                    |> Set.ofList
                i,spreadedClosure
            )
            |> Map.ofList
        {
            grammar = grammar
            kernels = kernels
            closures = closures
        }

    [<System.Obsolete("LALRCollectionCrewUtils.getGOTOs")>]
    member this.getGOTOs() =
        this.closures
        |> Map.map(fun i closure ->
            closure
            |> Set.filter(fun(la,itemCore)-> not (ItemCoreUtils.dotmax itemCore))
            |> Seq.groupBy(fun (la,_) -> la)
            |> Seq.map(fun(la,pairs)->
                let nextKernel =
                    pairs
                    |> Seq.map(fun(la,itemCore) ->
                        (ItemCoreUtils.dotIncr itemCore)
                    )
                    |> Set.ofSeq
                la, nextKernel
            )
            |> Map.ofSeq
        )
