module FslexFsyacc.Fsyacc.FsyaccFileRules

open FSharp.Idioms

/// 对相同lhs的rule合并，仅此
let flatToRawRules
    (rules:list<string list*string*string>)
    =
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

let addRule
    (rule:string list*string*string)
    (rules:list<string list*string*string>)
    =
    rules @ [rule]

let removeRule
    (rule:string list)
    (rules:list<string list*string*string>)
    =
    rules
    |> List.filter(fun(x,_,_) -> x<>rule)

let findRuleIndex
    (prod:string list)
    (rules:list<string list*string*string>)
    =
    rules
    |> List.findIndex(fun(p,_,_)->p=prod)

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
let repeatRule
    (rules:list<string list*string*string>)
    =
    rules
    |> List.groupBy (fun(rule,_,_)->rule)
    |> List.map(fun(rule,group)->rule,group.Length)
    |> List.filter(fun(r,c)->c>1)

let getProductions (rules:list<string list*string*string>) =
    rules
    |> List.map Triple.first

let getProductionNames(rules:list<string list*string*string>) =
    rules
    |> List.filter(fun(_,nm,_) -> nm > "")
    |> List.map(fun(prod,name,_)-> prod,name)
    |> Map.ofList
