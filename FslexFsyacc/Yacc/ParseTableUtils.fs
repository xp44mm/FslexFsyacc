module FslexFsyacc.Yacc.ParseTableUtils

//open FSharp.Idioms

//let getProductionsMap (productions:Set<string list>) =
//    let mp: Map<int,string list> =
//        productions
//        |> Set.toArray
//        |> Array.mapi(fun i prod -> -i, prod) // 产生式负数编号
//        |> Map.ofArray
//    mp

//let getActions (parsingTable:Set<int*string*int>) =
//    let mp: Map<int,Map<string,int>> =
//        parsingTable
//        |> Set.toUniqueJaggedMap
//    mp
