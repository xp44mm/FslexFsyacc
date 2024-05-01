module FslexFsyacc.Yacc.KernelUtils
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.LALRs

open FSharp.Idioms

///kernel中的每一项lookahead集合都是kernels的相同ItemCore的子集。
let isSubsetOf (kernels:Set<Set<ItemCore*Set<string>>>) (kernel:Set<ItemCore*Set<string>>) =
    let slrKernel0 = Set.map fst kernel
    let lookaheadsets0 = kernel |> Set.toList |> List.map snd
    kernels
    |> Seq.tryFind(fun clrKernel ->
        let slrKernel = clrKernel |> Set.map fst
        if slrKernel = slrKernel0 then
            let lookaheadsets = clrKernel |> Set.toList |> List.map snd
            let zp = List.zip lookaheadsets0 lookaheadsets
            zp |> List.forall(fun (st0,st) -> st0.IsSubsetOf st)
        else
            false
    )
    |> Option.isSome

//合并同一slrkernel下的所有clrKernel
let mergeClrKernels (slrKernel:Set<ItemCore>) (clrKernels:seq<Set<ItemCore*Set<string>>>) =
    //// lookaheadsArray的等价代码
    //clrKernels
    //|> Set.unionMany
    //|> Set.unionByKey

    let x =
        clrKernels
        |> Seq.map ClosureOperators.getLookaheads

    /// 直接按次序合并集合
    let lookaheadsList =
        x
        |> List.transpose
        |> List.map Set.unionMany

    slrKernel
    |> Set.toList
    |> List.zip <| lookaheadsList
    |> Set.ofList
