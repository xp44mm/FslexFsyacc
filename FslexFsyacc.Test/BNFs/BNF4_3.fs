module FslexFsyacc.Runtime.BNFs.BNF4_3
open FslexFsyacc.Runtime.ItemCores

let E = "E"
let T = "T"
let F = "F"
let id = "id"

let mainProductions = [
    [ E; E; "+"; E ]
    [ E; E; "*"; E ]
    [ E; "("; E; ")" ]
    [ E; id ]
    ]

let production_0 = ["";"E"]
let production_1 = ["E";"(";"E";")"]
let production_2 = ["E";"E";"*";"E"]
let production_3 = ["E";"E";"+";"E"]
let production_4 = ["E";"id"]

let productions =
    set [
        production_0
        production_1
        production_2
        production_3
        production_4    
    ]

let kernel_0 = [["";"E"],0]
let kernel_1 = [["";"E"],1;["E";"E";"*";"E"],1;["E";"E";"+";"E"],1]
let kernel_2 = [["E";"(";"E";")"],1]
let kernel_3 = [["E";"(";"E";")"],2;["E";"E";"*";"E"],1;["E";"E";"+";"E"],1]
let kernel_4 = [["E";"(";"E";")"],3]
let kernel_5 = [["E";"E";"*";"E"],1;["E";"E";"*";"E"],3;["E";"E";"+";"E"],1]
let kernel_6 = [["E";"E";"*";"E"],1;["E";"E";"+";"E"],1;["E";"E";"+";"E"],3]
let kernel_7 = [["E";"E";"*";"E"],2]
let kernel_8 = [["E";"E";"+";"E"],2]
let kernel_9 = [["E";"id"],1]

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
        kernel_7
        kernel_8
        kernel_9    
    ]
    |> Set.map kernel
//0
let closure_0 = [
    ["";"E"],0,[""];
    ["E";"(";"E";")"],0,["";"*";"+"];
    ["E";"E";"*";"E"],0,["";"*";"+"];
    ["E";"E";"+";"E"],0,["";"*";"+"];
    ["E";"id"],0,["";"*";"+"]]
//1
let closure_1 = [
    ["";"E"],1,[""];
    ["E";"E";"*";"E"],1,["";"*";"+"];
    ["E";"E";"+";"E"],1,["";"*";"+"]]
//2
let closure_2 = [
    ["E";"(";"E";")"],0,[")";"*";"+"];
    ["E";"(";"E";")"],1,["";")";"*";"+"];
    ["E";"E";"*";"E"],0,[")";"*";"+"];
    ["E";"E";"+";"E"],0,[")";"*";"+"];
    ["E";"id"],0,[")";"*";"+"]]
//6
let closure_3 = [
    ["E";"(";"E";")"],2,["";")";"*";"+"];
    ["E";"E";"*";"E"],1,[")";"*";"+"];
    ["E";"E";"+";"E"],1,[")";"*";"+"]]

//9
let closure_4 = [["E";"(";"E";")"],3,["";")";"*";"+"]]

//8
let closure_5 = [
    ["E";"E";"*";"E"],1,["";")";"*";"+"];
    ["E";"E";"*";"E"],3,["";")";"*";"+"];
    ["E";"E";"+";"E"],1,["";")";"*";"+"]]
//7
let closure_6 = [
    ["E";"E";"*";"E"],1,["";")";"*";"+"];
    ["E";"E";"+";"E"],1,["";")";"*";"+"];
    ["E";"E";"+";"E"],3,["";")";"*";"+"]]
//5
let closure_7 = [
    ["E";"(";"E";")"],0,["";")";"*";"+"];
    ["E";"E";"*";"E"],0,["";")";"*";"+"];
    ["E";"E";"*";"E"],2,["";")";"*";"+"];
    ["E";"E";"+";"E"],0,["";")";"*";"+"];
    ["E";"id"],0,["";")";"*";"+"]]
//4
let closure_8 = [
    ["E";"(";"E";")"],0,["";")";"*";"+"];
    ["E";"E";"*";"E"],0,["";")";"*";"+"];
    ["E";"E";"+";"E"],0,["";")";"*";"+"];
    ["E";"E";"+";"E"],2,["";")";"*";"+"];
    ["E";"id"],0,["";")";"*";"+"]]
