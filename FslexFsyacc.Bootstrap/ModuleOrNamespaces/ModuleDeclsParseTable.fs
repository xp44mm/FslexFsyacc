module FslexFsyacc.ModuleOrNamespaces.ModuleDeclsParseTable
let tokens = set [",";".";"<";"=";">";"IDENT";"OPEN";"TARG";"TYPAR";"TYPE"]
let kernels = [[0,0];[0,1];[-1,1];[-1,2;-10,1];[-1,3];[-2,1;-3,1];[-2,2];[-2,3];[-3,2;-8,1];[-4,1];[-5,1;-6,1];[-6,2];[-7,1];[-8,2];[-8,3];[-9,1];[-10,2];[-10,3];[-11,1];[-11,2];[-11,3];[-11,4];[-11,5];[-13,1]]
let kernelSymbols = ["";"moduleDecls";"<";"typars";">";"OPEN";"TYPE";"TARG";"path";"typeAbbreviation";"moduleDecl";"moduleDecls";"IDENT";".";"IDENT";"TYPAR";",";"TYPAR";"TYPE";"IDENT";"{genericParameters?}";"=";"TARG";"genericParameters"]
let actions = [["OPEN",5;"TYPE",18;"moduleDecl",10;"moduleDecls",1;"typeAbbreviation",9];["",0];["TYPAR",15;"typars",3];[",",16;">",4];["=",-1];["IDENT",12;"TYPE",6;"path",8];["TARG",7];["",-2;"OPEN",-2;"TYPE",-2];["",-3;".",13;"OPEN",-3;"TYPE",-3];["",-4;"OPEN",-4;"TYPE",-4];["",-5;"OPEN",5;"TYPE",18;"moduleDecl",10;"moduleDecls",11;"typeAbbreviation",9];["",-6];["",-7;".",-7;"OPEN",-7;"TYPE",-7];["IDENT",14];["",-8;".",-8;"OPEN",-8;"TYPE",-8];[",",-9;">",-9];["TYPAR",17];[",",-10;">",-10];["IDENT",19];["<",2;"=",-12;"genericParameters",23;"{genericParameters?}",20];["=",21];["TARG",22];["",-11;"OPEN",-11;"TYPE",-11];["=",-13]]
open FslexFsyacc.TypeArguments
let rules : list<string list*(obj list->obj)> = [
    ["";"moduleDecls"], fun(ss:obj list)-> ss.[0]
    ["genericParameters";"<";"typars";">"], fun(ss:obj list)->
        let s1 = unbox<list<string>> ss.[1]
        let result:list<string> =
            List.rev s1
        box result
    ["moduleDecl";"OPEN";"TYPE";"TARG"], fun(ss:obj list)->
        let s2 = unbox<TypeArgument> ss.[2]
        let result:ModuleDecl =
            OpenType(s2)
        box result
    ["moduleDecl";"OPEN";"path"], fun(ss:obj list)->
        let s1 = unbox<list<string>> ss.[1]
        let result:ModuleDecl =
            Open(List.rev s1)
        box result
    ["moduleDecl";"typeAbbreviation"], fun(ss:obj list)->
        let s0 = unbox<ModuleDecl> ss.[0]
        let result:ModuleDecl =
            s0
        box result
    ["moduleDecls";"moduleDecl"], fun(ss:obj list)->
        let s0 = unbox<ModuleDecl> ss.[0]
        let result:list<ModuleDecl> =
            [s0]
        box result
    ["moduleDecls";"moduleDecl";"moduleDecls"], fun(ss:obj list)->
        let s0 = unbox<ModuleDecl> ss.[0]
        let s1 = unbox<list<ModuleDecl>> ss.[1]
        let result:list<ModuleDecl> =
            s0::s1
        box result
    ["path";"IDENT"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:list<string> =
            [s0]
        box result
    ["path";"path";".";"IDENT"], fun(ss:obj list)->
        let s0 = unbox<list<string>> ss.[0]
        let s2 = unbox<string> ss.[2]
        let result:list<string> =
            s2::s0
        box result
    ["typars";"TYPAR"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:list<string> =
            [s0]
        box result
    ["typars";"typars";",";"TYPAR"], fun(ss:obj list)->
        let s0 = unbox<list<string>> ss.[0]
        let s2 = unbox<string> ss.[2]
        let result:list<string> =
            s2::s0
        box result
    ["typeAbbreviation";"TYPE";"IDENT";"{genericParameters?}";"=";"TARG"], fun(ss:obj list)->
        let s1 = unbox<string> ss.[1]
        let s2 = unbox<list<string>> ss.[2]
        let s4 = unbox<TypeArgument> ss.[4]
        let result:ModuleDecl =
            TypeAbb(s1::s2, s4)
        box result
    ["{genericParameters?}"], fun(ss:obj list)->
        let result:list<string> =
            []
        box result
    ["{genericParameters?}";"genericParameters"], fun(ss:obj list)->
        let s0 = unbox<list<string>> ss.[0]
        let result:list<string> =
            s0
        box result
]
let unboxRoot =
    unbox<list<ModuleDecl>>
let app: FslexFsyacc.ParseTableApp = {
    tokens        = tokens
    kernels       = kernels
    kernelSymbols = kernelSymbols
    actions       = actions
    rules         = rules
}