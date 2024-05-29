module FslexFsyacc.Expr.ExprParseTable
let tokens = set ["(";")";"*";"+";"-";"/";"NUMBER"]
let kernels = [[0,0];[0,1;-4,1;-5,1;-6,1;-7,1];[-1,1];[-1,2;-4,1;-5,1;-6,1;-7,1];[-1,3];[-2,1];[-2,2;-4,1;-5,1;-6,1;-7,1];[-3,1];[-4,1;-4,3;-5,1;-6,1;-7,1];[-4,1;-5,1;-5,3;-6,1;-7,1];[-4,1;-5,1;-6,1;-6,3;-7,1];[-4,1;-5,1;-6,1;-7,1;-7,3];[-4,2];[-5,2];[-6,2];[-7,2]]
let kernelSymbols = ["";"expr";"(";"expr";")";"-";"expr";"NUMBER";"expr";"expr";"expr";"expr";"*";"+";"-";"/"]
let actions = [["(",2;"-",5;"NUMBER",7;"expr",1];["",0;"*",12;"+",13;"-",14;"/",15];["(",2;"-",5;"NUMBER",7;"expr",3];[")",4;"*",12;"+",13;"-",14;"/",15];["",-1;")",-1;"*",-1;"+",-1;"-",-1;"/",-1];["(",2;"-",5;"NUMBER",7;"expr",6];["",-2;")",-2;"*",-2;"+",-2;"-",-2;"/",-2];["",-3;")",-3;"*",-3;"+",-3;"-",-3;"/",-3];["",-4;")",-4;"*",-4;"+",-4;"-",-4;"/",-4];["",-5;")",-5;"*",12;"+",-5;"-",-5;"/",15];["",-6;")",-6;"*",12;"+",-6;"-",-6;"/",15];["",-7;")",-7;"*",-7;"+",-7;"-",-7;"/",-7];["(",2;"-",5;"NUMBER",7;"expr",8];["(",2;"-",5;"NUMBER",7;"expr",9];["(",2;"-",5;"NUMBER",7;"expr",10];["(",2;"-",5;"NUMBER",7;"expr",11]]

let rules: list<string list*(obj list->obj)> = [
    ["";"expr"], fun(ss:obj list)-> ss.[0]
    ["expr";"(";"expr";")"], fun(ss:obj list)->
        let s1 = unbox<float> ss.[1]
        let result:float =
            s1
        box result
    ["expr";"-";"expr"], fun(ss:obj list)->
        let s1 = unbox<float> ss.[1]
        let result:float =
            -s1
        box result
    ["expr";"NUMBER"], fun(ss:obj list)->
        let s0 = unbox<float> ss.[0]
        let result:float =
            s0
        box result
    ["expr";"expr";"*";"expr"], fun(ss:obj list)->
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 * s2
        box result
    ["expr";"expr";"+";"expr"], fun(ss:obj list)->
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 + s2
        box result
    ["expr";"expr";"-";"expr"], fun(ss:obj list)->
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 - s2
        box result
    ["expr";"expr";"/";"expr"], fun(ss:obj list)->
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 / s2
        box result
]
let unboxRoot =
    unbox<float>

let app:FslexFsyacc.Runtime.ParseTableApp = {
    tokens = tokens
    kernels = kernels
    kernelSymbols = kernelSymbols
    actions = actions
    rules = rules
}
