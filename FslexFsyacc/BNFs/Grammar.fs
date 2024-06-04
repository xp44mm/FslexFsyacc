module FslexFsyacc.BNFs.Grammar

open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FSharp.Idioms

/// itemcore.preface(nullable,first)
let prefaceLookahead (itemCore:ItemCore) (lookahead:Set<string>) (grammar: Grammar) =
    let laBeta = grammar.first itemCore.beta
    if grammar.nullable itemCore.beta then
        Set.union laBeta lookahead
    else laBeta

/// lalr.kerneltoclosure(nonterminals,nullable,first,productions)
let kernelToClosure (grammar: Grammar) =
    let rec loop (acc:Set<ItemCore*Set<string>>) (items: Set<ItemCore*Set<string>>) =
        let newAcc =
            items
            |> Seq.filter(fun (itemCore,las) -> not itemCore.dotmax)
            |> Seq.filter(fun (itemCore,las) -> grammar.nonterminals.Contains itemCore.nextSymbol)
            |> Seq.map(fun(itemCore,las) ->
                let lookaheads = prefaceLookahead itemCore las grammar
                itemCore.nextSymbol, lookaheads
            )
            |> Seq.map(fun(nextSymbol, lookaheads) -> //扩展闭包一次。找到所有nextSymbol的产生式
                grammar.productions
                |> Set.filter(fun p -> p.Head = nextSymbol)
                |> Set.map(fun p -> {production=p; dot=0}, lookaheads)
            )
            |> Set.ofSeq
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

/// the collection of sets of items
let getKernelCollection (grammar:Grammar) =
    // kernel -> closure
    let toClosure = kernelToClosure grammar
    //let itemCores = grammar.itemCoreCrews |> Map.keys |> Set.ofSeq

    // 获取语法集合中所有的kernels
    let rec loop (fullKernels:LALRCollection) (newKernels:LALRCollection) =
        // 新增的clrkernel，有重复的slrkernel
        let kernels = newKernels.newKernels toClosure

        // 新的全集，无重复的slrkernel
        let newFullKernels = fullKernels.union kernels

        let newKernels = newFullKernels.difference fullKernels

        if newKernels.isEmpty then
            newFullKernels
        else
            loop newFullKernels newKernels
    let i0 = ItemCore.just(grammar.productions.MinimumElement,0)
    let k0 = 
        (i0, Set.singleton "")
        |> Set.singleton
        |> Set.singleton
        |> LALRCollection.just 
            
    /// kernel的集合
    loop k0 k0
