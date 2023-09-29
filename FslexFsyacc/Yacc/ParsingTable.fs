namespace FslexFsyacc.Yacc
open FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Literals.Literal

/// 原始解析表 before Encoder
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
            uc.conflicts
            |> Map.map(fun i cnflcts -> 
                let actions =
                    cnflcts
                    |> Map.map(fun la icores ->
                        match 
                            icores
                            |> ActionUtils.from
                            |> Set.toList
                        with
                        | [] -> failwith $"nonassoc error."
                        | [x] -> x
                        | acts -> failwith $"this is a one more actions conflict: {stringify acts}"
                    )
                actions
            )

        {
            grammar = uc.grammar
            kernels = uc.kernels
            actions = actions
            closures = 
                uc.conflicts
                |> Map.map(fun state cnflcts ->
                    AmbiguousCollectionUtils.getItemcores cnflcts
                )
        }
