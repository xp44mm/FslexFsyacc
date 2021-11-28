module FslexFsyacc.Yacc.CollectionFactory

open FSharp.Idioms

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
let mergeClrKernels (slrKernel:Set<ItemCore>) (clrKernels:seq<Set<_*Set<string>>>) =
    // 等价代码
    //clrKernels
    //|> Set.unionMany
    //|> Set.unionByKey

    /// 直接按次序合并集合
    let lookaheadsArray =
        clrKernels
        |> Seq.map ClosureOperators.getLookaheads
        |> Array.transpose
        |> Array.map Set.unionMany

    slrKernel
    |> Set.toArray
    |> Array.zip <| lookaheadsArray
    |> Set.ofArray



/// 
let make (itemCores:Set<ItemCore>) (itemCoreAttributes:Map<ItemCore, bool*Set<string>>) (productions:Set<string list>) =

    let getClosure = ClosureFactory.make itemCoreAttributes productions

    let rec loop (oldKernels:Set<Set<ItemCore*Set<string>>>) (newKernels:Set<Set<ItemCore*Set<string>>>) =
        let fullKernels = oldKernels + newKernels
        // 新增的clrkernel，有重复的slrkernel
        let newKernels =
            newKernels
            |> Set.map(fun kernel -> 
                getClosure kernel
                |> ClosureOperators.getNextKernels
                |> Set.map snd
                )
            |> Set.unionMany
            |> Set.filter(not << isSubsetOf fullKernels)

        if Set.isEmpty newKernels then
            fullKernels
        else
            // 新的全集，无重复的slrkernel
            let fullKernels =
                newKernels
                |> Set.union fullKernels
                |> Seq.groupBy(fun clrKernel ->
                    let slrKernel = Set.map fst clrKernel
                    slrKernel
                    )
                |> Seq.map(fun(slrKernel:Set<ItemCore>,clrKernels:seq<Set<ItemCore*Set<string>>>)-> //合并同一slrkernel下的所有clrKernel
                    mergeClrKernels slrKernel clrKernels
                )
                |> Set.ofSeq

            let newSlrKernels =
                newKernels
                |> Set.map(fun clrKernel ->
                    let slrKernel = clrKernel |> Set.map fst
                    slrKernel
                    )
            let newKernels, oldKernels =
                fullKernels
                |> Set.partition(fun clrKernel -> 
                    let slrKernel = clrKernel |> Set.map fst
                    newSlrKernels.Contains slrKernel
                )
            loop oldKernels newKernels

    let k0 = Set.singleton (itemCores.MinimumElement, Set.singleton "")
    loop Set.empty (Set.singleton k0)
    |> Set.map(fun kernel ->
        ClosureOperators.getCore kernel, getClosure kernel
        )