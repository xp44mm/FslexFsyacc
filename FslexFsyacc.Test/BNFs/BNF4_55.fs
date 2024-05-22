module FslexFsyacc.Runtime.BNFs.BNF4_55
open FslexFsyacc.Runtime

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FSharp.Idioms

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

let production_0 = ["";"S"]
let production_1 = ["C";"c";"C"]
let production_2 = ["C";"d"]
let production_3 = ["S";"C";"C"]

let productions =
    set [
        production_0
        production_1
        production_2
        production_3
    ]

let kernel_0 = [["";"S"],0]      // 0
let kernel_1 = [["";"S"],1]      // 1
let kernel_2 = [["C";"c";"C"],1] // 36
let kernel_3 = [["C";"c";"C"],2] // 89
let kernel_4 = [["C";"d"],1]     // 47
let kernel_5 = [["S";"C";"C"],1] // 2
let kernel_6 = [["S";"C";"C"],2] // 5

let kernel ls =
    ls
    |> Set.ofList
    |> Set.map(ItemCore.just)

let kernels =
    set [
        kernel_0
        kernel_1
        kernel_2
        kernel_3
        kernel_4
        kernel_5
        kernel_6
    ]
    |> Set.map kernel
    
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

let clousre ls =
    ls
    |> Set.ofList
    |> Set.map(fun (p,d,l)->ItemCore.just(p,d),set l)

let closures =
    [
        kernel_0, closure_0
        kernel_1, closure_1
        kernel_2, closure_2
        kernel_3, closure_3
        kernel_4, closure_4
        kernel_5, closure_5
        kernel_6, closure_6
    ]
    |> List.map(fun (k,c) -> kernel k, clousre c)
    |> Map.ofList

let shift k = Shift(kernel k)

let action_0_1 = kernel_0,"C",[shift kernel_5]
let action_0_2 = kernel_0,"S",[shift kernel_1]
let action_0_3 = kernel_0,"c",[shift kernel_2]
let action_0_4 = kernel_0,"d",[shift kernel_4]
let action_1_0 = kernel_1,"",[Reduce production_0]
let action_2_1 = kernel_2,"C",[shift kernel_3]
let action_2_3 = kernel_2,"c",[shift kernel_2]
let action_2_4 = kernel_2,"d",[shift kernel_4]
let action_3_0 = kernel_3,"",[Reduce production_1]
let action_3_3 = kernel_3,"c",[Reduce production_1]
let action_3_4 = kernel_3,"d",[Reduce production_1]
let action_4_0 = kernel_4,"",[Reduce production_2]
let action_4_3 = kernel_4,"c",[Reduce production_2]
let action_4_4 = kernel_4,"d",[Reduce production_2]
let action_5_1 = kernel_5,"C",[shift kernel_6]
let action_5_3 = kernel_5,"c",[shift kernel_2]
let action_5_4 = kernel_5,"d",[shift kernel_4]
let action_6_0 = kernel_6,"",[Reduce production_3]

let actions =
    [
        action_0_1
        action_0_2
        action_0_3
        action_0_4
        action_1_0
        action_2_1
        action_2_3
        action_2_4
        action_3_0
        action_3_3
        action_3_4
        action_4_0
        action_4_3
        action_4_4
        action_5_1
        action_5_3
        action_5_4
        action_6_0
    ]
    |> List.groupBy (fun(a,_,_)->a)
    |> List.map(fun (k,ls) ->
        let k = kernel k
        let mp =
            ls
            |> List.map(fun(_,y,z)-> y,set z )
            |> Map.ofList
        k,mp
    )
    |> Map.ofList


let uaction_0_1 = kernel_0,"C",shift kernel_5
let uaction_0_2 = kernel_0,"S",shift kernel_1
let uaction_0_3 = kernel_0,"c",shift kernel_2
let uaction_0_4 = kernel_0,"d",shift kernel_4
let uaction_1_0 = kernel_1,"",Reduce production_0
let uaction_2_1 = kernel_2,"C",shift kernel_3
let uaction_2_3 = kernel_2,"c",shift kernel_2
let uaction_2_4 = kernel_2,"d",shift kernel_4
let uaction_3_0 = kernel_3,"",Reduce production_1
let uaction_3_3 = kernel_3,"c",Reduce production_1
let uaction_3_4 = kernel_3,"d",Reduce production_1
let uaction_4_0 = kernel_4,"",Reduce production_2
let uaction_4_3 = kernel_4,"c",Reduce production_2
let uaction_4_4 = kernel_4,"d",Reduce production_2
let uaction_5_1 = kernel_5,"C",shift kernel_6
let uaction_5_3 = kernel_5,"c",shift kernel_2
let uaction_5_4 = kernel_5,"d",shift kernel_4
let uaction_6_0 = kernel_6,"",Reduce production_3

