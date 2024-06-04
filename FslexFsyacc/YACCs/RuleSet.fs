module FslexFsyacc.YACCs.RuleSet

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

/// 用到的符号
let getUsedSymbols (startSymbol:string) (rules:Set<Rule>) =
    let rec loop (rules:Set<Rule>) (symbols:Set<string>) =
        let nextRules, remainRules =
            rules
            |> Set.partition(fun rule ->
                rule.production.Head
                |> symbols.Contains )
        if nextRules.IsEmpty then
            symbols
        else
            let nextSymbols =
                nextRules
                |> Set.map(fun rule -> set rule.production.Tail)
                |> Set.unionMany
                |> Set.union symbols
            loop remainRules nextSymbols
    startSymbol
    |> Set.singleton
    |> loop rules
        
let purgeRules (startSymbol:string) (rules:Set<Rule>) =
    let symbols = getUsedSymbols startSymbol rules
    let rules =
        rules
        |> Set.filter(fun rule -> 
            rule.production.Head 
            |> symbols.Contains
            )
    if startSymbol = "" then
        rules
    else
        rules.Add(Rule.augment startSymbol)

let removeHeads (heads:Set<string>) (rules:Set<Rule>) =
    rules
    |> Set.filter(fun rule ->
        rule.production.Head 
        |> heads.Contains
        |> not
        )

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
