[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.AmbiguousTableFactory

open FSharp.Idioms

let make (closures:Set<Set<ItemCore>*Set<ItemCore*Set<string>>>) (gotos:Set<Set<ItemCore>*_*Set<ItemCore>>) =
    let shifts =
        gotos
        |> Set.map(fun(src,symbol,tgt)-> 
            src,symbol,Shift tgt)

    let reduces =
        closures
        |> Set.map(fun(kernel,closure) ->
            closure
            |> Set.filter(fun(i,_) -> i.gone)
            |> Set.map(fun(i,lookahead) ->
                lookahead
                |> Set.map(fun terminal -> kernel, terminal, Reduce i.production)
            )
            |> Set.unionMany
        )
        |> Set.unionMany

    let result:Set<Set<ItemCore>*string*Set<Action>> =
        Set.union shifts reduces
        |> Set.groupBy(Triple.firstTwo)
        |> Set.map(fun((src,symbol),st) -> 
            let targets = Set.map Triple.last st
            src, symbol, targets
        )
    result