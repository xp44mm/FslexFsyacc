module FslexFsyacc.Yacc.ProductionCrewUtils

open FslexFsyacc.Runtime

let getProductionCrew(production:Production) =
    let leftside =
        production
        |> List.head

    let body =
        production
        |> List.tail

    ProductionCrew(production,leftside,body)

