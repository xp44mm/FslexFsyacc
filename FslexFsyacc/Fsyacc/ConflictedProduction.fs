
namespace FslexFsyacc.Fsyacc

open System
open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc
open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FslexFsyacc.Precedences
open FslexFsyacc.BNFs

type ConflictedProduction = 
    {
        headOrLastTerminal: string
        dummyToken: string
        production: string list
    }

    static member from (rules: Rule Set) =
        rules
        |> RuleSet.conflictDummies 
        |> Set.map(fun (p,h,d) -> {
            headOrLastTerminal = h
            dummyToken = d
            production = p
        })
