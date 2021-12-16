module Expr.ExprParseTable
let header = "open Expr.ExprToken\r\nopen System"
let productions = Map [-7,["expr";"expr";"/";"expr"];-6,["expr";"expr";"-";"expr"];-5,["expr";"expr";"+";"expr"];-4,["expr";"expr";"*";"expr"];-3,["expr";"NUMBER"];-2,["expr";"-";"expr"];-1,["expr";"(";"expr";")"];0,["";"expr"]]
let actions = Map [0,Map ["(",2;"-",5;"NUMBER",7;"expr",1];1,Map ["",0;"*",12;"+",13;"-",14;"/",15];2,Map ["(",2;"-",5;"NUMBER",7;"expr",3];3,Map [")",4;"*",12;"+",13;"-",14;"/",15];4,Map ["",-1;")",-1;"*",-1;"+",-1;"-",-1;"/",-1];5,Map ["(",2;"-",5;"NUMBER",7;"expr",6];6,Map ["",-2;")",-2;"*",-2;"+",-2;"-",-2;"/",-2];7,Map ["",-3;")",-3;"*",-3;"+",-3;"-",-3;"/",-3];8,Map ["",-4;")",-4;"*",-4;"+",-4;"-",-4;"/",-4];9,Map ["",-5;")",-5;"*",12;"+",-5;"-",-5;"/",15];10,Map ["",-6;")",-6;"*",12;"+",-6;"-",-6;"/",15];11,Map ["",-7;")",-7;"*",-7;"+",-7;"-",-7;"/",-7];12,Map ["(",2;"-",5;"NUMBER",7;"expr",8];13,Map ["(",2;"-",5;"NUMBER",7;"expr",9];14,Map ["(",2;"-",5;"NUMBER",7;"expr",10];15,Map ["(",2;"-",5;"NUMBER",7;"expr",11]]
let kernelSymbols = Map [1,"expr";2,"(";3,"expr";4,")";5,"-";6,"expr";7,"NUMBER";8,"expr";9,"expr";10,"expr";11,"expr";12,"*";13,"+";14,"-";15,"/"]
let semantics = Map [-7,"s0 / s2";-6,"s0 - s2";-5,"//multiline test\r\ns0 + s2";-4,"s0 * s2";-3,"s0";-2,"-s1";-1,"s1"]
let declarations = ["NUMBER","float";"expr","float"]
open Expr.ExprToken
open System
let mappers:Map<int,(obj[]->obj)> = Map [
    -7,fun (ss:obj[]) ->
        // expr : expr "/" expr
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 / s2
        box result
    -6,fun (ss:obj[]) ->
        // expr : expr "-" expr
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 - s2
        box result
    -5,fun (ss:obj[]) ->
        // expr : expr "+" expr
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            //multiline test
            s0 + s2
        box result
    -4,fun (ss:obj[]) ->
        // expr : expr "*" expr
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 * s2
        box result
    -3,fun (ss:obj[]) ->
        // expr : NUMBER
        let s0 = unbox<float> ss.[0]
        let result:float =
            s0
        box result
    -2,fun (ss:obj[]) ->
        // expr : "-" expr
        let s1 = unbox<float> ss.[1]
        let result:float =
            -s1
        box result
    -1,fun (ss:obj[]) ->
        // expr : "(" expr ")"
        let s1 = unbox<float> ss.[1]
        let result:float =
            s1
        box result]
open FslexFsyacc.Runtime
let parser = Parser(productions, actions, kernelSymbols, mappers)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<float>