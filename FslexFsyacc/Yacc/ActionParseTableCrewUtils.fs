module FslexFsyacc.Yacc.ActionParseTableCrewUtils

open FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Literals.Literal

let getActionParseTableCrew
    (mainProductions:string list list)
    (productionNames:Map<string list,string>)
    (precedences:Map<string,int>)
    =

    let uc =
        mainProductions
        |> GrammarCrewUtils.getProductionsCrew
        |> GrammarCrewUtils.getNullableCrew
        |> GrammarCrewUtils.getFirstLastCrew
        |> GrammarCrewUtils.getFollowPrecedeCrew
        |> GrammarCrewUtils.getItemCoresCrew
        |> LALRCollectionCrewUtils.getLALRCollectionCrew
        |> AmbiguousCollectionCrewUtils.getAmbiguousCollectionCrew
        |> AmbiguousCollectionCrewUtils.toUnambiguousCollection productionNames precedences // 构建一个类
    
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
                    | acts -> failwith $"this is a conflict: {stringify acts}"
                )
            actions
        )

    let resolvedClosures = 
        uc.conflicts
        |> Map.map(fun state cnflcts ->
            AmbiguousCollectionUtils.getItemcores cnflcts
        )

    ActionParseTableCrew(uc,actions,resolvedClosures)
