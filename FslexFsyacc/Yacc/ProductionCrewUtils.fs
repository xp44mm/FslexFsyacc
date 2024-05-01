module FslexFsyacc.Yacc.ProductionCrewUtils

open FslexFsyacc.Runtime

let getProductionCrew(production:string list) =
    let leftside =
        production
        |> List.head

    let body =
        production
        |> List.tail

    ProductionCrew(production,leftside,body)

