module Expr.ExprParseTable2
let productions = [|["";"expr"];["expr";"(";"expr";")"];["expr";"-";"expr"];["expr";"NUMBER"];["expr";"expr";"*";"expr"];["expr";"expr";"+";"expr"];["expr";"expr";"-";"expr"];["expr";"expr";"/";"expr"]|]
let closures = [|[|0,0,[||];-1,0,[||];-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||]|];[|0,1,[|""|];-4,1,[||];-5,1,[||];-6,1,[||];-7,1,[||]|];[|-1,0,[||];-1,1,[||];-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||]|];[|-1,2,[||];-4,1,[||];-5,1,[||];-6,1,[||];-7,1,[||]|];[|-1,3,[|"";")";"*";"+";"-";"/"|]|];[|-1,0,[||];-2,0,[||];-2,1,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||]|];[|-2,2,[|"";")";"*";"+";"-";"/"|]|];[|-3,1,[|"";")";"*";"+";"-";"/"|]|];[|-4,3,[|"";")";"*";"+";"-";"/"|]|];[|-4,1,[||];-5,3,[|"";")";"+";"-"|];-7,1,[||]|];[|-4,1,[||];-6,3,[|"";")";"+";"-"|];-7,1,[||]|];[|-7,3,[|"";")";"*";"+";"-";"/"|]|];[|-1,0,[||];-2,0,[||];-3,0,[||];-4,0,[||];-4,2,[||];-5,0,[||];-6,0,[||];-7,0,[||]|];[|-1,0,[||];-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-5,2,[||];-6,0,[||];-7,0,[||]|];[|-1,0,[||];-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-6,2,[||];-7,0,[||]|];[|-1,0,[||];-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-7,2,[||]|]|]
let actions = [|[|"(",2;"-",5;"NUMBER",7;"expr",1|];[|"",0;"*",12;"+",13;"-",14;"/",15|];[|"(",2;"-",5;"NUMBER",7;"expr",3|];[|")",4;"*",12;"+",13;"-",14;"/",15|];[|"",-1;")",-1;"*",-1;"+",-1;"-",-1;"/",-1|];[|"(",2;"-",5;"NUMBER",7;"expr",6|];[|"",-2;")",-2;"*",-2;"+",-2;"-",-2;"/",-2|];[|"",-3;")",-3;"*",-3;"+",-3;"-",-3;"/",-3|];[|"",-4;")",-4;"*",-4;"+",-4;"-",-4;"/",-4|];[|"",-5;")",-5;"*",12;"+",-5;"-",-5;"/",15|];[|"",-6;")",-6;"*",12;"+",-6;"-",-6;"/",15|];[|"",-7;")",-7;"*",-7;"+",-7;"-",-7;"/",-7|];[|"(",2;"-",5;"NUMBER",7;"expr",8|];[|"(",2;"-",5;"NUMBER",7;"expr",9|];[|"(",2;"-",5;"NUMBER",7;"expr",10|];[|"(",2;"-",5;"NUMBER",7;"expr",11|]|]
let header = "open Expr.ExprToken"
let semantics = [|"s1";"-s1";"s0";"s0 * s2";"s0 + s2";"s0 - s2";"s0 / s2"|]
let declarations = [|"NUMBER","float";"expr","float"|]
open Expr.ExprToken
let mappers:(obj[]->obj)[] = [|
    fun (ss:obj[]) ->
        // expr -> "(" expr ")"
        let s1 = unbox<float> ss.[1]
        let result:float =
            s1
        box result
    fun (ss:obj[]) ->
        // expr -> "-" expr
        let s1 = unbox<float> ss.[1]
        let result:float =
            -s1
        box result
    fun (ss:obj[]) ->
        // expr -> NUMBER
        let s0 = unbox<float> ss.[0]
        let result:float =
            s0
        box result
    fun (ss:obj[]) ->
        // expr -> expr "*" expr
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 * s2
        box result
    fun (ss:obj[]) ->
        // expr -> expr "+" expr
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 + s2
        box result
    fun (ss:obj[]) ->
        // expr -> expr "-" expr
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 - s2
        box result
    fun (ss:obj[]) ->
        // expr -> expr "/" expr
        let s0 = unbox<float> ss.[0]
        let s2 = unbox<float> ss.[2]
        let result:float =
            s0 / s2
        box result
|]
open FslexFsyacc.Runtime
let parser = Parser2(productions, closures, actions, mappers)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<float>