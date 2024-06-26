﻿namespace FslexFsyacc.BNFs

open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FSharp.Idioms

type LALR = 
    {
    // ItemCore -> Lookaheads
    items: Set<ItemCore*Set<string>>
    }

    static member just(items) = { items = items }
    static member init(production) = 
        Set.singleton (ItemCore.just(production, 0), Set.singleton "")
        |> LALR.just

    /// 
    static member aggregate (itemCores: #seq<ItemCore*Set<string>>) =
        {
        items = 
            itemCores
            |> Seq.groupBy fst
            |> Seq.map(fun (i,sq) ->
                sq
                |> Seq.map snd // lookahead
                |> Set.unionMany
                |> Pair.ofApp i)
            |> Set.ofSeq
        }

    /// SLR不带LookAhead
    member this.getSLR() =
        Set.map fst this.items

    member this.getLookaheads() =
        Seq.map snd this.items
        
    /// 获取一个闭包的kernel
    member closure.getKernel() =
        closure.items
        |> Set.filter(fun (itemCore, _) -> itemCore.isKernel )

    /// 获取一个kernel/closure的Symbol
    member this.getSymbol() =
        let itemCore = 
            this.items
            |> Seq.map fst
            |> Seq.find (fun itemCore -> itemCore.isKernel )

        itemCore.prevSymbol

    /// 从当前closure，返回下一组kernels
    // 循环的时候不需要记住路径
    member closure.getNextKernels () =
        closure.items
        |> Seq.filter(fun (itemCore, _) -> not itemCore.dotmax )
        |> Seq.map(fun(itemCore, lookaheads) ->
            { itemCore with dot = itemCore.dot + 1 }, lookaheads
            )
        |> Seq.groupBy(fun(itemCore,_) -> itemCore.prevSymbol )
        |> Seq.map( snd >> Set.ofSeq )
        |> Set.ofSeq

    //member closure.goto() =
    //    closure.getNextKernels()
    //    |> Seq.map snd