let uactions =
    [
    uaction_0_1
    uaction_0_2
    uaction_0_3
    uaction_0_4
    uaction_1_0
    uaction_2_1
    uaction_2_3
    uaction_2_4
    uaction_3_0
    uaction_3_3
    uaction_3_4
    uaction_4_0
    uaction_4_3
    uaction_4_4
    uaction_5_1
    uaction_5_3
    uaction_5_4
    uaction_6_0
    ]
    |> List.groupBy (fun(a,_,_)->a)
    |> List.map(fun (k,ls) ->
        let k = kernel k
        let mp =
            ls
            |> List.map(fun(_,y,z)-> y,z )
            |> Map.ofList
        k,mp
    )
    |> Map.ofList


let encodeActions = [
    ["C",5;"S",1;"c",2;"d",4];
    ["",0];
    ["C",3;"c",2;"d",4];
    ["",-1;"c",-1;"d",-1];
    ["",-2;"c",-2;"d",-2];
    ["C",6;"c",2;"d",4];
    ["",-3]]

let unambiguousItemCores = 
    Map [
        kernel_0,Map ["C",set [{production=["S";"C";"C"];dot=0}];"S",set [{production=["";"S"];dot=0}];"c",set [{production=["C";"c";"C"];dot=0}];"d",set [{production=["C";"d"];dot=0}]];
        kernel_1,Map ["",set [{production=["";"S"];dot=1}]];
        kernel_2,Map ["C",set [{production=["C";"c";"C"];dot=1}];"c",set [{production=["C";"c";"C"];dot=0}];"d",set [{production=["C";"d"];dot=0}]];
        kernel_3,Map ["",set [{production=["C";"c";"C"];dot=2}];"c",set [{production=["C";"c";"C"];dot=2}];"d",set [{production=["C";"c";"C"];dot=2}]];
        kernel_4,Map ["",set [{production=["C";"d"];dot=1}];"c",set [{production=["C";"d"];dot=1}];"d",set [{production=["C";"d"];dot=1}]];
        kernel_5,Map ["C",set [{production=["S";"C";"C"];dot=1}];"c",set [{production=["C";"c";"C"];dot=0}];"d",set [{production=["C";"d"];dot=0}]];
        kernel_6,Map ["",set [{production=["S";"C";"C"];dot=2}]]
    ]
    |> Seq.map(fun(KeyValue(k,mp))-> kernel k,mp )
    |> Map.ofSeq

let resolvedClosures = 
    [
    kernel_0,Map [{production=["";"S"];dot=0},set [];{production=["C";"c";"C"];dot=0},set [];{production=["C";"d"];dot=0},set [];{production=["S";"C";"C"];dot=0},set []]
    kernel_1,Map [{production=["";"S"];dot=1},set [""]]
    kernel_2,Map [{production=["C";"c";"C"];dot=0},set [];{production=["C";"c";"C"];dot=1},set [];{production=["C";"d"];dot=0},set []]
    kernel_3,Map [{production=["C";"c";"C"];dot=2},set ["";"c";"d"]]
    kernel_4,Map [{production=["C";"d"];dot=1},set ["";"c";"d"]]
    kernel_5,Map [{production=["C";"c";"C"];dot=0},set [];{production=["C";"d"];dot=0},set [];{production=["S";"C";"C"];dot=1},set []]
    kernel_6,Map [{production=["S";"C";"C"];dot=2},set [""]]
    ]
    |> Seq.map(fun(k,mp)-> kernel k,mp )
    |> Map.ofSeq

let encodeClosures = 
    [
    [0,0,[];-1,0,[];-2,0,[];-3,0,[]]
    [0,1,[""]]
    [-1,0,[];-1,1,[];-2,0,[]]
    [-1,2,["";"c";"d"]]
    [-2,1,["";"c";"d"]]
    [-1,0,[];-2,0,[];-3,1,[]]
    [-3,2,[""]]
    ]

