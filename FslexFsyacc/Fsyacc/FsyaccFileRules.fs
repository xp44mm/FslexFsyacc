module FslexFsyacc.Fsyacc.FsyaccFileRules // FlatRules

open FSharp.Idioms
open System

/// 对相同lhs的rule合并，仅此
let flatToRawRules (rules:list<string list*string*string>) =
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

///分配左手边到右手边，保持规则顺序不变
let rawToFlatRules
    (rules:(string*(list<string list*string*string>))list)
    =
    rules
    |> List.collect(fun(lhs,bodies)->
        bodies
        |> List.map(fun (body,name,semantic)->
            lhs::body,name,semantic
        )
    )

let getMainProductions (rules:list<string list*string*string>) =
    rules |> List.map Triple.first

let getDummyTokens(rules:list<string list*string*string>) =
    rules
    |> List.filter(fun(_,dummyToken,_) -> dummyToken > "")
    |> List.map(fun(prod,dummyToken,_)-> prod,dummyToken)
    |> Map.ofList





let addRule
    (rule:string list*string*string)
    (rules:list<string list*string*string>)
    =
    rules @ [rule]

let removeRule
    (production:string list)
    (rules:list<string list*string*string>)
    =
    rules
    |> List.filter(fun(x,_,_) -> x<>production)

let findRuleIndex
    (production:string list)
    (rules:list<string list*string*string>)
    =
    rules
    |> List.findIndex(fun(p,_,_)->p=production)

/// 保持替换的位置
let replaceRule
    (oldProd:string list)
    (newRule:string list*string*string)
    (rules:list<string list*string*string>)
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
    (rules:list<string list*string*string>)
    =
    rules
    |> List.find(fun(_,x,_)->x=name)

///检查rules中是否有重复的规则
let duplicateRule
    (rules:list<string list*string*string>)
    =
    rules
    |> List.groupBy (fun(rule,_,_)->rule)
    |> List.map(fun(rule,group)->rule,group.Length)
    |> List.filter(fun(r,c)->c>1)

let removeErrorRules (robust:string Set) (rules:list<string list*string*string>) =
    let willBeRemoved (symbol: string) =
        robust
        |> Set.exists(fun kw -> symbol.Contains kw)

    let reserve prod =
        prod 
        |> List.forall(fun (symbol:string) -> not(willBeRemoved symbol))

    rules
    |> List.filter(fun(prod,nm,act)->reserve prod)

let eliminateSymbolFromRules (symbol:string) (rules:list<string list*string*string>) =
        //保存名字，和行为
        let keeps =
            rules
            |> List.map(fun(a,b,c)-> a,(b,c))
            |> Map.ofList
                    
        let bnf: FslexFsyacc.Yacc.BNF =
            {
                productions =
                    rules
                    |> List.map Triple.first
            }

        let bnf1 = bnf.eliminate(symbol)

        bnf1.productions
        |> List.map(fun prod -> 
            let nm,act = 
                if keeps.ContainsKey prod then 
                    keeps.[prod]
                else "",""
            prod,nm,act)
        
/// (terminals:Set<string>)
let getChomsky (rules:list<string list*string*string>) =
    let tryChomsky prods =
        match prods with
        | [prod & ([_]|[_;_])] -> 
            Some prod
        //| [prod & ] when terminals.Contains b ->
        //    Some prod
        | _ -> None

    rules
    |> List.map Triple.first // rule -> prod
    |> List.groupBy List.head
    |> List.map snd
    |> List.choose tryChomsky

let eliminateChomsky (rules:list<string list*string*string>) =

    let chos = getChomsky rules
    let rules =
        chos
        |> List.map List.head
        |> List.fold(fun rules cho -> eliminateSymbolFromRules cho rules) rules
    rules
