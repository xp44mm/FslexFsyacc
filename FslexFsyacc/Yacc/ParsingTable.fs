namespace FslexFsyacc.Yacc

open FSharp.Idioms
open FslexFsyacc.Runtime
open FSharp.Literals.Literal

/// 原始解析表
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
                let actions =
                    closure
                    |> Map.map(fun la icores ->
                        match 
                            icores
                            |> Action.from
                            |> Set.toList
                        with
                        | [] -> failwith $"nonassoc error."
                        | [x] -> x
                        | acts -> failwith $"this is a conflict: {stringify acts}"
                    )
                actions
            )

        let closures =
            uc.closures
            |> Map.map(fun i conflicts ->
                AmbiguousCollectionUtils.getItems conflicts
            )

        //简化数据的表达为数组
        {
            grammar = uc.grammar
            kernels = uc.kernels
            actions = actions
            closures = closures
        }
