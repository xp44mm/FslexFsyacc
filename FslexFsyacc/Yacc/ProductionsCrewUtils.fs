module FslexFsyacc.Yacc.ProductionsCrewUtils

open FslexFsyacc.Runtime
open FSharp.Idioms

//从文法生成增广文法
let getProductionsCrew(inputProductionList:list<list<string>>) =
    let startSymbol = inputProductionList.[0].[0]

    let mainProductions = set inputProductionList

    let augmentedProductions =
            mainProductions
            |> Set.add ["";startSymbol]

    ProductionsCrew(
        inputProductionList,
        startSymbol,
        mainProductions,
        augmentedProductions
    )

let ofAugmentedProductions (augmentedProductions:Set<list<string>>) =
    let augProd = 
        augmentedProductions
        |> Set.minElement

    let startSymbol = augProd.[1]

    let mainProductions = 
        augmentedProductions
        |> Set.remove augProd

    let a,b =
        mainProductions
        |> Set.partition(fun p ->
            p.[0] = startSymbol
        )

    ProductionsCrew(
        [ yield! a; yield! b],
        startSymbol,
        mainProductions,
        augmentedProductions
    )

