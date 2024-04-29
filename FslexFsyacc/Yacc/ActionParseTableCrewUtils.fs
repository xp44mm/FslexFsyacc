module FslexFsyacc.Yacc.ActionParseTableCrewUtils

open FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Idioms.Literal

let fromAmbiguousCollectionCrew
    (collection:AmbiguousCollectionCrew)
    (dummyTokens:Map<string list,string>)
    (precedences:Map<string,int>)
    =
    let unambiguousItemCores =
        collection.conflictedItemCores
        |> AmbiguousCollectionUtils.getUnambiguousItemCores
            (dummyTokens:Map<string list,string>)
            (precedences:Map<string,int>)
            (collection.terminals:Set<string>)

    let actions =
        unambiguousItemCores
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
                    | acts -> failwith $"there is a conflict: {stringify acts}"
                )
            actions
        )

    let resolvedClosures =
        unambiguousItemCores
        |> Map.map(fun state cnflcts ->
            AmbiguousCollectionUtils.getItemcores cnflcts
        )

    ActionParseTableCrew(collection,dummyTokens,precedences,unambiguousItemCores,actions,resolvedClosures)

let getActionParseTableCrew
    (mainProductions:string list list)
    (dummyTokens:Map<string list,string>)
    (precedences:Map<string,int>)
    =

    let collection =
        mainProductions
        |> ProductionsCrewUtils.getProductionsCrew
        |> GrammarCrewUtils.getNullableCrew
        |> GrammarCrewUtils.getFirstLastCrew
        |> GrammarCrewUtils.getFollowPrecedeCrew
        |> GrammarCrewUtils.getItemCoresCrew
        |> LALRCollectionCrewUtils.getLALRCollectionCrew
        |> AmbiguousCollectionCrewUtils.getAmbiguousCollectionCrew

    fromAmbiguousCollectionCrew collection dummyTokens precedences
