namespace FslexFsyacc.Yacc

//type UnambiguousCollection =
//    {
//        grammar : Grammar
//        kernels : Map<Set<ItemCore>,int>
//        unambiguousClosures: Map<int,Set<ItemCore*Set<string>>>
//        GOTOs: Map<int,Map<string,Set<ItemCore>>>
//    }

//    static member create
//        (mainProductions:string list list)
//        (names:Map<string list,string>)
//        (precedences:Map<string,int>)
//        =
//        let x = AmbiguousCollection.create mainProductions

//        let terminals = x.grammar.terminals

//        let unambiguousClosures =
//            x.closures
//            |> Map.map(fun i closure ->
//                closure
//                |> Map.toSeq
//                |> Seq.choose(fun (la,conflicts) ->
//                    let maybeItem =
//                        match conflicts with
//                        | [x] -> Some x
//                        //如果都是shift，则保持集合不变
//                        | [x;y] ->
//                            (x,y)
//                            |> ItemCoreUtils.disambiguate terminals names precedences
//                        | _ -> failwithf "failed to resolve:%A" (i,la,conflicts)
//                    maybeItem |> Option.map(fun item -> la, item)
//                )
//                |> Map.ofSeq
//            )
//        {
//            grammar = x.grammar
//            kernels = x.kernels
//            unambiguousClosures = unambiguousClosures
//            GOTOs = x.GOTOs
//        }
