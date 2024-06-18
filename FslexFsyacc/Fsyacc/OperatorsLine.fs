module FslexFsyacc.Fsyacc.OperatorsLine

open FSharp.Idioms
open FSharp.Idioms.Literal
open FslexFsyacc
open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FslexFsyacc.Precedences

/// dummy和lastTerm必须都包含在Operators中
/// dummy 必须对应一个操作符，这个操作符在集合中必须有两个。
let filterOperatorsLines (rules:list<Rule>) (operatorsLines: list<Associativity * Set<string>>) =
    let dummies =
        rules
        |> Seq.filter(fun rule -> rule.dummy > "")
        |> Seq.map(fun rule -> rule.dummy)
        |> Set.ofSeq

    let productions =
        rules
        |> Seq.map(fun rule -> rule.production)

    let heads =
        productions
        |> Seq.map List.head
        |> Set.ofSeq

    let terminals =
        productions
        |> Seq.collect List.tail
        |> Set.ofSeq
        |> Set.difference <| heads

    let st = terminals + dummies

    operatorsLines
    |> List.map(fun (assoc,symbols) -> 
        assoc, Set.intersect symbols st
    )


