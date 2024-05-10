module FslexFsyacc.Runtime.BNFs.BNF4_67
open FslexFsyacc.Runtime.ItemCores

let stmt = "stmt"
let expr = "expr"
let other = "other"

let mainProductions = [
    [ stmt; "if"; expr; "then"; stmt; "else"; stmt ]
    [ stmt; "if"; expr; "then"; stmt ]
    [ stmt; other ]
    ]

let production_0 = ["";"stmt"]
let production_1 = ["stmt";"if";"expr";"then";"stmt"]
let production_2 = ["stmt";"if";"expr";"then";"stmt";"else";"stmt"]
let production_3 = ["stmt";"other"]

let productions =
    set [
        production_0
        production_1
        production_2
        production_3
    ]

let kernel_0 = [["";"stmt"],0]
let kernel_1 = [["";"stmt"],1]
let kernel_2 = [["stmt";"if";"expr";"then";"stmt"],1;["stmt";"if";"expr";"then";"stmt";"else";"stmt"],1]
let kernel_3 = [["stmt";"if";"expr";"then";"stmt"],2;["stmt";"if";"expr";"then";"stmt";"else";"stmt"],2]
let kernel_4 = [["stmt";"if";"expr";"then";"stmt"],3;["stmt";"if";"expr";"then";"stmt";"else";"stmt"],3]
let kernel_5 = [["stmt";"if";"expr";"then";"stmt"],4;["stmt";"if";"expr";"then";"stmt";"else";"stmt"],4]
let kernel_6 = [["stmt";"if";"expr";"then";"stmt";"else";"stmt"],5]
let kernel_7 = [["stmt";"if";"expr";"then";"stmt";"else";"stmt"],6]
let kernel_8 = [["stmt";"other"],1]

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
    ]
    |> Set.map kernel

let closure_0 = [["";"stmt"],0,[""];["stmt";"if";"expr";"then";"stmt"],0,[""];["stmt";"if";"expr";"then";"stmt";"else";"stmt"],0,[""];["stmt";"other"],0,[""]]
let closure_1 = [["";"stmt"],1,[""]]
let closure_2 = [["stmt";"if";"expr";"then";"stmt"],1,["";"else"];["stmt";"if";"expr";"then";"stmt";"else";"stmt"],1,["";"else"]]
let closure_3 = [["stmt";"if";"expr";"then";"stmt"],2,["";"else"];["stmt";"if";"expr";"then";"stmt";"else";"stmt"],2,["";"else"]]
let closure_4 = [["stmt";"if";"expr";"then";"stmt"],0,["";"else"];["stmt";"if";"expr";"then";"stmt"],3,["";"else"];["stmt";"if";"expr";"then";"stmt";"else";"stmt"],0,["";"else"];["stmt";"if";"expr";"then";"stmt";"else";"stmt"],3,["";"else"];["stmt";"other"],0,["";"else"]]
let closure_5 = [["stmt";"if";"expr";"then";"stmt"],4,["";"else"];["stmt";"if";"expr";"then";"stmt";"else";"stmt"],4,["";"else"]]
let closure_6 = [["stmt";"if";"expr";"then";"stmt"],0,["";"else"];["stmt";"if";"expr";"then";"stmt";"else";"stmt"],0,["";"else"];["stmt";"if";"expr";"then";"stmt";"else";"stmt"],5,["";"else"];["stmt";"other"],0,["";"else"]]
let closure_7 = [["stmt";"if";"expr";"then";"stmt";"else";"stmt"],6,["";"else"]]
let closure_8 = [["stmt";"other"],1,["";"else"]]

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
    ]
    |> List.map(fun (k,c) -> kernel k, clousre c)
    |> Map.ofList

let shift k = Shift(kernel k)

let action_0_3 = kernel_0,"if",[shift kernel_2]
let action_0_4 = kernel_0,"other",[shift kernel_8]
let action_0_5 = kernel_0,"stmt",[shift kernel_1]
let action_1_0 = kernel_1,"",[Reduce production_0]
let action_2_2 = kernel_2,"expr",[shift kernel_3]
let action_3_6 = kernel_3,"then",[shift kernel_4]
let action_4_3 = kernel_4,"if",[shift kernel_2]
let action_4_4 = kernel_4,"other",[shift kernel_8]
let action_4_5 = kernel_4,"stmt",[shift kernel_5]
let action_5_0 = kernel_5,"",[Reduce production_1]
let action_5_1 = kernel_5,"else",[shift kernel_6;Reduce production_1]
let action_6_3 = kernel_6,"if",[shift kernel_2]
let action_6_4 = kernel_6,"other",[shift kernel_8]
let action_6_5 = kernel_6,"stmt",[shift kernel_7]
let action_7_0 = kernel_7,"",[Reduce production_2]
let action_7_1 = kernel_7,"else",[Reduce production_2]
let action_8_0 = kernel_8,"",[Reduce production_3]
let action_8_1 = kernel_8,"else",[Reduce production_3]

let actions =
    [
        action_0_3
        action_0_4
        action_0_5
        action_1_0
        action_2_2
        action_3_6
        action_4_3
        action_4_4
        action_4_5
        action_5_0
        action_5_1
        action_6_3
        action_6_4
        action_6_5
        action_7_0
        action_7_1
        action_8_0
        action_8_1
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

let uaction_0_3 = kernel_0,"if",shift kernel_2
let uaction_0_4 = kernel_0,"other",shift kernel_8
let uaction_0_5 = kernel_0,"stmt",shift kernel_1
let uaction_1_0 = kernel_1,"",Reduce production_0
let uaction_2_2 = kernel_2,"expr",shift kernel_3
let uaction_3_6 = kernel_3,"then",shift kernel_4
let uaction_4_3 = kernel_4,"if",shift kernel_2
let uaction_4_4 = kernel_4,"other",shift kernel_8
let uaction_4_5 = kernel_4,"stmt",shift kernel_5
let uaction_5_0 = kernel_5,"",Reduce production_1
let uaction_5_1 = kernel_5,"else",shift kernel_6
let uaction_6_3 = kernel_6,"if",shift kernel_2
let uaction_6_4 = kernel_6,"other",shift kernel_8
let uaction_6_5 = kernel_6,"stmt",shift kernel_7
let uaction_7_0 = kernel_7,"",Reduce production_2
let uaction_7_1 = kernel_7,"else",Reduce production_2
let uaction_8_0 = kernel_8,"",Reduce production_3
let uaction_8_1 = kernel_8,"else",Reduce production_3


let uactions =
    [
        uaction_0_3
        uaction_0_4
        uaction_0_5
        uaction_1_0
        uaction_2_2
        uaction_3_6
        uaction_4_3
        uaction_4_4
        uaction_4_5
        uaction_5_0
        uaction_5_1
        uaction_6_3
        uaction_6_4
        uaction_6_5
        uaction_7_0
        uaction_7_1
        uaction_8_0
        uaction_8_1
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


let encodeActions = [["if",2;"other",8;"stmt",1];["",0];["expr",3];["then",4];["if",2;"other",8;"stmt",5];["",-1;"else",6];["if",2;"other",8;"stmt",7];["",-2;"else",-2];["",-3;"else",-3]]
