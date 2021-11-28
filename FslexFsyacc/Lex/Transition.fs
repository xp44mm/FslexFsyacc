/// 转换是从一个状态，经过某路径，到达另一个状态。源状态和目标状态同类型。
module FslexFsyacc.Lex.Transition

open FSharp.Idioms

/////自動機的所有源狀態
//let sourceStates (transitions:#seq<'state*'a*'state>) =
//    transitions
//    |> Seq.map(Triple.first)
//    |> Set.ofSeq

/////自動機的所有目标狀態
//let targetStates (transitions:#seq<'state*'a*'state>) =
//    transitions
//    |> Seq.map(Triple.last)
//    |> Set.ofSeq

///自動機的所有狀態
let allStates (transitions:#seq<'state*'a*'state>) =
    transitions
    |> Seq.map(Triple.ends)
    |> Seq.map(fun(s,t)-> [s;t])
    |> List.concat
    |> Set.ofList

    //sourceStates transitions + targetStates transitions

/////構造用的NFA只有一個開始狀態，是最小整數編號的那個狀態。只發出箭頭，不接收箭頭。
//let minState (transitions:#seq<'state*'a*'state>) =
//    transitions |> sourceStates |> Seq.min

/////構造用的NFA只有一個結束狀態，是最大整數編號的那個狀態。只接收箭頭，不發出箭頭。
//let maxState (transitions:#seq<'state*'a*'state>) =
//    transitions |> targetStates |> Seq.max

///// 转换表(transitions)所有狀態編號都平移(shift)一個整數偏移量(offset)
//let shift (offset:uint32) (transitions:#seq<uint32*'a*uint32>) =
//    transitions
//    |> Seq.map(fun(s,a,t)-> s+offset ,a, t+offset)
//    |> Set.ofSeq


