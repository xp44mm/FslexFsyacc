module FslexFsyacc.Fslex.FslexParseTable
let tokens = set ["%%";"&";"(";")";"*";"+";"/";"=";"?";"CAP";"HEADER";"HOLE";"ID";"LITERAL";"REDUCER";"[";"]";"|"]
let kernels = [[0,0];[0,1];[-1,1];[-2,1];[-3,1];[-3,2];[-3,3;-8,1;-9,1;-10,1;-11,1;-12,1];[-4,1];[-4,2;-8,1;-9,1;-10,1;-11,1;-12,1];[-4,3];[-5,1];[-6,1];[-6,2;-18,1];[-6,3];[-7,1];[-8,1;-8,3;-9,1;-10,1;-11,1;-12,1];[-8,1;-9,1;-10,1;-11,1;-12,1;-12,3];[-8,1;-9,1;-10,1;-11,1;-12,1;-15,1;-16,1];[-8,1;-9,1;-10,1;-11,1;-12,1;-15,3];[-8,2];[-9,2];[-10,2];[-11,2];[-12,2];[-13,1;-14,1];[-13,2;-20,1];[-13,3];[-13,4;-22,1];[-14,2;-22,1];[-15,2];[-15,4];[-16,2];[-17,1];[-18,2];[-18,3];[-19,1];[-20,2];[-21,1];[-22,2]]
let kernelSymbols = ["";"file";"ID";"LITERAL";"CAP";"=";"expr";"(";"expr";")";"HOLE";"[";"{atomic+}";"]";"atomic";"expr";"expr";"expr";"expr";"&";"*";"+";"?";"|";"HEADER";"{definition+}";"%%";"{rule+}";"{rule+}";"/";"REDUCER";"REDUCER";"atomic";"&";"atomic";"definition";"definition";"rule";"rule"]
let actions = [["HEADER",24;"file",1];["",0];["%%",-1;"&",-1;")",-1;"*",-1;"+",-1;"/",-1;"?",-1;"CAP",-1;"REDUCER",-1;"]",-1;"|",-1];["%%",-2;"&",-2;")",-2;"*",-2;"+",-2;"/",-2;"?",-2;"CAP",-2;"REDUCER",-2;"]",-2;"|",-2];["=",5];["(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",6];["%%",-3;"&",19;"*",20;"+",21;"?",22;"CAP",-3;"|",23];["(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",8];["&",19;")",9;"*",20;"+",21;"?",22;"|",23];["%%",-4;"&",-4;")",-4;"*",-4;"+",-4;"/",-4;"?",-4;"CAP",-4;"REDUCER",-4;"|",-4];["%%",-5;"&",-5;")",-5;"*",-5;"+",-5;"/",-5;"?",-5;"CAP",-5;"REDUCER",-5;"|",-5];["ID",2;"LITERAL",3;"atomic",32;"{atomic+}",12];["&",33;"]",13];["%%",-6;"&",-6;")",-6;"*",-6;"+",-6;"/",-6;"?",-6;"CAP",-6;"REDUCER",-6;"|",-6];["%%",-7;"&",-7;")",-7;"*",-7;"+",-7;"/",-7;"?",-7;"CAP",-7;"REDUCER",-7;"|",-7];["%%",-8;"&",-8;")",-8;"*",20;"+",21;"/",-8;"?",22;"CAP",-8;"REDUCER",-8;"|",-8];["%%",-12;"&",19;")",-12;"*",20;"+",21;"/",-12;"?",22;"CAP",-12;"REDUCER",-12;"|",-12];["&",19;"*",20;"+",21;"/",29;"?",22;"REDUCER",31;"|",23];["&",19;"*",20;"+",21;"?",22;"REDUCER",30;"|",23];["(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",15];["%%",-9;"&",-9;")",-9;"*",-9;"+",-9;"/",-9;"?",-9;"CAP",-9;"REDUCER",-9;"|",-9];["%%",-10;"&",-10;")",-10;"*",-10;"+",-10;"/",-10;"?",-10;"CAP",-10;"REDUCER",-10;"|",-10];["%%",-11;"&",-11;")",-11;"*",-11;"+",-11;"/",-11;"?",-11;"CAP",-11;"REDUCER",-11;"|",-11];["(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",16];["(",7;"CAP",4;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"definition",35;"expr",17;"rule",37;"{definition+}",25;"{rule+}",28];["%%",26;"CAP",4;"definition",36];["(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",17;"rule",37;"{rule+}",27];["",-13;"(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",17;"rule",38];["",-14;"(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",17;"rule",38];["(",7;"HOLE",10;"ID",2;"LITERAL",3;"[",11;"atomic",14;"expr",18];["",-15;"(",-15;"HOLE",-15;"ID",-15;"LITERAL",-15;"[",-15];["",-16;"(",-16;"HOLE",-16;"ID",-16;"LITERAL",-16;"[",-16];["&",-17;"]",-17];["ID",2;"LITERAL",3;"atomic",34];["&",-18;"]",-18];["%%",-19;"CAP",-19];["%%",-20;"CAP",-20];["",-21;"(",-21;"HOLE",-21;"ID",-21;"LITERAL",-21;"[",-21];["",-22;"(",-22;"HOLE",-22;"ID",-22;"LITERAL",-22;"[",-22]]
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.Lex
let clazz s = s |> List.rev |> List.reduce(fun a b -> Either(a,b))
let rules : list<string list*(obj list->obj)> = [
    ["";"file"], fun(ss:obj list)-> ss.[0]
    ["atomic";"ID"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:RegularExpression<string> =
            Atomic s0
        box result
    ["atomic";"LITERAL"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:RegularExpression<string> =
            Atomic s0
        box result
    ["definition";"CAP";"=";"expr"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:string*RegularExpression<string> =
            s0,s2
        box result
    ["expr";"(";"expr";")"], fun(ss:obj list)->
        let s1 = unbox<RegularExpression<string>> ss.[1]
        let result:RegularExpression<string> =
            s1
        box result
    ["expr";"HOLE"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:RegularExpression<string> =
            Hole s0
        box result
    ["expr";"[";"{atomic+}";"]"], fun(ss:obj list)->
        let s1 = unbox<RegularExpression<string> list> ss.[1]
        let result:RegularExpression<string> =
            clazz s1
        box result
    ["expr";"atomic"], fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            s0
        box result
    ["expr";"expr";"&";"expr"], fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:RegularExpression<string> =
            Both(s0,s2)
        box result
    ["expr";"expr";"*"], fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            Natural s0
        box result
    ["expr";"expr";"+"], fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            Plural s0
        box result
    ["expr";"expr";"?"], fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            Optional s0
        box result
    ["expr";"expr";"|";"expr"], fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:RegularExpression<string> =
            Either(s0,s2)
        box result
    ["file";"HEADER";"{definition+}";"%%";"{rule+}"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*RegularExpression<string>)list> ss.[1]
        let s3 = unbox<(RegularExpression<string>list*string)list> ss.[3]
        let result:string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list =
            s0,List.rev s1,List.rev s3
        box result
    ["file";"HEADER";"{rule+}"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(RegularExpression<string>list*string)list> ss.[1]
        let result:string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list =
            s0,[],List.rev s1
        box result
    ["rule";"expr";"/";"expr";"REDUCER"], fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let s3 = unbox<string> ss.[3]
        let result:RegularExpression<string>list*string =
            [s0;s2],s3
        box result
    ["rule";"expr";"REDUCER"], fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:RegularExpression<string>list*string =
            [s0],s1
        box result
    ["{atomic+}";"atomic"], fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> list =
            [s0]
        box result
    ["{atomic+}";"{atomic+}";"&";"atomic"], fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string> list> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:RegularExpression<string> list =
            s2::s0
        box result
    ["{definition+}";"definition"], fun(ss:obj list)->
        let s0 = unbox<string*RegularExpression<string>> ss.[0]
        let result:(string*RegularExpression<string>)list =
            [s0]
        box result
    ["{definition+}";"{definition+}";"definition"], fun(ss:obj list)->
        let s0 = unbox<(string*RegularExpression<string>)list> ss.[0]
        let s1 = unbox<string*RegularExpression<string>> ss.[1]
        let result:(string*RegularExpression<string>)list =
            s1::s0
        box result
    ["{rule+}";"rule"], fun(ss:obj list)->
        let s0 = unbox<RegularExpression<string>list*string> ss.[0]
        let result:(RegularExpression<string>list*string)list =
            [s0]
        box result
    ["{rule+}";"{rule+}";"rule"], fun(ss:obj list)->
        let s0 = unbox<(RegularExpression<string>list*string)list> ss.[0]
        let s1 = unbox<RegularExpression<string>list*string> ss.[1]
        let result:(RegularExpression<string>list*string)list =
            s1::s0
        box result
]
let unboxRoot =
    unbox<string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list>
let app: FslexFsyacc.Runtime.ParseTableApp = {
    tokens        = tokens
    kernels       = kernels
    kernelSymbols = kernelSymbols
    actions       = actions
    rules         = rules
}