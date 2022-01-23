namespace FslexFsyacc.Yacc

open FSharp.Idioms

type ParsingTable =
    {
        grammar : Grammar
        kernels : Map<Set<ItemCore>,int>
        actions : Map<int,Map<string,Action>>
        closures: Map<int,Map<ItemCore,Set<string>>>
    }

    static member create(
        mainProductions:string list list,
        productionNames:Map<string list,string>,
        precedences:Map<string,int>
        ) =

        let uc =
            AmbiguousCollection
                .create(mainProductions)
                .toUnambiguousCollection(productionNames,precedences)

        let actions =
            uc.closures
            |> Map.map(fun i closure ->
                let rMp,gMp =
                    closure
                    |> Map.partition(fun la icores -> icores.MinimumElement.dotmax)

                let reduces =
                    rMp
                    |> Map.map(fun la icores ->
                        let item = Seq.exactlyOne icores
                        Reduce item.production
                    )

                let shifts =
                    let gotos = uc.GOTOs.[i]
                    gMp
                    |> Map.map(fun la icores ->
                        Shift gotos.[la]
                    )
                Map.append shifts reduces
                |> Map.map(fun la sq -> Seq.exactlyOne sq)
            )

        let closures =
            uc.closures
            |> Map.map(fun i closure ->
                let rMp,gMp =
                    closure
                    |> Map.partition(fun la icores -> icores.MinimumElement.dotmax)

                let reduces =
                    rMp
                    |> Map.toArray
                    |> Array.map(fun(la, icores) -> Seq.exactlyOne icores, la)
                    |> Array.groupBy fst
                    |> Array.map(fun(icore,sq)->
                        let st = sq |> Seq.map snd |> Set.ofSeq
                        icore, st)
                    |> Set.ofArray

                let shifts =
                    gMp
                    |> Map.values
                    |> Set.unionMany
                    |> Set.map(fun icore -> icore, Set.empty)

                shifts+reduces
                |> Map.ofSeq
            )

        //简化数据的表达为数组
        {
            grammar = uc.grammar
            kernels = uc.kernels
            actions = actions
            closures = closures
        }
