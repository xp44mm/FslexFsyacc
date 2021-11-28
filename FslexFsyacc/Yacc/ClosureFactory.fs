[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.ClosureFactory

open FSharp.Idioms

let make (itemCoreAttributes:Map<ItemCore, bool*Set<string>>)(productions:Set<string list>) =
    let rec loop (acc:Set<ItemCore*Set<string>>)(items: Set<ItemCore*Set<string>>) =
        let newAcc =
            items
            |> Set.filter(fst >> itemCoreAttributes.ContainsKey)
            |> Set.map(fun(itemCore,las) ->
                let lookaheads = 
                    let propagatable,spontaneous = itemCoreAttributes.[itemCore]
                    if propagatable then spontaneous + las else spontaneous
                itemCore.nextSymbol, lookaheads
            )
            |> Set.map(fun(nextSymbol, lookaheads) -> //扩展闭包一次。
                productions
                |> Set.filter(fun p -> p.Head = nextSymbol)
                |> Set.map(fun p -> {production=p; dot=0}, lookaheads)
            )
            |> Set.add acc
            |> Set.unionMany
            |> Set.unionByKey //合并相同项的lookahead集合

        //整个项新增，或者项的lookahead集合的元素有新增，都要重新推导
        let newItems = newAcc - acc
        if newItems.IsEmpty then
            newAcc
        else
            loop newAcc newItems

    fun kernel -> loop kernel kernel

