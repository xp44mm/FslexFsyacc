
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
        lastTerminal:string
        dummyToken:string
        production:string list
    }

    static member from (rules: Rule Set) =
        let bnf =
            rules
            |> Set.map(fun rule -> rule.production)
            |> BNF.just
        let ps = bnf.getConflictedProductions()
        let tryGetLastTerminal = Precedence.lastTerminal bnf.terminals
        rules
        |> Set.filter(fun rule -> 
            ps.Contains rule.production
        )
        |> Set.map(fun rule ->
            let lt = tryGetLastTerminal rule.production |> Option.defaultValue ""
            {
                lastTerminal = lt
                dummyToken = rule.dummy
                production = rule.production
            }
        )