//3
let closure_9 = [["E";"id"],1,["";")";"*";"+"]]

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
        kernel_7, closure_7
        kernel_8, closure_8
        kernel_9, closure_9    
    ]
    |> List.map(fun (k,c) -> kernel k, clousre c)
    |> Map.ofList

let shift k = Shift(kernel k)

let action_0_1 = kernel_0,"(",[shift kernel_2]
let action_0_5 = kernel_0,"E",[shift kernel_1]
let action_0_6 = kernel_0,"id",[shift kernel_9]
let action_1_0 = kernel_1,"",[Reduce production_0]
let action_1_3 = kernel_1,"*",[shift kernel_7]
let action_1_4 = kernel_1,"+",[shift kernel_8]
let action_2_1 = kernel_2,"(",[shift kernel_2]
let action_2_5 = kernel_2,"E",[shift kernel_3]
let action_2_6 = kernel_2,"id",[shift kernel_9]
let action_3_2 = kernel_3,")",[shift kernel_4]
let action_3_3 = kernel_3,"*",[shift kernel_7]
let action_3_4 = kernel_3,"+",[shift kernel_8]
let action_4_0 = kernel_4,"",[Reduce production_1]
let action_4_2 = kernel_4,")",[Reduce production_1]
let action_4_3 = kernel_4,"*",[Reduce production_1]
let action_4_4 = kernel_4,"+",[Reduce production_1]
let action_5_0 = kernel_5,"",[Reduce production_2]
let action_5_2 = kernel_5,")",[Reduce production_2]
let action_5_3 = kernel_5,"*",[shift kernel_7;Reduce production_2]
let action_5_4 = kernel_5,"+",[shift kernel_8;Reduce production_2]
let action_6_0 = kernel_6,"",[Reduce production_3]
let action_6_2 = kernel_6,")",[Reduce production_3]
let action_6_3 = kernel_6,"*",[shift kernel_7;Reduce production_3]
let action_6_4 = kernel_6,"+",[shift kernel_8;Reduce production_3]
let action_7_1 = kernel_7,"(",[shift kernel_2]
let action_7_5 = kernel_7,"E",[shift kernel_5]
let action_7_6 = kernel_7,"id",[shift kernel_9]
let action_8_1 = kernel_8,"(",[shift kernel_2]
let action_8_5 = kernel_8,"E",[shift kernel_6]
let action_8_6 = kernel_8,"id",[shift kernel_9]
let action_9_0 = kernel_9,"",[Reduce production_4]
let action_9_2 = kernel_9,")",[Reduce production_4]
let action_9_3 = kernel_9,"*",[Reduce production_4]
let action_9_4 = kernel_9,"+",[Reduce production_4]

