[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.GotoFactory

open FSharp.Idioms

/// SLR goto
let make (closures:Set<Set<ItemCore>*Set<ItemCore*_>>) =
    closures
    |> Set.map(fun (kernel,closure) ->
        closure
        |> Set.map fst
        |> Set.filter(fun itemCore -> not itemCore.gone)
        |> Set.map(fun itemCore -> itemCore.dotIncr())
        |> Set.groupBy(fun itemCore -> itemCore.prevSymbol)
        |> Set.map(fun(symbol,tgt) -> kernel,symbol,tgt)
        )
    |> Set.unionMany
