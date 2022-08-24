namespace FslexFsyacc.Yacc

open FSharp.Idioms

type ParseTableEncoder =
    {
        productions: Map<string list,int>
        kernels    : Map<Set<ItemCore>,int>
    }
    /// 产生式编码为负
    static member getProductions(productions:Set<string list>)=
        productions
        |> Set.toArray
        |> Array.mapi(fun i p -> p,-i)
        |> Map.ofArray

    /// 具体数据编码成整数的表
    member this.encodeAction(action:Action) =
        match action with
        | Shift j -> this.kernels.[j]
        | Reduce p -> this.productions.[p]

    member this.encodeProduction(production:string list) =
            this.productions.[production]

    member this.encodeItemCore(itemCore: ItemCore) =
        let iprod = 
            this.productions.[itemCore.production]
        iprod,itemCore.dot

    member encoder.encodeActions(actions: Map<int,Map<string,Action>>) =
        encoder.kernels
        |> Map.toList
        |> List.map(fun(_,i)->
            if actions.ContainsKey i then
                actions.[i]
                |> Map.toList
                |> List.map(fun(la,action)->
                    try
                        let iaction = encoder.encodeAction action
                        la,iaction
                    with _ -> failwithf "%A" (la,action)
                )
            else []
        )

    member encoder.encodeClosures(closures: Map<int,Map<ItemCore,Set<string>>>) =
        encoder.kernels
        |> Map.toList
        |> List.map(fun (_,i) ->
            if closures.ContainsKey i then
                closures.[i]
                |> Map.toList
                |> List.map(fun(icore,las)->
                    let prod,dot = encoder.encodeItemCore icore
                    prod,dot,Set.toList las
                )
            else []
        )
