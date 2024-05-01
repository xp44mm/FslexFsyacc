module FslexFsyacc.Fsyacc.AugmentRulesUtils

open FslexFsyacc.Runtime
open FslexFsyacc.Yacc

open FSharp.Idioms
open FSharp.Idioms.Literal
open System

/// get Augment Rules
let ofFlat (rules:list<list<string>*string*string>) =
    let p0,_,_ = rules.[0]
    let augmentRule = ["";p0.Head],"","s0"

    rules
    |> Set.ofList
    |> Set.add augmentRule

let toFlat (augmentRules:Set<list<string>*string*string>) =
    augmentRules
    |> Set.minElement
    |> Set.remove <| augmentRules
    |> Set.toList

/// get Augment Rules
let ofRaw (rules:(string*(list<list<string>*string*string>))list) =
    rules
    |> FlatRulesUtils.ofRaw
    |> ofFlat

let getDummyTokens (augmentRules:Set<list<string>*string*string>) =
    augmentRules
    |> Set.filter(fun (prod,dummy,act) -> dummy > "")
    |> Set.map(Triple.firstTwo)
    |> Map.ofSeq

let getStartSymbol (augmentRules:Set<list<string>*string*string>) =
    augmentRules.MinimumElement
    |> Triple.first
    |> List.last

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
    |>Set.groupBy (fun(prod,_,_)->prod)
    |>Set.map(fun(prod,group)->prod,group.Count)
    |>Set.filter(fun(prod,c)->c>1)

///
let getSingleProductions (augmentRules:Set<list<string>*string*string>) =
    augmentRules
    |> Set.map Triple.first // rule -> prod
    |> Set.groupBy List.head
    |> Set.map snd
    |> Seq.choose ProductionListUtils.trySingle
    |> Set.ofSeq

let addRule
    (rule:list<string>*string*string)
    (augmentRules:Set<list<string>*string*string>)
    =
    Set.add rule augmentRules

let removeProduction
    (production:list<string>)
    (augmentRules:Set<list<string>*string*string>)
    =
    augmentRules
    |> Set.filter(fun(x,_,_) -> x <> production)

/// 消除规则集的被removed符号，生成新的规则集。新旧规则集等价。
let eliminateSymbol (removed:string) (augmentRules:Set<list<string>*string*string>) =
    //保存名字，和行为
    let keeps =
        augmentRules
        |> Seq.map(fun(a,b,c)-> a,(b,c))
        |> Map.ofSeq
                    
    let productions =
        augmentRules
        |> Set.map Triple.first
        |> ProductionSetUtils.eliminateSymbol removed

    productions
    |> Set.map(fun prod -> 
        let nm,act = 
            if keeps.ContainsKey prod then
                keeps.[prod]
            else "",""
        prod,nm,act)
                
/// 注意扩展开始产生式也是Singles
let getSingles (augmentRules:Set<list<string>*string*string>) =
    augmentRules
    |> Set.map Triple.first // rule -> prod
    |> ProductionSetUtils.getSingles
