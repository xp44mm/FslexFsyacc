namespace FslexFsyacc.Yacc

/// SLR && LALR
type Action = 
    | Shift of kernel: Set<ItemCore>
    | Reduce of production: string list

    static member from(closure:Map<string,ItemCore>) =
        let rItems,gItems =
            closure
            |> Map.toList
            |> List.partition(fun(la,item)-> item.dotmax)
    
        let reduces =
            rItems
            |> List.map(fun(la,i) ->
                la, Reduce i.production
            )
    
        let shifts =
            gItems
            |> List.groupBy(fun (la,_) -> la)
            |> List.map(fun(la,pairs)->
                let items = 
                    pairs 
                    |> List.map(fun(la,itemcore) ->
                        itemcore.dotIncr()
                    )
                    |> Set.ofList
                la, Shift items
            )

        seq { yield! reduces; yield! shifts}
        |> Map.ofSeq
