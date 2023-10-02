module FslexFsyacc.Yacc.GrammarCrewUtils
open FslexFsyacc.Runtime

open FSharp.Idioms

//从文法生成增广文法
let getProductionsCrew(mainProductions:string list list) =
    let startSymbol = mainProductions.[0].[0]

    let mainProductions = set mainProductions

    let augmentedProductions =
            mainProductions
            |> Set.add ["";startSymbol]

    ProductionsCrew(
        mainProductions,
        augmentedProductions
    )

let getNullableCrew(prototype:ProductionsCrew) =
    let symbols =
        prototype.mainProductions 
        |> List.concat 
        |> Set.ofList

    let nonterminals = 
        prototype.mainProductions
        |> Set.map List.head 
    
    let terminals = Set.difference symbols nonterminals

    let nullables = 
        prototype.mainProductions 
        |> NullableFactory.make 

    NullableCrew(prototype,symbols,nonterminals,terminals,nullables)

let getFirstLastCrew(prototype:NullableCrew) =
    let firsts = FirstFactory.make prototype.nullables prototype.mainProductions
    let lasts  = LastFactory.make prototype.nullables prototype.mainProductions
    FirstLastCrew(prototype,firsts,lasts)

let getFollowPrecedeCrew(prototype:FirstLastCrew) =
    let follows  = FollowFactory.make prototype.nullables prototype.firsts prototype.augmentedProductions
    let precedes = PrecedeFactory.make prototype.nullables prototype.lasts prototype.augmentedProductions
    FollowPrecedeCrew(prototype,follows,precedes)

let getItemCoresCrew (prototype:FollowPrecedeCrew) =
    let itemCores = 
        prototype.augmentedProductions
        |> ItemCoreUtils.make
        |> Seq.map(fun itemCore ->
            let prodCrew =
                itemCore.production
                |> ProductionCrewUtils.getProductionCrew
            let itemCoreCrew =
                ItemCoreCrewUtils.getItemCoreCrew(prodCrew,itemCore.dot)
            //let itemCoreLookaheadFactorCrew =
            //    ItemCoreCrewUtils.getItemCoreLookaheadFactorCrew (prototype) itemCoreCrew
            itemCore,itemCoreCrew
        )
        |> Map.ofSeq
    ItemCoresCrew(prototype,itemCores)

let getClosure (prototype:ItemCoresCrew) =
    let augmentedProductions = prototype.augmentedProductions
    let itemCores = prototype.itemCoreCrews
    let terminals = prototype.terminals
    let firsts = prototype.firsts
    let nullables = prototype.nullables

    /// ItemCore*Set<lookahead:string> 返回Items
    let rec loop (acc:Set<ItemCore*Set<string>>) (items: Set<ItemCore*Set<string>>) =
        let newAcc =
            items
            |> Seq.choose(fun(itemCore,las) ->
                let itemCoreCrew = itemCores.[itemCore]
                if itemCoreCrew.dotmax then
                    None
                else
                    let nextSymbol = itemCoreCrew |> ItemCoreCrewUtils.getNextSymbol
                    if nextSymbol |> terminals.Contains then
                        None
                    else
                        let nullable = NullableFactory.nullable nullables
                        let first = FirstFactory.first nullables firsts
                        let beta = ItemCoreCrewUtils.getBeta itemCoreCrew
                        let lookaheads = 
                            if nullable beta then las else Set.empty
                            |> Set.union (first beta)
                        Some(nextSymbol, lookaheads)
            )
            |> Seq.map(fun(nextSymbol, lookaheads) -> //扩展闭包一次。找到所有nextSymbol的产生式
                augmentedProductions
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

let getNextKernels (collection:ItemCoresCrew) (closure:Set<ItemCore*Set<string>>) =
    let itemCores = collection.itemCoreCrews

    closure
    |> Set.filter(fun(itemCore,lookahead) -> not itemCores.[itemCore].dotmax)
    |> Set.map(fun(itemCore,lookahead) -> 
        {itemCore with dot = itemCore.dot + 1},lookahead)
    |> Set.groupBy(fun(itemCore,lookahead) -> 
        let itemCoreCrew = itemCores.[itemCore]
        ItemCoreCrewUtils.getPrevSymbol itemCoreCrew)

/// the collection of sets of items
let getClosureCollection (grammar:ItemCoresCrew) =
    // kernel -> closure
    let getClosure = getClosure grammar
    let itemCores = grammar.itemCoreCrews |> Map.keys

    // 获取语法集合中所有的kernels
    let rec loop (oldKernels:Set<Set<ItemCore*Set<string>>>) (newKernels:Set<Set<ItemCore*Set<string>>>) =
        let fullKernels = Set.union oldKernels newKernels

        // 新增的clrkernel，有重复的slrkernel
        let newKernels =
            newKernels
            |> Set.map(fun kernel -> 
                getClosure kernel
                |> getNextKernels grammar
                |> Set.map snd
                )
            |> Set.unionMany
            |> Set.filter(not << KernelUtils.isSubsetOf fullKernels)

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
                    KernelUtils.mergeClrKernels slrKernel clrKernels
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
    /// kernel*closure的集合
    let result = 
        loop Set.empty (Set.singleton k0)
        |> Set.map(fun kernel ->
            ClosureOperators.getCore kernel, getClosure kernel
            )
    result

