module FslexFsyacc.Yacc.ParseTableTools

open FSharp.Idioms

let getProductionsMap (productions:Set<string list>) :Map<int,string list> =
    productions
    |> Set.toArray
    |> Array.mapi(fun i prod -> -i, prod) // 产生式负数编号
    |> Map.ofArray

let getActions (parsingTable:Set<int*string*int>) :Map<int,Map<string,int>> =
    parsingTable
    |> Set.toUniqueJaggedMap