let actions =
    [
        action_0_1
        action_0_5
        action_0_6
        action_1_0
        action_1_3
        action_1_4
        action_2_1
        action_2_5
        action_2_6
        action_3_2
        action_3_3
        action_3_4
        action_4_0
        action_4_2
        action_4_3
        action_4_4
        action_5_0
        action_5_2
        action_5_3
        action_5_4
        action_6_0
        action_6_2
        action_6_3
        action_6_4
        action_7_1
        action_7_5
        action_7_6
        action_8_1
        action_8_5
        action_8_6
        action_9_0
        action_9_2
        action_9_3
        action_9_4    
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

let precedences =
    Map [
        "+",100-1
        "*",200-1
    ]

let uaction_0_1 = kernel_0,"(",shift kernel_2
let uaction_0_5 = kernel_0,"E",shift kernel_1
let uaction_0_6 = kernel_0,"id",shift kernel_9
let uaction_1_0 = kernel_1,"",Reduce production_0
let uaction_1_3 = kernel_1,"*",shift kernel_7
let uaction_1_4 = kernel_1,"+",shift kernel_8
let uaction_2_1 = kernel_2,"(",shift kernel_2
let uaction_2_5 = kernel_2,"E",shift kernel_3
let uaction_2_6 = kernel_2,"id",shift kernel_9
let uaction_3_2 = kernel_3,")",shift kernel_4
let uaction_3_3 = kernel_3,"*",shift kernel_7
let uaction_3_4 = kernel_3,"+",shift kernel_8
let uaction_4_0 = kernel_4,"",Reduce production_1
let uaction_4_2 = kernel_4,")",Reduce production_1
let uaction_4_3 = kernel_4,"*",Reduce production_1
let uaction_4_4 = kernel_4,"+",Reduce production_1
let uaction_5_0 = kernel_5,"",Reduce production_2
let uaction_5_2 = kernel_5,")",Reduce production_2
let uaction_5_3 = kernel_5,"*",Reduce production_2
let uaction_5_4 = kernel_5,"+",Reduce production_2
let uaction_6_0 = kernel_6,"",Reduce production_3
let uaction_6_2 = kernel_6,")",Reduce production_3
let uaction_6_3 = kernel_6,"*",shift kernel_7
let uaction_6_4 = kernel_6,"+",Reduce production_3
let uaction_7_1 = kernel_7,"(",shift kernel_2
let uaction_7_5 = kernel_7,"E",shift kernel_5
let uaction_7_6 = kernel_7,"id",shift kernel_9
let uaction_8_1 = kernel_8,"(",shift kernel_2
let uaction_8_5 = kernel_8,"E",shift kernel_6
let uaction_8_6 = kernel_8,"id",shift kernel_9
let uaction_9_0 = kernel_9,"",Reduce production_4
let uaction_9_2 = kernel_9,")",Reduce production_4
let uaction_9_3 = kernel_9,"*",Reduce production_4
let uaction_9_4 = kernel_9,"+",Reduce production_4


let uactions =
    [
    uaction_0_1
    uaction_0_5
    uaction_0_6
    uaction_1_0
    uaction_1_3
    uaction_1_4
    uaction_2_1
    uaction_2_5
    uaction_2_6
    uaction_3_2
    uaction_3_3
    uaction_3_4
    uaction_4_0
    uaction_4_2
    uaction_4_3
    uaction_4_4
    uaction_5_0
    uaction_5_2
    uaction_5_3
    uaction_5_4
    uaction_6_0
    uaction_6_2
    uaction_6_3
    uaction_6_4
    uaction_7_1
    uaction_7_5
    uaction_7_6
    uaction_8_1
    uaction_8_5
    uaction_8_6
    uaction_9_0
    uaction_9_2
    uaction_9_3
    uaction_9_4
    ]
    |> List.groupBy (fun(a,_,_)-> a )
    |> List.map(fun (k,ls) ->
        let k = kernel k
        let mp =
            ls
            |> List.map(fun(_,y,z)-> y,z )
            |> Map.ofList
        k,mp
    )
    |> Map.ofList

let encodeActions = [["(",2;"E",1;"id",9];["",0;"*",7;"+",8];["(",2;"E",3;"id",9];[")",4;"*",7;"+",8];["",-1;")",-1;"*",-1;"+",-1];["",-2;")",-2;"*",-2;"+",-2];["",-3;")",-3;"*",7;"+",-3];["(",2;"E",5;"id",9];["(",2;"E",6;"id",9];["",-4;")",-4;"*",-4;"+",-4]]

let unambiguousItemCores = 
    [
    kernel_0,Map ["(",set [{production=["E";"(";"E";")"];dot=0}];"E",set [{production=["";"E"];dot=0};{production=["E";"E";"*";"E"];dot=0};{production=["E";"E";"+";"E"];dot=0}];"id",set [{production=["E";"id"];dot=0}]]
    kernel_1,Map ["",set [{production=["";"E"];dot=1}];"*",set [{production=["E";"E";"*";"E"];dot=1}];"+",set [{production=["E";"E";"+";"E"];dot=1}]]
    kernel_2,Map ["(",set [{production=["E";"(";"E";")"];dot=0}];"E",set [{production=["E";"(";"E";")"];dot=1};{production=["E";"E";"*";"E"];dot=0};{production=["E";"E";"+";"E"];dot=0}];"id",set [{production=["E";"id"];dot=0}]]
    kernel_3,Map [")",set [{production=["E";"(";"E";")"];dot=2}];"*",set [{production=["E";"E";"*";"E"];dot=1}];"+",set [{production=["E";"E";"+";"E"];dot=1}]]
    kernel_4,Map ["",set [{production=["E";"(";"E";")"];dot=3}];")",set [{production=["E";"(";"E";")"];dot=3}];"*",set [{production=["E";"(";"E";")"];dot=3}];"+",set [{production=["E";"(";"E";")"];dot=3}]]
    kernel_5,Map ["",set [{production=["E";"E";"*";"E"];dot=3}];")",set [{production=["E";"E";"*";"E"];dot=3}];"*",set [{production=["E";"E";"*";"E"];dot=3}];"+",set [{production=["E";"E";"*";"E"];dot=3}]]
    kernel_6,Map ["",set [{production=["E";"E";"+";"E"];dot=3}];")",set [{production=["E";"E";"+";"E"];dot=3}];"*",set [{production=["E";"E";"*";"E"];dot=1}];"+",set [{production=["E";"E";"+";"E"];dot=3}]]
    kernel_7,Map ["(",set [{production=["E";"(";"E";")"];dot=0}];"E",set [{production=["E";"E";"*";"E"];dot=0};{production=["E";"E";"*";"E"];dot=2};{production=["E";"E";"+";"E"];dot=0}];"id",set [{production=["E";"id"];dot=0}]]
    kernel_8,Map ["(",set [{production=["E";"(";"E";")"];dot=0}];"E",set [{production=["E";"E";"*";"E"];dot=0};{production=["E";"E";"+";"E"];dot=0};{production=["E";"E";"+";"E"];dot=2}];"id",set [{production=["E";"id"];dot=0}]]
    kernel_9,Map ["",set [{production=["E";"id"];dot=1}];")",set [{production=["E";"id"];dot=1}];"*",set [{production=["E";"id"];dot=1}];"+",set [{production=["E";"id"];dot=1}]]
    ]
    |> Seq.map(fun(k,mp)->
        kernel k,mp
    )
    |> Map.ofSeq


let resolvedClosures = 
    [
    kernel_0,Map [{production=["";"E"];dot=0},set [];{production=["E";"(";"E";")"];dot=0},set [];{production=["E";"E";"*";"E"];dot=0},set [];{production=["E";"E";"+";"E"];dot=0},set [];{production=["E";"id"];dot=0},set []]
    kernel_1,Map [{production=["";"E"];dot=1},set [""];{production=["E";"E";"*";"E"];dot=1},set [];{production=["E";"E";"+";"E"];dot=1},set []]
    kernel_2,Map [{production=["E";"(";"E";")"];dot=0},set [];{production=["E";"(";"E";")"];dot=1},set [];{production=["E";"E";"*";"E"];dot=0},set [];{production=["E";"E";"+";"E"];dot=0},set [];{production=["E";"id"];dot=0},set []]
    kernel_3,Map [{production=["E";"(";"E";")"];dot=2},set [];{production=["E";"E";"*";"E"];dot=1},set [];{production=["E";"E";"+";"E"];dot=1},set []]
    kernel_4,Map [{production=["E";"(";"E";")"];dot=3},set ["";")";"*";"+"]]
    kernel_5,Map [{production=["E";"E";"*";"E"];dot=3},set ["";")";"*";"+"]]
    kernel_6,Map [{production=["E";"E";"*";"E"];dot=1},set [];{production=["E";"E";"+";"E"];dot=3},set ["";")";"+"]]
    kernel_7,Map [{production=["E";"(";"E";")"];dot=0},set [];{production=["E";"E";"*";"E"];dot=0},set [];{production=["E";"E";"*";"E"];dot=2},set [];{production=["E";"E";"+";"E"];dot=0},set [];{production=["E";"id"];dot=0},set []]
    kernel_8,Map [{production=["E";"(";"E";")"];dot=0},set [];{production=["E";"E";"*";"E"];dot=0},set [];{production=["E";"E";"+";"E"];dot=0},set [];{production=["E";"E";"+";"E"];dot=2},set [];{production=["E";"id"];dot=0},set []]
    kernel_9,Map [{production=["E";"id"];dot=1},set ["";")";"*";"+"]]
    ]
    |> Seq.map(fun(k,mp)-> kernel k,mp )
    |> Map.ofSeq

let encodeClosures = 
    [
    [0,0,[];-1,0,[];-2,0,[];-3,0,[];-4,0,[]]
    [0,1,[""];-2,1,[];-3,1,[]]
    [-1,0,[];-1,1,[];-2,0,[];-3,0,[];-4,0,[]]
    [-1,2,[];-2,1,[];-3,1,[]]
    [-1,3,["";")";"*";"+"]]
    [-2,3,["";")";"*";"+"]]
    [-2,1,[];-3,3,["";")";"+"]]
    [-1,0,[];-2,0,[];-2,2,[];-3,0,[];-4,0,[]]
    [-1,0,[];-2,0,[];-3,0,[];-3,2,[];-4,0,[]]
    [-4,1,["";")";"*";"+"]]
    ]
