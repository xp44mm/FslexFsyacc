module FslexFsyacc.Runtime.YACCs.Operators

open FSharp.Idioms
open FSharp.Idioms.Literal
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.Precedences

let getProperDummies (rules: Rule Set) =
    let grammar = 
        rules
        |> Set.map(fun rule -> rule.production)
        |> Grammar.just

    rules
    |> Set.filter(fun rule -> rule.dummy > "")
    |> Set.map(fun rule ->
        let lastTerm = Precedence.lastTerminal grammar.terminals rule.production
        rule.production, rule.dummy, lastTerm)

/// dummy和lastTerm必须都包含在Operators中
let unusedDummies (ops:string Set) (rules: Rule Set) =
    //dummy和lastTerm
    let dummies =
        rules
        |> getProperDummies
        |> Seq.collect(fun (p,d,l) -> 
            [
                d;
                match l with
                | Some x -> x
                | None -> ()
            ]
        )
        |> Set.ofSeq
    dummies - ops
