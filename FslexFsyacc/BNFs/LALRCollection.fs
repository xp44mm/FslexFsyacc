namespace FslexFsyacc.BNFs

open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FSharp.Idioms

/// 传播用的集合
type LALRCollection = 
    {
    collection: Set<Set<ItemCore*Set<string>>> // Set< kernel|closure >
    }

    static member just(collection) = { collection = collection }

    /// 查看项集合并，看看原文是什么名字
    static member aggregate (kernels: #seq<Set<ItemCore*Set<string>>>) =
        kernels
        |> Seq.groupBy(fun clrKernel -> LALR.just(clrKernel).getSLR())
        |> Seq.map(fun (slrKernel:Set<ItemCore>,clrKernels:seq<Set<ItemCore*Set<string>>>) ->
            /// 直接按次序合并集合
            let lookaheadsList =
                clrKernels
                |> Seq.map(fun (items:Set<ItemCore*Set<string>>) ->
                    items
                    |> Set.toList
                    |> List.map snd)
                |> List.transpose
                |> List.map Set.unionMany
            lookaheadsList
            |> Seq.zip slrKernel
            |> Set.ofSeq
        )
        |> Set.ofSeq
        |> LALRCollection.just

    member this.newKernels(toClosure:Set<ItemCore*Set<string>>->Set<ItemCore*Set<string>>) =
        this.collection
        |> Set.map(fun kernel ->
            let clr =
                kernel
                |> toClosure
                |> LALR.just
            clr.getNextKernels()
            )
        |> Set.unionMany

    member this.union(that) =
        LALRCollection.aggregate [ yield! this.collection; yield! that ]

    member this.union(that:LALRCollection) = this.union that.collection

    member this.difference(collection) =
        Set.difference this.collection collection
        |> LALRCollection.just

    member this.difference(that:LALRCollection) = this.difference that.collection

    member this.isEmpty = this.collection.IsEmpty
