module FslexFsyacc.Runtime.Lex.NFAOperationsUtils

open FSharp.Idioms

/// 从NFA的转换表获得操作的查询表
let create (transition:Set<'state*'a option*'state>) =
    /// 状态和他的等价状态集
    let allStates =
        transition
        |> Transition.allStates
        |> Set.map(fun s -> s, Set.singleton s)
        |> Map.ofSeq

    let noneTable =
        transition
        |> Set.filter(fun(_,op,_) -> Option.isNone op)

    //每個pair(1,2)代表如下：
    //(1)--None-->(2)
    let statePairs =
        noneTable 
        |> Set.map(fun(s,_,t)->s,t)

    //單個NFA狀態的空閉包
    let closures = Graph.propagate allStates statePairs

    //起點相同，標簽非空的箭頭分到一組。
    let moves =
        let someTable = transition - noneTable
        someTable
        |> Set.map(fun(s,a,t)-> s,a.Value,t)
        |> Set.toJaggedMap

    {
        moves = moves;
        closures = closures;
    }

