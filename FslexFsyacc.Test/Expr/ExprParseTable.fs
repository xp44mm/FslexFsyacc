﻿module FslexFsyacc.Expr.ExprParseTable
let actions = [["(",2;"-",5;"NUMBER",7;"expr",1];["",0;"*",12;"+",13;"-",14;"/",15];["(",2;"-",5;"NUMBER",7;"expr",3];[")",4;"*",12;"+",13;"-",14;"/",15];["",-1;")",-1;"*",-1;"+",-1;"-",-1;"/",-1];["(",2;"-",5;"NUMBER",7;"expr",6];["",-2;")",-2;"*",-2;"+",-2;"-",-2;"/",-2];["",-3;")",-3;"*",-3;"+",-3;"-",-3;"/",-3];["",-4;")",-4;"*",-4;"+",-4;"-",-4;"/",-4];["",-5;")",-5;"*",12;"+",-5;"-",-5;"/",15];["",-6;")",-6;"*",12;"+",-6;"-",-6;"/",15];["",-7;")",-7;"*",-7;"+",-7;"-",-7;"/",-7];["(",2;"-",5;"NUMBER",7;"expr",8];["(",2;"-",5;"NUMBER",7;"expr",9];["(",2;"-",5;"NUMBER",7;"expr",10];["(",2;"-",5;"NUMBER",7;"expr",11]]
let closures = [[0,0,[];-1,0,[];-2,0,[];-3,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[]];[0,1,[""];-4,1,[];-5,1,[];-6,1,[];-7,1,[]];[-1,0,[];-1,1,[];-2,0,[];-3,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[]];[-1,2,[];-4,1,[];-5,1,[];-6,1,[];-7,1,[]];[-1,3,["";")";"*";"+";"-";"/"]];[-1,0,[];-2,0,[];-2,1,[];-3,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[]];[-2,2,["";")";"*";"+";"-";"/"]];[-3,1,["";")";"*";"+";"-";"/"]];[-4,3,["";")";"*";"+";"-";"/"]];[-4,1,[];-5,3,["";")";"+";"-"];-7,1,[]];[-4,1,[];-6,3,["";")";"+";"-"];-7,1,[]];[-7,3,["";")";"*";"+";"-";"/"]];[-1,0,[];-2,0,[];-3,0,[];-4,0,[];-4,2,[];-5,0,[];-6,0,[];-7,0,[]];[-1,0,[];-2,0,[];-3,0,[];-4,0,[];-5,0,[];-5,2,[];-6,0,[];-7,0,[]];[-1,0,[];-2,0,[];-3,0,[];-4,0,[];-5,0,[];-6,0,[];-6,2,[];-7,0,[]];[-1,0,[];-2,0,[];-3,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[];-7,2,[]]]

let rules:(string list*(obj list->obj))list = [
    ["expr";"expr";"+";"expr"],fun(ss:obj list)->
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 + s2
        box result
    ["expr";"expr";"-";"expr"],fun(ss:obj list)->
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 - s2
        box result
    ["expr";"expr";"*";"expr"],fun(ss:obj list)->
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 * s2
        box result
    ["expr";"expr";"/";"expr"],fun(ss:obj list)->
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 / s2
        box result
    ["expr";"(";"expr";")"],fun(ss:obj list)->
        let s1 = unbox<float> ss.[1]
        let result:float =
            s1
        box result
    ["expr";"-";"expr"],fun(ss:obj list)->
        let s1 = unbox<float> ss.[1]
        let result:float =
            -s1
        box result
    ["expr";"NUMBER"],fun(ss:obj list)->
        let s0 = unbox<float> ss.[0]
        let result:float =
            s0
        box result
]
let unboxRoot =
    unbox<float>
let theoryParser = FslexFsyacc.Runtime.TheoryParser.create(rules, actions, closures)
let stateSymbolPairs = theoryParser.getStateSymbolPairs()