namespace FslexFsyacc.Yacc

/// SLR && LALR
type Action = 
    | Shift of kernel: Set<ItemCore>
    | Reduce of production: string list
    | DeadState // 多余的，删除

    static member from(closure:Map<string,ItemCore>) =
        let rItems,gItems =
            closure
            |> Map.toArray
            |> Array.partition(fun(la,item)-> item.dotmax)
    
        let reduces =
            rItems
            |> Array.map(fun(la,i) ->
                la, Reduce i.production
            )
    
        let shifts =
            gItems
            |> Array.groupBy(fun (la,_) -> la)
            |> Array.map(fun(la,pairs)->
                let items = 
                    pairs 
                    |> Array.map(fun(la,itemcore) ->
                        itemcore.dotIncr()
                    )
                    |> Set.ofArray
                la, Shift items
            )

        seq { yield! reduces; yield! shifts}
        |> Map.ofSeq