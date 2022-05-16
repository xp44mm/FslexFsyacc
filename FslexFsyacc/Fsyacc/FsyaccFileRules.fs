module FslexFsyacc.Fsyacc.FsyaccFileRules

let normToRawRules 
    (rules:(string list*string*string)list)
    //(productionNames:Map<string list,string>)
    =

    rules
    //|> List.map(fun(prod,name,sem)->
    //    // with name
    //    let name = productionNames.[prod]
    //    prod,name,sem
    //)
    |> List.groupBy(fun(prod,_,_)->
        prod.Head
    )
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
    |> List.collect(fun (head,bodies)->
        bodies
        |> List.map(fun (symbols,name,semantic)->
            head::symbols,name,semantic
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


