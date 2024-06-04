namespace FslexFsyacc.Runtime.YACCs

open FslexFsyacc.Runtime.Precedences
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

open FSharp.Idioms
open FSharp.Idioms.Literal

type ParseTableDecoder =
    {
        iproductions: Map<int, string list>
        ikernels: Map<int,Set<ItemCore>>
    }

    /// 产生式编码为负
    static member from<'reducer> (
        rules:list<string list*'reducer>, 
        kernels: list<list<int*int>>
    ) =
        let iproductions =
            rules
            |> Seq.mapi(fun i (p,_) -> -i, p)
            |> Map.ofSeq

        let ikernels =
            kernels 
            |> List.map(fun kernel -> 
                kernel
                |> List.map(fun (prod,dot) ->
                    ItemCore.just(iproductions.[prod],dot)
                )
                |> Set.ofList
            )
            |> List.indexed
            |> Map.ofSeq
        { iproductions = iproductions; ikernels = ikernels }

    /// 具体数据编码成整数的表
    member this.decodeAction (iaction:int) =
        if iaction > 0 then
            let kernel = this.ikernels.[iaction]
            Shift kernel
        else
            Reduce this.iproductions.[iaction]

    member this.decodeActions (iactions: Map<int,Map<string,int>>) =
        iactions
        |> Seq.map(fun(KeyValue(ik,mp)) ->
            mp
            |> Map.map(fun symbol iact ->
                this.decodeAction iact
            )
            |> Pair.prepend this.ikernels.[ik]
        )
        |> Map.ofSeq
