module FslexFsyacc.Fsyacc.RuleListUtils

open FSharp.Idioms
open System
open FslexFsyacc.Yacc

open FSharp.Idioms
open FSharp.Literals.Literal
open System

///分配左手边到右手边，保持规则顺序不变
let ofRaw (rules:(string*(list<list<string>*string*string>))list) =
    rules
    |> List.collect(fun(lhs,bodies)->
        bodies
        |> List.map(fun (body,name,semantic)->
            lhs::body,name,semantic
        )
    )

/// 对相同lhs的rule合并，仅此
let toRaw (rules:list<list<string>*string*string>) =
    rules
    |> List.groupBy(fun(prod,_,_)->prod.Head) //lhs
    |> List.map(fun(lhs,groups)->
        let rhs =
            groups
            |> List.map(fun(prod,name,sem)->
                prod.Tail,name,sem
            )
        lhs,rhs
    )

/// 排序
let normRules (rules:list<list<string>*string*string>) =
    rules
    |> Set.ofList
    |> Set.toList

let filterDummyProductions (rules:list<list<string>*string*string>) =
    rules
    |> List.filter(fun (prod,dummy,act) -> dummy > "")
    |> List.map(Triple.firstTwo)

let getMainProductions (rules:list<list<string>*string*string>) =
    rules 
    |> List.map Triple.first

let getSemanticRules (rules:list<list<string>*string*string>) = 
    rules 
    |> List.map Triple.ends

let getStartSymbol (rules:list<list<string>*string*string>) =
    rules.[0]
    |> Triple.first
    |> List.head

let addRule
    (rule:list<string>*string*string)
    (rules:list<list<string>*string*string>)
    =
    rules @ [rule]

let removeRule
    (production:list<string>)
    (rules:list<list<string>*string*string>)
    =
    rules
    |> List.filter(fun(x,_,_) -> x<>production)

let findRuleIndex
    (production:list<string>)
    (rules:list<list<string>*string*string>)
    =
    rules
    |> List.findIndex(fun(p,_,_)->p=production)

/// 保持替换的位置
let replaceRule
    (oldProd:list<string>)
    (newRule:list<string>*string*string)
    (rules:list<list<string>*string*string>)
    =
    let sameLhs () =
        let nl = 
            newRule
            |> Triple.first 
            |> List.head
        let ol = oldProd.Head
        nl = ol

    if not(sameLhs ()) then
        failwith $"replaceRule should is same lhs."

    let i = 
        rules
        |> List.findIndex(fun(prod,_,_)->prod=oldProd)

    let x,y =
        rules
        |> List.splitAt i

    [
        yield! x
        newRule
        yield! y.Tail
    ]

let findRuleByName
    (name:string)
    (rules:list<list<string>*string*string>)
    =
    rules
    |> List.find(fun(_,x,_)->x=name)

///检查rules中是否有重复的规则
let duplicateRule
    (rules:list<list<string>*string*string>)
    =
    rules
    |> List.groupBy (fun(rule,_,_)->rule)
    |> List.map(fun(rule,group)->rule,group.Length)
    |> List.filter(fun(r,c)->c>1)

let removeErrorRules (robust:Set<string>) (rules:list<list<string>*string*string>) =
    let willBeRemoved (symbol: string) =
        robust
        |> Set.exists(fun kw -> symbol.Contains kw)

    let reserve prod =
        prod 
        |> List.forall(fun (symbol:string) -> not(willBeRemoved symbol))

    rules
    |> List.filter(fun(prod,nm,act)->reserve prod)

/// 消除规则集的被removed符号，生成新的规则集。
let eliminateSymbol (removed:string) (rules:list<list<string>*string*string>) =
    //保存名字，和行为
    let keeps =
        rules
        |> List.map(fun(a,b,c)-> a,(b,c))
        |> Map.ofList
                    
    let productions =
        rules
        |> List.map Triple.first
        |> ProductionListUtils.eliminateSymbol removed

    productions
    |> List.map(fun prod -> 
        let nm,act = 
            if keeps.ContainsKey prod then
                keeps.[prod]
            else "",""
        prod,nm,act)
                
///
let getChomsky (rules:list<list<string>*string*string>) =
    rules
    |> List.map Triple.first // rule -> prod
    |> ProductionListUtils.getChomsky

let eliminateChomsky (rules:list<list<string>*string*string>) =
    let nonterminals = 
        rules
        |> getChomsky 
        |> List.map List.head

    let rules =
        nonterminals
        |> List.fold(fun rules sym -> eliminateSymbol sym rules) rules
    rules

let extractRules (start:string) (rules:list<list<string>*string*string>) =
    //每个符号对应的产生式体
    let mp = 
        rules
        |> toRaw
        |> Map.ofList

    let symbols =
        rules
        |> List.map Triple.first
        |> ProductionListUtils.extractSymbols start

    symbols
    |> List.choose(fun s -> 
        if mp.ContainsKey s then
            Some(s,mp.[s])
        else None)
    |> ofRaw

// 在excludeSymbols中的符号，位于规则左手边，此规则将被删除
let removeNonterminals
    (excludeSymbols:Set<string>)
    (rules:list<list<string>*string*string>)
    =
    rules
        |> List.filter(fun (prod,_,_) ->
            prod.Head
            |> excludeSymbols.Contains
            |> not)
