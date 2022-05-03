module Interpolation.PlaceholderParseTable
let rules = [|["Placeholder";"expr";"}"],"s0";["expr";"expr";"+";"expr"],"BinaryExpression (s0 ,\"+\", s2)";["expr";"expr";"-";"expr"],"BinaryExpression (s0 ,\"-\", s2)";["expr";"expr";"*";"expr"],"BinaryExpression (s0 ,\"*\", s2)";["expr";"expr";"/";"expr"],"BinaryExpression (s0 ,\"/\", s2)";["expr";"(";"expr";")"],"s1";["expr";"-";"expr"],"PrefixExpression(\"-\", s1)";["expr";"NUMBER"],"Number (float s0)"|]
let actions = [|[|"(",4;"-",7;"NUMBER",9;"Placeholder",1;"expr",2|];[|"",0|];[|"*",14;"+",15;"-",16;"/",17;"}",3|];[|"",-1|];[|"(",4;"-",7;"NUMBER",9;"expr",5|];[|")",6;"*",14;"+",15;"-",16;"/",17|];[|")",-2;"*",-2;"+",-2;"-",-2;"/",-2;"}",-2|];[|"(",4;"-",7;"NUMBER",9;"expr",8|];[|")",-3;"*",-3;"+",-3;"-",-3;"/",-3;"}",-3|];[|")",-4;"*",-4;"+",-4;"-",-4;"/",-4;"}",-4|];[|")",-5;"*",-5;"+",-5;"-",-5;"/",-5;"}",-5|];[|")",-6;"*",14;"+",-6;"-",-6;"/",17;"}",-6|];[|")",-7;"*",14;"+",-7;"-",-7;"/",17;"}",-7|];[|")",-8;"*",-8;"+",-8;"-",-8;"/",-8;"}",-8|];[|"(",4;"-",7;"NUMBER",9;"expr",10|];[|"(",4;"-",7;"NUMBER",9;"expr",11|];[|"(",4;"-",7;"NUMBER",9;"expr",12|];[|"(",4;"-",7;"NUMBER",9;"expr",13|]|]
let closures = [|[|0,0,[||];-1,0,[||];-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||]|];[|0,1,[|""|]|];[|-1,1,[||];-5,1,[||];-6,1,[||];-7,1,[||];-8,1,[||]|];[|-1,2,[|""|]|];[|-2,0,[||];-2,1,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||]|];[|-2,2,[||];-5,1,[||];-6,1,[||];-7,1,[||];-8,1,[||]|];[|-2,3,[|")";"*";"+";"-";"/";"}"|]|];[|-2,0,[||];-3,0,[||];-3,1,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||]|];[|-3,2,[|")";"*";"+";"-";"/";"}"|]|];[|-4,1,[|")";"*";"+";"-";"/";"}"|]|];[|-5,3,[|")";"*";"+";"-";"/";"}"|]|];[|-5,1,[||];-6,3,[|")";"+";"-";"}"|];-8,1,[||]|];[|-5,1,[||];-7,3,[|")";"+";"-";"}"|];-8,1,[||]|];[|-8,3,[|")";"*";"+";"-";"/";"}"|]|];[|-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-5,2,[||];-6,0,[||];-7,0,[||];-8,0,[||]|];[|-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-6,2,[||];-7,0,[||];-8,0,[||]|];[|-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-7,2,[||];-8,0,[||]|];[|-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-8,2,[||]|]|]
let header = "open Interpolation.PlaceholderUtils\r\nopen FslexFsyacc.Runtime\r\ntype token = Position<Token>"
let declarations = [|"NUMBER","string";"expr","Expression";"Placeholder","Expression"|]
open Interpolation.PlaceholderUtils
open FslexFsyacc.Runtime
type token = Position<Token>
let fxRules:(string list*(obj[]->obj))[] = [|
    ["Placeholder";"expr";"}"],fun (ss:obj[]) ->
            let s0 = unbox<Expression> ss.[0]
            let result:Expression =
                s0
            box result
    ["expr";"expr";"+";"expr"],fun (ss:obj[]) ->
            let s0 = unbox<Expression> ss.[0]
            let s2 = unbox<Expression> ss.[2]
            let result:Expression =
                BinaryExpression (s0 ,"+", s2)
            box result
    ["expr";"expr";"-";"expr"],fun (ss:obj[]) ->
            let s0 = unbox<Expression> ss.[0]
            let s2 = unbox<Expression> ss.[2]
            let result:Expression =
                BinaryExpression (s0 ,"-", s2)
            box result
    ["expr";"expr";"*";"expr"],fun (ss:obj[]) ->
            let s0 = unbox<Expression> ss.[0]
            let s2 = unbox<Expression> ss.[2]
            let result:Expression =
                BinaryExpression (s0 ,"*", s2)
            box result
    ["expr";"expr";"/";"expr"],fun (ss:obj[]) ->
            let s0 = unbox<Expression> ss.[0]
            let s2 = unbox<Expression> ss.[2]
            let result:Expression =
                BinaryExpression (s0 ,"/", s2)
            box result
    ["expr";"(";"expr";")"],fun (ss:obj[]) ->
            let s1 = unbox<Expression> ss.[1]
            let result:Expression =
                s1
            box result
    ["expr";"-";"expr"],fun (ss:obj[]) ->
            let s1 = unbox<Expression> ss.[1]
            let result:Expression =
                PrefixExpression("-", s1)
            box result
    ["expr";"NUMBER"],fun (ss:obj[]) ->
            let s0 = unbox<string> ss.[0]
            let result:Expression =
                Number (float s0)
            box result
|]
open FslexFsyacc.Runtime
let parser = XParser<token>(fxRules,actions,closures,getTag,getLexeme)
let parse(tokens:seq<token>) =
    tokens
    |> parser.parse
    |> unbox<Expression>