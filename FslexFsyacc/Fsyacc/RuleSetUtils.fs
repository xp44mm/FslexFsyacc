module FslexFsyacc.Fsyacc.RuleSetUtils

open FslexFsyacc.Runtime
open FslexFsyacc.Yacc

open FSharp.Idioms
open FSharp.Literals.Literal
open System

/// get Augment Rules
let ofFlat (rules:list<list<string>*string*string>) =
    let p0,_,_ = rules.[0]
    let augmentRule = ["";p0.Head],"","s0"

    rules
    |> Set.ofList
    |> Set.add augmentRule

/// get Augment Rules
let ofRaw (rules:(string*(list<list<string>*string*string>))list) =
    rules
    |> RuleListUtils.ofRaw
    |> ofFlat

/// 不行，用start然后在RuleListUtils中toRaw，问题是Rule的顺序
let toRaw (rules:Set<list<string>*string*string>) =
    let augmentRule = rules |> Set.minElement
    
    rules
    |> Set.remove augmentRule
    |> Set.groupBy(fun(prod,_,_)->prod.Head) //lhs
    |> Set.map(fun(lhs,groups)->
        let rhs =
            groups
            |> Set.map(fun(prod,name,sem) ->
                prod.Tail,name,sem
            )
        lhs,rhs
    )

let getDummyTokens (augmentRules:Set<Production*string*string>) =
    augmentRules
    |> Set.filter(fun (prod,dummy,act) -> dummy > "")
    |> Set.map(Triple.firstTwo)
    |> Map.ofSeq

let getStartSymbol (augmentRules:Set<Production*string*string>) =
    augmentRules.MinimumElement
    |> Triple.first
    |> List.last

let findRuleByDummyToken (dummyToken:string) (augmentRules:Set<list<string>*string*string>) =
    augmentRules
    |> Seq.find(fun(_,tok,_)->tok = dummyToken )

//删除包含符号的规则
let removeSymbols (symbols:Set<string>) (augmentRules:Set<list<string>*string*string>) =
    augmentRules
    |> Set.filter(fun (prod,nm,act) -> 
        prod 
        |> ProductionUtils.without symbols)

// 在excludeSymbols中的符号，位于规则左手边，此规则将被删除
let removeHeads
    (excludeSymbols:Set<string>)
    (augmentRules:Set<list<string>*string*string>)
    =
    augmentRules
    |> Set.filter(fun (prod,_,_) ->
        prod.Head
        |> excludeSymbols.Contains
        |> not)

///检查rules中是否有重复的规则
let duplicateRule (augmentRules:Set<list<string>*string*string>) =
    augmentRules
    |>Set.groupBy (fun(rule,_,_)->rule)
    |>Set.map(fun(rule,group)->rule,group.Count)
    |>Set.filter(fun(r,c)->c>1)

/////
//let getChomsky (augmentRules:Set<list<string>*string*string>) =
//    augmentRules
//    |> Set.map Triple.first // rule -> prod
//    |> ProductionListUtils.getChomsky
