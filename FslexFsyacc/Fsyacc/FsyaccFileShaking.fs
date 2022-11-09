module FslexFsyacc.Fsyacc.FsyaccFileShaking
open FslexFsyacc.Yacc

open FSharp.Idioms
open FSharp.Literals.Literal
open System

let extractRules (start:string) (rules:list<string list*string*string>) =
    let rawRules = 
        rules
        |> FsyaccFileRules.flatToRawRules
        |> Map.ofList

    let productions =
        rules
        |> List.map Triple.first
        |> ProductionUtils.getNodes

    start
    |> List.depthFirstSort productions
    |> List.choose(fun s -> 
        if rawRules.ContainsKey s then
            Some(s,rawRules.[s])
        else None)
    |> FsyaccFileRules.rawToFlatRules
