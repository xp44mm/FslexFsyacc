module FslexFsyacc.Expr.ExprParseTable
let tokens = set ["(";")";"*";"+";"-";"/";"NUMBER"]
let kernels = [[0,0];[0,1;-3,1;-4,1;-5,1;-6,1];[-1,1];[-1,2;-3,1;-4,1;-5,1;-6,1];[-1,3];[-2,1];[-3,1;-3,3;-4,1;-5,1;-6,1];[-3,1;-4,1;-4,3;-5,1;-6,1];[-3,1;-4,1;-5,1;-5,3;-6,1];[-3,1;-4,1;-5,1;-6,1;-6,3];[-3,1;-4,1;-5,1;-6,1;-8,2];[-3,2];[-4,2];[-5,2];[-6,2];[-7,1];[-8,1]]
let kernelSymbols = ["";"expr";"(";"expr";")";"NUMBER";"expr";"expr";"expr";"expr";"expr";"*";"+";"-";"/";"unaryExpr";"-"]
let actions = [["(",2;"-",16;"NUMBER",5;"expr",1;"unaryExpr",15];["",0;"*",11;"+",12;"-",13;"/",14];["(",2;"-",16;"NUMBER",5;"expr",3;"unaryExpr",15];[")",4;"*",11;"+",12;"-",13;"/",14];["",-1;")",-1;"*",-1;"+",-1;"-",-1;"/",-1];["",-2;")",-2;"*",-2;"+",-2;"-",-2;"/",-2];["",-3;")",-3;"*",-3;"+",-3;"-",-3;"/",-3];["",-4;")",-4;"*",11;"+",-4;"-",-4;"/",14];["",-5;")",-5;"*",11;"+",-5;"-",-5;"/",14];["",-6;")",-6;"*",-6;"+",-6;"-",-6;"/",-6];["",-8;")",-8;"*",-8;"+",-8;"-",-8;"/",-8];["(",2;"-",16;"NUMBER",5;"expr",6;"unaryExpr",15];["(",2;"-",16;"NUMBER",5;"expr",7;"unaryExpr",15];["(",2;"-",16;"NUMBER",5;"expr",8;"unaryExpr",15];["(",2;"-",16;"NUMBER",5;"expr",9;"unaryExpr",15];["",-7;")",-7;"*",-7;"+",-7;"-",-7;"/",-7];["(",2;"-",16;"NUMBER",5;"expr",10;"unaryExpr",15]]

let rules : list<string list*(obj list->obj)> = [
    ["";"expr"], fun(ss:obj list)-> ss.[0]
    ["expr";"(";"expr";")"], fun(ss:obj list)->
        let s1 = unbox<float> ss.[1]
        let result:float =
            s1
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
    ["expr";"unaryExpr"], fun(ss:obj list)->
        let s0 = unbox<float> ss.[0]
        let result:float =
            s0
        box result
    ["unaryExpr";"-";"expr"], fun(ss:obj list)->
        let s1 = unbox<float> ss.[1]
        let result:float =
            -s1
        box result
]
let unboxRoot =
    unbox<float>
let app: FslexFsyacc.ParseTableApp = {
    tokens        = tokens
    kernels       = kernels
    kernelSymbols = kernelSymbols
    actions       = actions
    rules         = rules
}