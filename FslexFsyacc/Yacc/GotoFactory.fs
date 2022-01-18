[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.GotoFactory

//open FSharp.Idioms

///// SLR goto
//let make (closures:Set<Set<ItemCore>*Set<ItemCore*_>>) =
//    closures
//    |> Set.map(fun (kernel,closure) ->
//        let gItems =
//            closure
//            |> Set.map (fun(itemcore,b)->itemcore)
//            |> Set.filter(fun itemCore -> not itemCore.dotmax)
//        gItems
//        |> Set.map(fun itemCore -> itemCore.dotIncr())
//        |> Set.groupBy(fun itemCore -> itemCore.prevSymbol)
//        |> Set.map(fun(symbol,tgt) -> kernel,symbol,tgt) //tgt就是目标状态kernel
//        )
//    |> Set.unionMany
