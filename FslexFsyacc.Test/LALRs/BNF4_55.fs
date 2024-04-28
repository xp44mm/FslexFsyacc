module FslexFsyacc.LALRs.BNF4_55

open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores

// grammar 4.55
let S = "S"
let C = "C"
let c = "c"
let d = "d"

let mainProductions = [
    [ S; C; C ]
    [ C; c; C ]
    [ C; d ]
]

let kernel_0 = [["";"S"],0,[""]]              // 0
let kernel_1 = [["";"S"],1,[""]]              // 1
let kernel_2 = [["C";"c";"C"],1,["";"c";"d"]] // 36
let kernel_3 = [["C";"c";"C"],2,["";"c";"d"]] // 89
let kernel_4 = [["C";"d"],1,["";"c";"d"]]     // 47
let kernel_5 = [["S";"C";"C"],1,[""]]         // 2
let kernel_6 = [["S";"C";"C"],2,[""]]         // 5

let kernels =
    [
        kernel_0
        kernel_1
        kernel_2
        kernel_3
        kernel_4
        kernel_5
        kernel_6
    ]
    |> List.map(List.map(fun (a,b,c) -> a,b ))

let closure_0 = [
    ["";"S"],0,[""];
    ["C";"c";"C"],0,["c";"d"];
    ["C";"d"],0,["c";"d"];
    ["S";"C";"C"],0,[""]]
let closure_1 = [["";"S"],1,[""]]
let closure_2 = [
    ["C";"c";"C"],0,["";"c";"d"];
    ["C";"c";"C"],1,["";"c";"d"];
    ["C";"d"],0,["";"c";"d"]]
let closure_3 = [["C";"c";"C"],2,["";"c";"d"]]
let closure_4 = [["C";"d"],1,["";"c";"d"]]
let closure_5 = [
    ["C";"c";"C"],0,[""];
    ["C";"d"],0,[""];
    ["S";"C";"C"],1,[""]]
let closure_6 = [["S";"C";"C"],2,[""]]

let spreadClosure_0 = ["C",["S";"C";"C"],0;"S",["";"S"],0;"c",["C";"c";"C"],0;"d",["C";"d"],0]
let spreadClosure_1 = ["",["";"S"],1]
let spreadClosure_2 = ["C",["C";"c";"C"],1;"c",["C";"c";"C"],0;"d",["C";"d"],0]
let spreadClosure_3 = ["",["C";"c";"C"],2;"c",["C";"c";"C"],2;"d",["C";"c";"C"],2]
let spreadClosure_4 = ["",["C";"d"],1;"c",["C";"d"],1;"d",["C";"d"],1]
let spreadClosure_5 = ["C",["S";"C";"C"],1;"c",["C";"c";"C"],0;"d",["C";"d"],0]
let spreadClosure_6 = ["",["S";"C";"C"],2]

let ikernel i =
    kernels.[i]
    |> Set.ofList
    |> Set.map(fun (p,dot) -> ItemCore.just(p,dot))

let gotos = 
    [
        0,"C",5
        0,"S",1
        0,"c",2
        0,"d",4
        2,"C",3
        2,"c",2
        2,"d",4
        5,"C",6
        5,"c",2
        5,"d",4
    ]
    |> List.map( fun (src,sym,tgt) -> ikernel src, sym, ikernel tgt )

