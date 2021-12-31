module Expr.ExprParseTable
let header = "open Expr.ExprToken\r\nopen System"
let productions = [|0,[|"";"expr"|];-1,[|"expr";"-";"expr"|];-2,[|"expr";"NUMBER"|];-3,[|"expr";"expr";"*";"expr"|];-4,[|"expr";"expr";"+";"expr"|];-5,[|"expr";"expr";"-";"expr"|];-6,[|"expr";"expr";"/";"expr"|];-7,[|"expr";"lparen";"expr";"rparen"|];-8,[|"lparen";"("|];-9,[|"rparen";")"|]|]
let actions = [|0,[|"(",16;"-",2;"NUMBER",4;"expr",1;"lparen",14|];1,[|"",0;"*",10;"+",11;"-",12;"/",13|];2,[|"(",16;"-",2;"NUMBER",4;"expr",3;"lparen",14|];3,[|"",-1;")",-1;"*",-1;"+",-1;"-",-1;"/",-1|];4,[|"",-2;")",-2;"*",-2;"+",-2;"-",-2;"/",-2|];5,[|"",-3;")",-3;"*",-3;"+",-3;"-",-3;"/",-3|];6,[|"",-4;")",-4;"*",10;"+",-4;"-",-4;"/",13|];7,[|"",-5;")",-5;"*",10;"+",-5;"-",-5;"/",13|];8,[|"",-6;")",-6;"*",-6;"+",-6;"-",-6;"/",-6|];9,[|")",17;"*",10;"+",11;"-",12;"/",13;"rparen",15|];10,[|"(",16;"-",2;"NUMBER",4;"expr",5;"lparen",14|];11,[|"(",16;"-",2;"NUMBER",4;"expr",6;"lparen",14|];12,[|"(",16;"-",2;"NUMBER",4;"expr",7;"lparen",14|];13,[|"(",16;"-",2;"NUMBER",4;"expr",8;"lparen",14|];14,[|"(",16;"-",2;"NUMBER",4;"expr",9;"lparen",14|];15,[|"",-7;")",-7;"*",-7;"+",-7;"-",-7;"/",-7|];16,[|"(",-8;"-",-8;"NUMBER",-8|];17,[|"",-9;")",-9;"*",-9;"+",-9;"-",-9;"/",-9|]|]
let kernelSymbols = [|1,"expr";2,"-";3,"expr";4,"NUMBER";5,"expr";6,"expr";7,"expr";8,"expr";9,"expr";10,"*";11,"+";12,"-";13,"/";14,"lparen";15,"rparen";16,"(";17,")"|]
let semantics = [|-1,"-s1";-2,"s0";-3,"s0 * s2";-4,"//multiline test\r\ns0 + s2";-5,"s0 - s2";-6,"s0 / s2";-7,"s1";-8,"";-9,"(*this semantic return unit*)"|]
let declarations = [|"NUMBER","float";"expr","float"|]
open Expr.ExprToken
open System
let mappers:(int*(obj[]->obj))[] = [|
    -1,fun (ss:obj[]) ->
        // expr -> "-" expr
        let s1 = unbox<float> ss.[1]
        let result:float =
            -s1
        box result
    -2,fun (ss:obj[]) ->
        // expr -> NUMBER
        let s0 = unbox<float> ss.[0]
        let result:float =
            s0
        box result
    -3,fun (ss:obj[]) ->
        // expr -> expr "*" expr
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 * s2
        box result
    -4,fun (ss:obj[]) ->
        // expr -> expr "+" expr
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            //multiline test
            s0 + s2
        box result
    -5,fun (ss:obj[]) ->
        // expr -> expr "-" expr
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 - s2
        box result
    -6,fun (ss:obj[]) ->
        // expr -> expr "/" expr
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 / s2
        box result
    -7,fun (ss:obj[]) ->
        // expr -> lparen expr rparen
        let s1 = unbox<float> ss.[1]
        let result:float =
            s1
        box result
    -8,fun (ss:obj[]) ->
        // lparen -> "("
        null
    -9,fun (ss:obj[]) ->
        // rparen -> ")"
        (*this semantic return unit*)
        null|]
open FslexFsyacc.Runtime
let parser = Parser(productions, actions, kernelSymbols, mappers)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<float>