module FslexFsyacc.Runtime.YACCs.RuleSet

open System
open FSharp.Idioms
open FSharp.Idioms.Literal

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
let getSymbols (rules:Set<Rule>) =
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
    set [""]
    |> loop rules
        
let filterUsedRules (rules:Set<Rule>) =
    let symbols = getSymbols rules
    rules
    |> Set.filter(fun rule -> 
        rule.production.Head 
        |> symbols.Contains
        )

let removeHeads (heads:Set<string>) (rules:Set<Rule>) =
    rules
    |> Set.filter(fun rule -> 
        rule.production.Head 
        |> heads.Contains
        |> not
        )
