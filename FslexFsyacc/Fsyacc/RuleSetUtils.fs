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
    |> List.find(fun s -> s > "")

let findRuleByDummyToken
    (dummyToken:string)
    (augmentRules:Set<list<string>*string*string>)
    =
    augmentRules
    |> Seq.find(fun(_,tok,_)->tok=dummyToken)

