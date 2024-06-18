module FslexFsyacc.ModuleOrNamespaces.ModuleDeclsParseTable
let tokens = set [".";"IDENT";"OPEN";"TARG";"TYPE"]
let kernels = [[0,0];[0,1];[-1,1];[-2,1;-3,1];[-3,2];[-4,1;-5,1];[-4,2];[-4,3];[-5,2;-7,1];[-6,1];[-7,2];[-7,3]]
let kernelSymbols = ["";"moduleDecls";"openDecl";"moduleDecl";"moduleDecls";"OPEN";"TYPE";"TARG";"path";"IDENT";".";"IDENT"]
let actions = [["OPEN",5;"moduleDecl",3;"moduleDecls",1;"openDecl",2];["",0];["",-1;"OPEN",-1];["",-2;"OPEN",5;"moduleDecl",3;"moduleDecls",4;"openDecl",2];["",-3];["IDENT",9;"TYPE",6;"path",8];["TARG",7];["",-4;"OPEN",-4];["",-5;".",10;"OPEN",-5];["",-6;".",-6;"OPEN",-6];["IDENT",11];["",-7;".",-7;"OPEN",-7]]
open FslexFsyacc.TypeArguments
let rules : list<string list*(obj list->obj)> = [
    ["";"moduleDecls"], fun(ss:obj list)-> ss.[0]
    ["moduleDecl";"openDecl"], fun(ss:obj list)->
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
    ["openDecl";"OPEN";"TYPE";"TARG"], fun(ss:obj list)->
        let s2 = unbox<TypeArgument> ss.[2]
        let result:ModuleDecl =
            OpenType(s2)
        box result
    ["openDecl";"OPEN";"path"], fun(ss:obj list)->
        let s1 = unbox<list<string>> ss.[1]
        let result:ModuleDecl =
            Open(List.rev s1)
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