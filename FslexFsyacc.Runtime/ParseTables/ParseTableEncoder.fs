namespace FslexFsyacc.Runtime.ParseTables

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

open FSharp.Idioms
open FSharp.Idioms.Literal

type ParseTableEncoder =
    {
        productions: Map<string list,int>
        kernels    : Map<Set<ItemCore>,int>
    }

    /// 产生式编码为负
    static member getProductions (productions:Set<string list>) =
        productions
        |> Set.toList
        |> List.mapi(fun i p -> p,-i)
        |> Map.ofList

    /// 具体数据编码成整数的表
    member this.encodeAction (action:Action) =
        match action with
        | Shift j -> this.kernels.[j]
        | Reduce p -> this.productions.[p]

    member encoder.encodeActions (actions: Map< Set<ItemCore>, Map<string,Action>> ) =
        encoder.kernels
        |> Map.toList
        |> List.map(fun (kernel,state) ->
            if actions.ContainsKey kernel then
                actions.[kernel]
                |> Map.toList
                |> List.map(fun(sym,action)->
                    let iaction = encoder.encodeAction action
                    sym,iaction
                )
            else []
        )
    member this.encodeProduction (production:string list) =
            this.productions.[production]

    member this.encodeItemCore (itemCore: ItemCore) =
        let iprod = 
            this.encodeProduction itemCore.production
        iprod,itemCore.dot

    member encoder.encodeClosures(closures: Map<Set<ItemCore>,Map<ItemCore,Set<string>>>) =
        closures
        |> Seq.mapi(fun i (KeyValue(kernel,itemCores)) -> 
            if i<>encoder.kernels.[kernel] then failwith $"state编码不同于自然数序列"
            itemCores
            |> Map.toList
            |> List.map(fun(icore,las)->
                let prod,dot = encoder.encodeItemCore icore
                prod,dot,Set.toList las
            )
        )
        |> Seq.toList

