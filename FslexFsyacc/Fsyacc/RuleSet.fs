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
    //产生式去重
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

/// 删除符号的产生式，不展开此符号
let removeHeads (heads:Set<string>) (rules:Set<Rule>) =
    rules
    |> Set.filter(fun rule ->
        rule.production.Head 
        |> heads.Contains
        |> not
        )

/// 删除符号所在产生式
let removeSymbols (symbols:Set<string>) (rules:Set<Rule>) =
    rules
    |> Set.filter(fun rule ->
        rule.production
        |> List.exists(fun s -> symbols.Contains s)
        |> not
        )

/// 从产生式中删除此符号，但保留修改后的产生式
let deleteSymbols (symbols:Set<string>) (rules:Set<Rule>) =
    let mapper (rule:Rule) =
        let head, body =
            match rule.production with
            | head:: body -> head, body
            | _ -> failwith "unreachable"
        let body2 =
            body
            |> List.filter( 
                symbols.Contains >> not
            )
        {
            rule with production = head :: body2
        }
        
    rules
    |> Set.map( mapper )

/// 冲突产生式的dummy提示
let conflictDummies (rules:Set<Rule>) =
    let productions = 
        rules
        |> Set.map(fun rule -> rule.production)

    let bnf = BNF.just productions

    let conflicts = bnf.getConflictedProductions()

    conflicts
    |> Set.map(fun production ->
        let rule = 
            rules
            |> Seq.find(fun rule -> rule.production = production)
        let nm =
            match production with
            | Precedence.HeadAsDummy productions dummy ->
                dummy
            | Precedence.LastTerminalAsDummy bnf.terminals dummy ->
                dummy
            | _ -> ""

        production,nm,rule.dummy
    )
