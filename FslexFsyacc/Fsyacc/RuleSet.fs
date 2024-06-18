module FslexFsyacc.Fsyacc.RuleSet

open System
open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc
open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FslexFsyacc.Precedences
open FslexFsyacc.BNFs

let fromGroups (ruleGroups: RuleGroup list) =
    let ruleList =
        ruleGroups
        |> List.collect(fun grp -> 
            grp.bodies
            |> List.map(fun bd ->
                {
                    production = grp.lhs::bd.rhs
                    dummy = bd.dummy
                    reducer =  bd.reducer
                }
            )
        )
    //去重
    let repl =
        ruleList
        |> List.groupBy(fun rule -> rule.production)
        |> List.filter(fun (p,ls) -> ls.Length > 1)

    if repl.IsEmpty then
        let startSymbol = ruleGroups.[0].lhs
        let augmentRule = Rule.augment startSymbol
        Set.ofList (augmentRule :: ruleList)
    else
        let ps = repl |> List.map fst
        failwith $"输入有重复的产生式：{stringify ps}"

let getProperDummies (rules: Rule Set) =
    rules
    |> Set.filter(fun rule -> rule.dummy > "")

///检查多余的dummy
let redundantDummies (rules: Rule Set) =
    let dummies =
        rules
        |> Set.filter(fun rule -> rule.dummy > "")
        |> Set.map(fun rule -> rule.production)

    let bnf =
        rules
        |> Set.map(fun rule -> rule.production)
        |> BNF.just

    let conflictedProductions = bnf.getConflictedProductions()

    let diff = 
        Set.difference dummies conflictedProductions

    if diff.IsEmpty then
        ()
    else failwith $"{stringify (List.ofSeq diff)}"

/// 用到的符号
let crawl (startSymbol:string) (rules:Set<Rule>) =
    let heads =
        rules
        |> Set.map( fun rule -> rule.production.Head )

    let rec loop (rules:Set<Rule>) (rulelist:list<Rule>) =
        let symbols =
            rulelist
            |> List.collect(fun rule -> rule.production.Tail )
            |> Set.ofList
            |> Set.filter(heads.Contains)

        let nextRules, remainRules =
            rules
            |> Set.partition(fun rule ->
                symbols.Contains rule.production.Head
                )
        if nextRules.IsEmpty then
            rulelist
        else
            let rulelist =
                [
                    yield! rulelist
                    yield! nextRules
                ]
            loop remainRules rulelist

    let nextRules, remainRules =
        rules
        |> Set.partition(fun rule ->
            rule.production.Head = startSymbol
            )
    nextRules
    |> List.ofSeq
    |> loop remainRules

let toGroups (rules:seq<Rule>) =
    rules
    |> Seq.groupBy(fun rule -> rule.production.Head)
    |> Seq.map(fun (hd,ls)->
        let bodies =
            ls
            |> Seq.map(fun rule -> {
                rhs = rule.production.Tail
                dummy = rule.dummy
                reducer = rule.reducer
            })
            |> Seq.toList

        {
            lhs = hd
            bodies = bodies
        }
    )
    |> Seq.toList

let removeHeads (heads:Set<string>) (rules:Set<Rule>) =
    rules
    |> Set.filter(fun rule ->
        rule.production.Head 
        |> heads.Contains
        |> not
        )

let removeSymbols (symbols:Set<string>) (rules:Set<Rule>) =
    rules
    |> Set.filter(fun rule ->
        rule.production
        |> List.exists(fun s -> symbols.Contains s)
        |> not
        )
