module FslexFsyacc.Yacc.ClosureOperators
open FslexFsyacc.Runtime

open FSharp.Idioms

/// 一个状态的闭包能goto的所有状态，以kernel表示新状态。
[<System.ObsoleteAttribute("ClosureOperators.getNextKernelsByCrew")>]
let getNextKernels(closure:Set<ItemCore*Set<string>>) =
    closure
    |> Set.filter(fun(itemCore,lookahead) -> not (ItemCoreUtils.dotmax itemCore))
    |> Set.map(fun(itemCore,lookahead) -> (ItemCoreUtils.dotIncr itemCore),lookahead)
    |> Set.groupBy(fun(itemCore,lookahead) -> (ItemCoreUtils.prevSymbol itemCore))

/// 获取一个闭包的kernel
let getKernel(closure:Set<(string list*int)*Set<string>>) =
    closure
    |> Set.filter(fun((prod,dot),_)-> List.head prod = "" || dot > 0)

/// 获取一个闭包的Symbol
let getSymbol (closure:Set<(string list*int)*Set<string>>) =
    match
        closure
        |> Seq.find(fun((prod,dot),_) -> List.head prod = "" || dot > 0)
    with (prod,dot),_ ->
        prod.[dot]

let getCore (items:Set<ItemCore*Set<string>>) = Set.map fst items

let getLookaheads (items:Set<ItemCore*Set<string>>) =
    items
    |> Set.toList
    |> List.map snd



