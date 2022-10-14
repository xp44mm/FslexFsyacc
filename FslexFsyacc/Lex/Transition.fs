/// 转换是从一个状态，经过某路径，到达另一个状态。源状态和目标状态同类型。
module FslexFsyacc.Lex.Transition

open FSharp.Idioms

///自動機的所有狀態
let allStates (transitions:#seq<'state*'a*'state>) =
    transitions
    |> Seq.map(Triple.ends)
    |> Seq.map(fun(s,t)-> [s;t])
    |> List.concat
    |> Set.ofList


