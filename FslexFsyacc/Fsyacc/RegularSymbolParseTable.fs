﻿module FslexFsyacc.Fsyacc.RegularSymbolParseTable
let actions = [["(",7;"ID",2;"LITERAL",3;"[",4;"atomic",13;"brackets",14;"parens",15;"regularSymbol",1;"repetition",16];["",0;"*",10;"+",11;"?",12;"quantifier",19];["",-1;"(",-1;")",-1;"*",-1;"+",-1;"?",-1;"ID",-1;"LITERAL",-1;"[",-1;"]",-1];["",-2;"(",-2;")",-2;"*",-2;"+",-2;"?",-2;"ID",-2;"LITERAL",-2;"[",-2;"]",-2];["(",7;"ID",2;"LITERAL",3;"[",4;"atomic",13;"brackets",14;"parens",15;"regularSymbol",17;"repetition",16;"{regularSymbol+}",5];["(",7;"ID",2;"LITERAL",3;"[",4;"]",6;"atomic",13;"brackets",14;"parens",15;"regularSymbol",18;"repetition",16];["",-3;"(",-3;")",-3;"*",-3;"+",-3;"?",-3;"ID",-3;"LITERAL",-3;"[",-3;"]",-3];["(",7;"ID",2;"LITERAL",3;"[",4;"atomic",13;"brackets",14;"parens",15;"regularSymbol",17;"repetition",16;"{regularSymbol+}",8];["(",7;")",9;"ID",2;"LITERAL",3;"[",4;"atomic",13;"brackets",14;"parens",15;"regularSymbol",18;"repetition",16];["",-4;"(",-4;")",-4;"*",-4;"+",-4;"?",-4;"ID",-4;"LITERAL",-4;"[",-4;"]",-4];["",-5;"(",-5;")",-5;"*",-5;"+",-5;"?",-5;"ID",-5;"LITERAL",-5;"[",-5;"]",-5];["",-6;"(",-6;")",-6;"*",-6;"+",-6;"?",-6;"ID",-6;"LITERAL",-6;"[",-6;"]",-6];["",-7;"(",-7;")",-7;"*",-7;"+",-7;"?",-7;"ID",-7;"LITERAL",-7;"[",-7;"]",-7];["",-8;"(",-8;")",-8;"*",-8;"+",-8;"?",-8;"ID",-8;"LITERAL",-8;"[",-8;"]",-8];["",-9;"(",-9;")",-9;"*",-9;"+",-9;"?",-9;"ID",-9;"LITERAL",-9;"[",-9;"]",-9];["",-10;"(",-10;")",-10;"*",-10;"+",-10;"?",-10;"ID",-10;"LITERAL",-10;"[",-10;"]",-10];["",-11;"(",-11;")",-11;"*",-11;"+",-11;"?",-11;"ID",-11;"LITERAL",-11;"[",-11;"]",-11];["(",-13;")",-13;"*",10;"+",11;"?",12;"ID",-13;"LITERAL",-13;"[",-13;"]",-13;"quantifier",19];["(",-14;")",-14;"*",10;"+",11;"?",12;"ID",-14;"LITERAL",-14;"[",-14;"]",-14;"quantifier",19];["",-12;"(",-12;")",-12;"*",-12;"+",-12;"?",-12;"ID",-12;"LITERAL",-12;"[",-12;"]",-12]]
let closures = [[0,0,[];-1,0,[];-2,0,[];-3,0,[];-4,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[]];[0,1,[""];-5,0,[];-6,0,[];-7,0,[];-12,1,[]];[-1,1,["";"(";")";"*";"+";"?";"ID";"LITERAL";"[";"]"]];[-2,1,["";"(";")";"*";"+";"?";"ID";"LITERAL";"[";"]"]];[-1,0,[];-2,0,[];-3,0,[];-3,1,[];-4,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-13,0,[];-14,0,[]];[-1,0,[];-2,0,[];-3,0,[];-3,2,[];-4,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-14,1,[]];[-3,3,["";"(";")";"*";"+";"?";"ID";"LITERAL";"[";"]"]];[-1,0,[];-2,0,[];-3,0,[];-4,0,[];-4,1,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-13,0,[];-14,0,[]];[-1,0,[];-2,0,[];-3,0,[];-4,0,[];-4,2,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-14,1,[]];[-4,3,["";"(";")";"*";"+";"?";"ID";"LITERAL";"[";"]"]];[-5,1,["";"(";")";"*";"+";"?";"ID";"LITERAL";"[";"]"]];[-6,1,["";"(";")";"*";"+";"?";"ID";"LITERAL";"[";"]"]];[-7,1,["";"(";")";"*";"+";"?";"ID";"LITERAL";"[";"]"]];[-8,1,["";"(";")";"*";"+";"?";"ID";"LITERAL";"[";"]"]];[-9,1,["";"(";")";"*";"+";"?";"ID";"LITERAL";"[";"]"]];[-10,1,["";"(";")";"*";"+";"?";"ID";"LITERAL";"[";"]"]];[-11,1,["";"(";")";"*";"+";"?";"ID";"LITERAL";"[";"]"]];[-5,0,[];-6,0,[];-7,0,[];-12,1,[];-13,1,["(";")";"ID";"LITERAL";"[";"]"]];[-5,0,[];-6,0,[];-7,0,[];-12,1,[];-14,2,["(";")";"ID";"LITERAL";"[";"]"]];[-12,2,["";"(";")";"*";"+";"?";"ID";"LITERAL";"[";"]"]]]
open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc
open FslexFsyacc.Fsyacc.RegularSymbolUtils
type token = FsyaccToken
let rules:(string list*(obj list->obj))list = [
    ["regularSymbol";"atomic"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:RegularSymbol =
            Atomic s0
        box result
    ["regularSymbol";"repetition"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol*string> ss.[0]
        let result:RegularSymbol =
            match s0 with (f,q) ->
            Repetition(f,q)
        box result
    ["regularSymbol";"brackets"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let result:RegularSymbol =
            Oneof s0
        box result
    ["regularSymbol";"parens"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let result:RegularSymbol =
            Chain s0
        box result
    ["atomic";"ID"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["atomic";"LITERAL"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["repetition";"regularSymbol";"quantifier"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:RegularSymbol*string =
            (s0,s1)
        box result
    ["quantifier";"?"],fun(ss:obj list)->
        let result:string =
            "?"
        box result
    ["quantifier";"+"],fun(ss:obj list)->
        let result:string =
            "+"
        box result
    ["quantifier";"*"],fun(ss:obj list)->
        let result:string =
            "*"
        box result
    ["brackets";"[";"{regularSymbol+}";"]"],fun(ss:obj list)->
        let s1 = unbox<RegularSymbol list> ss.[1]
        let result:RegularSymbol list =
            List.rev s1
        box result
    ["{regularSymbol+}";"regularSymbol"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol> ss.[0]
        let result:RegularSymbol list =
            [s0]
        box result
    ["{regularSymbol+}";"{regularSymbol+}";"regularSymbol"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let s1 = unbox<RegularSymbol> ss.[1]
        let result:RegularSymbol list =
            s1::s0
        box result
    ["parens";"(";"{regularSymbol+}";")"],fun(ss:obj list)->
        let s1 = unbox<RegularSymbol list> ss.[1]
        let result:RegularSymbol list =
            List.rev s1
        box result
]
let parser = Parser<token>(rules,actions,closures,getTag,getLexeme)
let parse(tokens:seq<token>) =
    tokens
    |> parser.parse
    |> unbox<RegularSymbol>