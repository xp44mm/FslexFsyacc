module FslexFsyacc.Fsyacc.RegularSymbolParseTable
let actions = [["regularSymbol",1;"{",13];["",0];["(",-1;")",-1;"*",-1;"+",-1;"?",-1;"ID",-1;"LITERAL",-1;"[",-1;"]",-1;"}",-1];["(",-2;")",-2;"*",-2;"+",-2;"?",-2;"ID",-2;"LITERAL",-2;"[",-2;"]",-2;"}",-2];["(",7;"ID",2;"LITERAL",3;"[",4;"atomic",19;"brackets",20;"parens",21;"repetition",22;"symbol",16;"{symbol+}",5];["(",7;"ID",2;"LITERAL",3;"[",4;"]",6;"atomic",19;"brackets",20;"parens",21;"repetition",22;"symbol",17];["(",-3;")",-3;"*",-3;"+",-3;"?",-3;"ID",-3;"LITERAL",-3;"[",-3;"]",-3;"}",-3];["(",7;"ID",2;"LITERAL",3;"[",4;"atomic",19;"brackets",20;"parens",21;"repetition",22;"symbol",16;"{symbol+}",8];["(",7;")",9;"ID",2;"LITERAL",3;"[",4;"atomic",19;"brackets",20;"parens",21;"repetition",22;"symbol",17];["(",-4;")",-4;"*",-4;"+",-4;"?",-4;"ID",-4;"LITERAL",-4;"[",-4;"]",-4;"}",-4];["(",-5;")",-5;"*",-5;"+",-5;"?",-5;"ID",-5;"LITERAL",-5;"[",-5;"]",-5;"}",-5];["(",-6;")",-6;"*",-6;"+",-6;"?",-6;"ID",-6;"LITERAL",-6;"[",-6;"]",-6;"}",-6];["(",-7;")",-7;"*",-7;"+",-7;"?",-7;"ID",-7;"LITERAL",-7;"[",-7;"]",-7;"}",-7];["(",7;"ID",2;"LITERAL",3;"[",4;"atomic",19;"brackets",20;"parens",21;"repetition",22;"symbol",14];["*",10;"+",11;"?",12;"quantifier",18;"}",15];["",-8];["(",-14;")",-14;"*",10;"+",11;"?",12;"ID",-14;"LITERAL",-14;"[",-14;"]",-14;"quantifier",18];["(",-15;")",-15;"*",10;"+",11;"?",12;"ID",-15;"LITERAL",-15;"[",-15;"]",-15;"quantifier",18];["(",-9;")",-9;"*",-9;"+",-9;"?",-9;"ID",-9;"LITERAL",-9;"[",-9;"]",-9;"}",-9];["(",-10;")",-10;"*",-10;"+",-10;"?",-10;"ID",-10;"LITERAL",-10;"[",-10;"]",-10;"}",-10];["(",-11;")",-11;"*",-11;"+",-11;"?",-11;"ID",-11;"LITERAL",-11;"[",-11;"]",-11;"}",-11];["(",-12;")",-12;"*",-12;"+",-12;"?",-12;"ID",-12;"LITERAL",-12;"[",-12;"]",-12;"}",-12];["(",-13;")",-13;"*",-13;"+",-13;"?",-13;"ID",-13;"LITERAL",-13;"[",-13;"]",-13;"}",-13]]
let closures = [[0,0,[];-8,0,[]];[0,1,[""]];[-1,1,["(";")";"*";"+";"?";"ID";"LITERAL";"[";"]";"}"]];[-2,1,["(";")";"*";"+";"?";"ID";"LITERAL";"[";"]";"}"]];[-1,0,[];-2,0,[];-3,0,[];-3,1,[];-4,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-13,0,[];-14,0,[];-15,0,[]];[-1,0,[];-2,0,[];-3,0,[];-3,2,[];-4,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-13,0,[];-15,1,[]];[-3,3,["(";")";"*";"+";"?";"ID";"LITERAL";"[";"]";"}"]];[-1,0,[];-2,0,[];-3,0,[];-4,0,[];-4,1,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-13,0,[];-14,0,[];-15,0,[]];[-1,0,[];-2,0,[];-3,0,[];-4,0,[];-4,2,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-13,0,[];-15,1,[]];[-4,3,["(";")";"*";"+";"?";"ID";"LITERAL";"[";"]";"}"]];[-5,1,["(";")";"*";"+";"?";"ID";"LITERAL";"[";"]";"}"]];[-6,1,["(";")";"*";"+";"?";"ID";"LITERAL";"[";"]";"}"]];[-7,1,["(";")";"*";"+";"?";"ID";"LITERAL";"[";"]";"}"]];[-1,0,[];-2,0,[];-3,0,[];-4,0,[];-8,1,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-13,0,[]];[-5,0,[];-6,0,[];-7,0,[];-8,2,[];-9,1,[]];[-8,3,[""]];[-5,0,[];-6,0,[];-7,0,[];-9,1,[];-14,1,["(";")";"ID";"LITERAL";"[";"]"]];[-5,0,[];-6,0,[];-7,0,[];-9,1,[];-15,2,["(";")";"ID";"LITERAL";"[";"]"]];[-9,2,["(";")";"*";"+";"?";"ID";"LITERAL";"[";"]";"}"]];[-10,1,["(";")";"*";"+";"?";"ID";"LITERAL";"[";"]";"}"]];[-11,1,["(";")";"*";"+";"?";"ID";"LITERAL";"[";"]";"}"]];[-12,1,["(";")";"*";"+";"?";"ID";"LITERAL";"[";"]";"}"]];[-13,1,["(";")";"*";"+";"?";"ID";"LITERAL";"[";"]";"}"]]]
open FslexFsyacc.Fsyacc
let rules:(string list*(obj list->obj))list = [
    ["regularSymbol";"{";"symbol";"}"],fun(ss:obj list)->
        let s1 = unbox<RegularSymbol> ss.[1]
        let result:RegularSymbol =
            s1
        box result
    ["symbol";"atomic"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:RegularSymbol =
            Atomic s0
        box result
    ["symbol";"repetition"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol*string> ss.[0]
        let result:RegularSymbol =
            match s0 with (f,q) ->
            Repetition(f,q)
        box result
    ["symbol";"brackets"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let result:RegularSymbol =
            Oneof s0
        box result
    ["symbol";"parens"],fun(ss:obj list)->
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
    ["repetition";"symbol";"quantifier"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:RegularSymbol*string =
            s0,s1
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
    ["brackets";"[";"{symbol+}";"]"],fun(ss:obj list)->
        let s1 = unbox<RegularSymbol list> ss.[1]
        let result:RegularSymbol list =
            List.rev s1
        box result
    ["{symbol+}";"symbol"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol> ss.[0]
        let result:RegularSymbol list =
            [s0]
        box result
    ["{symbol+}";"{symbol+}";"symbol"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let s1 = unbox<RegularSymbol> ss.[1]
        let result:RegularSymbol list =
            s1::s0
        box result
    ["parens";"(";"{symbol+}";")"],fun(ss:obj list)->
        let s1 = unbox<RegularSymbol list> ss.[1]
        let result:RegularSymbol list =
            List.rev s1
        box result
]
let unboxRoot =
    unbox<RegularSymbol>