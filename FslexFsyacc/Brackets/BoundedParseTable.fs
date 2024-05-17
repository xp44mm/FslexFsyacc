module FslexFsyacc.Brackets.BoundedParseTable
let actions = [["LEFT",6;"bounded",1];["",0];["LEFT",-1;"RIGHT",-1;"TICK",-1];["LEFT",-2;"RIGHT",-2;"TICK",-2];["LEFT",6;"RIGHT",7;"TICK",2;"band",5;"bounded",3];["LEFT",-4;"RIGHT",-4;"TICK",-4];["LEFT",-3;"RIGHT",-3;"TICK",-3;"bands",4];["",-5;"LEFT",-5;"RIGHT",-5;"TICK",-5]]
let closures = [[0,0,[];-5,0,[]];[0,1,[""]];[-1,1,["LEFT";"RIGHT";"TICK"]];[-2,1,["LEFT";"RIGHT";"TICK"]];[-1,0,[];-2,0,[];-4,1,[];-5,0,[];-5,2,[]];[-4,2,["LEFT";"RIGHT";"TICK"]];[-3,0,["LEFT";"RIGHT";"TICK"];-4,0,[];-5,1,[]];[-5,3,["";"LEFT";"RIGHT";"TICK"]]]

let rules:list<string list*(obj list->obj)> = [
    ["";"bounded"], fun(ss:obj list)-> ss.[0]
    ["band";"TICK"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:Band =
            Tick s0
        box result
    ["band";"bounded"], fun(ss:obj list)->
        let s0 = unbox<Band> ss.[0]
        let result:Band =
            s0
        box result
    ["bands"], fun(ss:obj list)->
        let result:Band list =
            []
        box result
    ["bands";"bands";"band"], fun(ss:obj list)->
        let s0 = unbox<Band list> ss.[0]
        let s1 = unbox<Band> ss.[1]
        let result:Band list =
            s1::s0
        box result
    ["bounded";"LEFT";"bands";"RIGHT"], fun(ss:obj list)->
        let s0 = unbox<int> ss.[0]
        let s1 = unbox<Band list> ss.[1]
        let s2 = unbox<int> ss.[2]
        let result:Band =
            Bounded(s0,List.rev s1,s2)
        box result
]
let unboxRoot =
    unbox<Band>
let baseParser = FslexFsyacc.Runtime.BaseParser.create(rules, actions, closures)
let stateSymbolPairs = baseParser.getStateSymbolPairs()