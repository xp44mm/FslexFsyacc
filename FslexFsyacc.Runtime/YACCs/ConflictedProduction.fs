
namespace FslexFsyacc.Runtime.YACCs

open System
open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.Precedences
open FslexFsyacc.Runtime.BNFs

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
