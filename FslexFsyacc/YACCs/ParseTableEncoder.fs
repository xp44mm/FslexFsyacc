namespace FslexFsyacc.Runtime.YACCs

open FslexFsyacc.Runtime.Precedences
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

open FSharp.Idioms
open FSharp.Idioms.Literal

type ParseTableEncoder =
    {
        productions: Map<string list,int>
        kernels: Map<Set<ItemCore>,int>
    }

    /// 产生式编码为负
    static member from (productions:Set<string list>, kernels: Set<Set<ItemCore>>) =
        let p =
            productions
            |> Seq.mapi(fun i p -> p,-i)
            |> Map.ofSeq
        let k =
            kernels 
            |> Seq.mapi(fun i k -> k,i) 
            |> Map.ofSeq
        { productions = p; kernels = k }

    /// 具体数据编码成整数的表
    member this.encodeAction (action:ParseTableAction) =
        match action with
        | Shift ik -> this.kernels.[ik]
        | Reduce ip -> this.productions.[ip]

    member encoder.encodeActions (actions: Map<Set<ItemCore>, Map<string,ParseTableAction>>) =
        // 一些kernel没有出状态的箭头，必须用完全的kernels补足所有状态
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

    member this.encodeItemCore(itemCore: ItemCore) =
        this.productions.[itemCore.production],itemCore.dot

    member this.encodeKernel(kernel: Set<ItemCore>) =
        kernel
        |> Seq.map(this.encodeItemCore)
        |> Seq.toList

    member this.encodeKernels(kernels: Set<Set<ItemCore>>) =
        kernels
        |> List.ofSeq
        |> List.map this.encodeKernel

    //member _.encodeKernelSymbols(kernelSymbols: Map<Set<ItemCore>,string>) =
    //    kernelSymbols
    //    |> Map.values
    //    |> Seq.toList

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

