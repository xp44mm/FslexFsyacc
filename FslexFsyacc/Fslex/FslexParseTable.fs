module FslexFsyacc.Fslex.FslexParseTable
let actions = [["HEADER",24;"file",1];["",0];["%%",-1;"&",-1;")",-1;"*",-1;"+",-1;"/",-1;"?",-1;"CAP",-1;"SEMANTIC",-1;"]",-1;"|",-1];["%%",-2;"&",-2;")",-2;"*",-2;"+",-2;"/",-2;"?",-2;"CAP",-2;"SEMANTIC",-2;"]",-2;"|",-2];["=",5];["(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",6];["%%",-3;"&",19;"*",20;"+",21;"?",22;"CAP",-3;"|",23];["(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",8];["&",19;")",9;"*",20;"+",21;"?",22;"|",23];["%%",-4;"&",-4;")",-4;"*",-4;"+",-4;"/",-4;"?",-4;"CAP",-4;"SEMANTIC",-4;"|",-4];["%%",-5;"&",-5;")",-5;"*",-5;"+",-5;"/",-5;"?",-5;"CAP",-5;"SEMANTIC",-5;"|",-5];["ID",2;"LITERAL",3;"atomic",32;"{atomic+}",12];["&",33;"]",13];["%%",-6;"&",-6;")",-6;"*",-6;"+",-6;"/",-6;"?",-6;"CAP",-6;"SEMANTIC",-6;"|",-6];["%%",-7;"&",-7;")",-7;"*",-7;"+",-7;"/",-7;"?",-7;"CAP",-7;"SEMANTIC",-7;"|",-7];["%%",-8;"&",-8;")",-8;"*",20;"+",21;"/",-8;"?",22;"CAP",-8;"SEMANTIC",-8;"|",-8];["%%",-12;"&",19;")",-12;"*",20;"+",21;"/",-12;"?",22;"CAP",-12;"SEMANTIC",-12;"|",-12];["&",19;"*",20;"+",21;"/",29;"?",22;"SEMANTIC",31;"|",23];["&",19;"*",20;"+",21;"?",22;"SEMANTIC",30;"|",23];["(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",15];["%%",-9;"&",-9;")",-9;"*",-9;"+",-9;"/",-9;"?",-9;"CAP",-9;"SEMANTIC",-9;"|",-9];["%%",-10;"&",-10;")",-10;"*",-10;"+",-10;"/",-10;"?",-10;"CAP",-10;"SEMANTIC",-10;"|",-10];["%%",-11;"&",-11;")",-11;"*",-11;"+",-11;"/",-11;"?",-11;"CAP",-11;"SEMANTIC",-11;"|",-11];["(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",16];["(",7;"CAP",4;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"definition",35;"expr",17;"rule",37;"{definition+}",25;"{rule+}",28];["%%",26;"CAP",4;"definition",36];["(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",17;"rule",37;"{rule+}",27];["",-13;"(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",17;"rule",38];["",-14;"(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",17;"rule",38];["(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",18];["",-15;"(",-15;"HOLE",-15;"ID",-15;"LITERAL",-15;"[",-15];["",-16;"(",-16;"HOLE",-16;"ID",-16;"LITERAL",-16;"[",-16];["&",-17;"]",-17];["ID",2;"LITERAL",3;"atomic",34];["&",-18;"]",-18];["%%",-19;"CAP",-19];["%%",-20;"CAP",-20];["",-21;"(",-21;"HOLE",-21;"ID",-21;"LITERAL",-21;"[",-21];["",-22;"(",-22;"HOLE",-22;"ID",-22;"LITERAL",-22;"[",-22]]
let closures = [[0,0,[];-13,0,[];-14,0,[]];[0,1,[""]];[-1,1,["%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"]";"|"]];[-2,1,["%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"]";"|"]];[-3,1,[]];[-1,0,[];-2,0,[];-3,2,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[]];[-3,3,["%%";"CAP"];-8,1,[];-9,1,[];-10,1,[];-11,1,[];-12,1,[]];[-1,0,[];-2,0,[];-4,0,[];-4,1,[];-5,0,[];-6,0,[];-7,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[]];[-4,2,[];-8,1,[];-9,1,[];-10,1,[];-11,1,[];-12,1,[]];[-4,3,["%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"]];[-5,1,["%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"]];[-1,0,[];-2,0,[];-6,1,[];-17,0,[];-18,0,[]];[-6,2,[];-18,1,[]];[-6,3,["%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"]];[-7,1,["%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"]];[-8,3,["%%";"&";")";"/";"CAP";"SEMANTIC";"|"];-9,1,[];-10,1,[];-11,1,[]];[-8,1,[];-9,1,[];-10,1,[];-11,1,[];-12,3,["%%";")";"/";"CAP";"SEMANTIC";"|"]];[-8,1,[];-9,1,[];-10,1,[];-11,1,[];-12,1,[];-15,1,[];-16,1,[]];[-8,1,[];-9,1,[];-10,1,[];-11,1,[];-12,1,[];-15,3,[]];[-1,0,[];-2,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[];-8,0,[];-8,2,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[]];[-9,2,["%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"]];[-10,2,["%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"]];[-11,2,["%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"]];[-1,0,[];-2,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-12,2,[]];[-1,0,[];-2,0,[];-3,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-13,1,[];-14,1,[];-15,0,[];-16,0,[];-19,0,[];-20,0,[];-21,0,[];-22,0,[]];[-3,0,[];-13,2,[];-20,1,[]];[-1,0,[];-2,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-13,3,[];-15,0,[];-16,0,[];-21,0,[];-22,0,[]];[-1,0,[];-2,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-13,4,[""];-15,0,[];-16,0,[];-22,1,[]];[-1,0,[];-2,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-14,2,[""];-15,0,[];-16,0,[];-22,1,[]];[-1,0,[];-2,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[];-15,2,[]];[-15,4,["";"(";"HOLE";"ID";"LITERAL";"["]];[-16,2,["";"(";"HOLE";"ID";"LITERAL";"["]];[-17,1,["&";"]"]];[-1,0,[];-2,0,[];-18,2,[]];[-18,3,["&";"]"]];[-19,1,["%%";"CAP"]];[-20,2,["%%";"CAP"]];[-21,1,["";"(";"HOLE";"ID";"LITERAL";"["]];[-22,2,["";"(";"HOLE";"ID";"LITERAL";"["]]]
open FslexFsyacc.Lex
let clazz s = s |> List.rev |> List.reduce(fun a b -> Either(a,b))
let rules:(string list*(obj list->obj))list = [
    ["file";"HEADER";"{definition+}";"%%";"{rule+}"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*RegularExpression<string>)list> ss.[1]
        let s3 = unbox<(RegularExpression<string>list*string)list> ss.[3]
        let result:string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list =
            s0,List.rev s1,List.rev s3
        box result
    ["file";"HEADER";"{rule+}"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(RegularExpression<string>list*string)list> ss.[1]
        let result:string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list =
            s0,[],List.rev s1
        box result
    ["{definition+}";"{definition+}";"definition"],fun(ss:obj list)->
        let s0 = unbox<(string*RegularExpression<string>)list> ss.[0]
        let s1 = unbox<string*RegularExpression<string>> ss.[1]
        let result:(string*RegularExpression<string>)list =
            s1::s0
        box result
    ["{definition+}";"definition"],fun(ss:obj list)->
        let s0 = unbox<string*RegularExpression<string>> ss.[0]
        let result:(string*RegularExpression<string>)list =
            [s0]
        box result
    ["definition";"CAP";"=";"expr"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:string*RegularExpression<string> =
            s0,s2
        box result
    ["expr";"atomic"],fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            s0
        box result
    ["expr";"HOLE"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:RegularExpression<string> =
            Hole s0
        box result
    ["expr";"expr";"*"],fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            Natural s0
        box result
    ["expr";"expr";"+"],fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            Plural s0
        box result
    ["expr";"expr";"?"],fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            Optional s0
        box result
    ["expr";"expr";"|";"expr"],fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:RegularExpression<string> =
            Either(s0,s2)
        box result
    ["expr";"expr";"&";"expr"],fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:RegularExpression<string> =
            Both(s0,s2)
        box result
    ["expr";"(";"expr";")"],fun(ss:obj list)->
        let s1 = unbox<RegularExpression<string>> ss.[1]
        let result:RegularExpression<string> =
            s1
        box result
    ["expr";"[";"{atomic+}";"]"],fun(ss:obj list)->
        let s1 = unbox<RegularExpression<string> list> ss.[1]
        let result:RegularExpression<string> =
            clazz s1
        box result
    ["atomic";"ID"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:RegularExpression<string> =
            Atomic s0
        box result
    ["atomic";"LITERAL"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:RegularExpression<string> =
            Atomic s0
        box result
    ["{atomic+}";"{atomic+}";"&";"atomic"],fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string> list> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:RegularExpression<string> list =
            s2::s0
        box result
    ["{atomic+}";"atomic"],fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> list =
            [s0]
        box result
    ["{rule+}";"{rule+}";"rule"],fun(ss:obj list)->
        let s0 = unbox<(RegularExpression<string>list*string)list> ss.[0]
        let s1 = unbox<RegularExpression<string>list*string> ss.[1]
        let result:(RegularExpression<string>list*string)list =
            s1::s0
        box result
    ["{rule+}";"rule"],fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>list*string> ss.[0]
        let result:(RegularExpression<string>list*string)list =
            [s0]
        box result
    ["rule";"expr";"/";"expr";"SEMANTIC"],fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let s3 = unbox<string> ss.[3]
        let result:RegularExpression<string>list*string =
            [s0;s2],s3
        box result
    ["rule";"expr";"SEMANTIC"],fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:RegularExpression<string>list*string =
            [s0],s1
        box result
]
let unboxRoot =
    unbox<string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list>
let theoryParser = FslexFsyacc.Runtime.TheoryParser.create(rules, actions, closures)
let stateSymbolPairs = theoryParser.getStateSymbolPairs()