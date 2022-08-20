module FslexFsyacc.Fsyacc.FsyaccFileRules

/// 对相同lhs的rule合并，仅此
let normToRawRules
    (rules:(string list*string*string)list)
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

//简写的产生式规范化为完整的产生式
let rawToNormRules
    (rules:(string*((string list*string*string)list))list)
    =
    rules
    |> List.collect(fun(lhs,bodies)->
        bodies
        |> List.map(fun (symbols,name,semantic)->
            lhs::symbols,name,semantic
        )
    )

let addRule
    (rule:string list*string*string)
    (rules:(string list*string*string)list)
    =
    rule::rules

let removeRule
    (rule:string list)
    (rules:(string list*string*string)list)
    =
    rules
    |> List.filter(fun(x,_,_) -> x<>rule)

let replaceRule
    (oldProd:string list)
    (newRule:string list*string*string)
    (rules:(string list*string*string)list)
    =
    newRule::(rules |> removeRule oldProd)

let findRule
    (rule:string list)
    (rules:(string list*string*string)list)
    =
    rules
    |> List.find(fun(x,_,_)->x=rule)

let findRuleByName
    (name:string)
    (rules:(string list*string*string)list)
    =
    rules
    |> List.find(fun(_,x,_)->x=name)

///检查rules中是否有重复的规则
let repeatRule
    (rules:(string list*string*string)list)
    =
    rules
    |> List.groupBy (fun(rule,_,_)->rule)
    |> List.map(fun(rule,group)->rule,group.Length)
    |> List.filter(fun(r,c)->c>1)
