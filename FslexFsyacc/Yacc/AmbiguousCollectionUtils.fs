module FslexFsyacc.Yacc.AmbiguousCollectionUtils
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.LALRs

open System

open FSharp.Idioms
open FSharp.Idioms.Literal

/// from ambiguous collection to (itemcore,lookahead)
let getItemcores (conflicts:Map<string,Set<ItemCore>>) =
    let flat =
        conflicts
        |> Map.toList
        |> List.collect(fun(sym,st)->
            st
            |> Set.toList
            |> List.map(fun item -> item,sym)
        )

    let fReduces,fGotos =
        flat
        |> List.partition(fun(itemCore,sym)->(ItemCoreUtils.dotmax itemCore))

    let reduces =
        fReduces
        |> List.groupBy fst
        |> List.map(fun(icore,group)-> // 合并lookaheads
            let lookaheads =
                group
                |> Seq.map snd
                |> Set.ofSeq
            icore, lookaheads
            )
        |> Set.ofList

    let shifts =
        fGotos
        |> List.map(fun(icore,_)-> icore, Set.empty)
        |> Set.ofList

    // itemcore with lookaheads
    let result:Map<ItemCore,Set<string>> =
        shifts+reduces
        |> Map.ofSeq
    result

let sortItemsByKernel (items:Map<ItemCore,Set<string>>) =
    let kernel,ext =
        items
        |> Map.toList
        |> List.partition(fun(ic,_)->
            ItemCoreUtils.isKernel ic
        )
    [
        yield! kernel
        yield! ext
    ]

/// shift/reduce
let isSRConflict(itemcores:Set<ItemCore>) =
    let reduces =
        itemcores
        |> Set.filter(ItemCoreUtils.dotmax)
    itemcores.Count > 1 && reduces.Count = 1

let isConflict (itemcores:Set<ItemCore>) =
    // 无需限定冲突符号必须是终结符号，
    // 冲突一定存在reduce，reduce的lookahead一定是终结符号
    itemcores.Count > 1 && 
    itemcores |> Set.exists((ItemCoreUtils.dotmax))

// map<item,lookaheads>
let renderItems (itemcores:list<int*(ItemCore*Set<string>)>) =
    itemcores
    |> List.map(fun(i,(ic,lookaheads)) ->
        let itemcore = RenderUtils.renderItemCore ic.production ic.dot
        if (ItemCoreUtils.dotmax ic) then
            let la =
                lookaheads
                |> Set.map RenderUtils.renderSymbol
                |> String.concat " "
            $"{i} {itemcore} / {la}"
        else
            $"{i} {itemcore}"
    )
    |> String.concat "\r\n"

let renderConflict 
    (kernels: Map<Set<ItemCore>,int>)
    (productions:Map<string list,int>)
    (lookahead:string)
    (itemcores:Set<ItemCore>)
    =
    let itemcoresBlock = 
        itemcores
        |> ActionUtils.from
        |> Set.map(fun action ->
            match action with
            | Reduce ic -> $"reduce using rule {productions.[ic]}"
            | Shift knl -> 
                if kernels.ContainsKey knl then
                    $"shift, and go to state {kernels.[knl]}"
                else failwith $"{stringify knl} not in {stringify kernels}"
        )
        |> String.concat "\r\n"

    [
        $"{RenderUtils.renderSymbol lookahead} :"
        itemcoresBlock |> Line.indentCodeBlock 4
    ]
    |> String.concat "\r\n"

/// 过滤掉假冲突，保留真冲突
let filterProperConflicts(conflicts: Map<int,Map<string,Set<ItemCore>>>) =
    conflicts
    |> Map.map(fun state conflicts ->
        conflicts
        |> Map.filter(fun la items ->
            isConflict items
        )
    )
    |> Map.filter(fun state conflicts ->
        //内层空，外层也排除
        not conflicts.IsEmpty
    )

/// 从冲突汇总产生式，以此得知哪些产生式必须指定优先级，以排除歧义。
let collectConflictedProductions(conflicts: Map<int,Map<string,Set<ItemCore>>>) =
    set [
        for KeyValue(state,cnflcts) in filterProperConflicts(conflicts) do
        for KeyValue(sym,st) in cnflcts do
        for icore in st do
        icore.production
    ]

/// 去重，即冲突项目仅保留一个，但是grammar,kernels,gotos仍然保持去重前的数值
let getUnambiguousItemCores
    (dummyTokens:Map<string list,string>)
    (precedences:Map<string,int>)
    (terminals:Set<string>)
    (conflictedItemCores:Map<int,Map<string,Set<ItemCore>>>)
    =

    let eliminator: AmbiguityEliminator =
        {
            terminals = terminals
            dummyTokens = dummyTokens
            precedences = precedences
        }

    conflictedItemCores
    |> Map.map(fun i closure ->
        closure
        |> Map.map(fun sym itemcores ->
            if isSRConflict(itemcores) then
                eliminator.disambiguate(itemcores)
            else
                itemcores
        )
        |> Map.filter(fun sym itemcores -> 
            // empty is nonassoc, will be error
            not itemcores.IsEmpty
            )
    )

let render
    (terminals)
    (conflicts: Map<int,Map<string,Set<ItemCore>>>)
    (kernels: Map<Set<ItemCore>,int>)
    =
    ///变换为(itemcore,lookaheads) list
    let getItemcores (conflicts:Map<string,Set<ItemCore>>) = 
        conflicts
        |> getItemcores
        |> sortItemsByKernel
        |> List.mapi(fun i x -> i+1,x)

    //过滤出点号在最后的产生式
    let getReducedProductions (itemlist:list<int*(ItemCore*_)>) =
        itemlist
        |> List.choose(fun(i,(itemCore,_))->
            if (ItemCoreUtils.dotmax itemCore) then
                Some(itemCore.production,i)
            else None
        )
        |> Map.ofList

    let properConflicts = filterProperConflicts(conflicts)

    //偏应用
    let renderConflict = renderConflict kernels

    let itemsBlock =
        conflicts
        |> Map.toList
        |> List.map(fun (state,conflicts) ->
            let properConflicts = 
                if properConflicts.ContainsKey state then
                    properConflicts.[state]
                else Map.empty

            let itemlist = getItemcores conflicts

            let productions = getReducedProductions itemlist

            let renderConflict = renderConflict productions

            let itemsBlock =
                itemlist
                |> renderItems

            let conflictsBlock =
                properConflicts
                |> Map.toList
                |> List.map(fun(la,items)->
                    renderConflict la items
                )
                |> String.concat "\r\n"

            let errorLookaheads = 
                conflicts
                |> Map.keys 
                |> Set.ofSeq
                |> Set.difference terminals

            let errorBlock =
                errorLookaheads
                |> Set.toList
                |> List.map RenderUtils.renderSymbol
                |> String.concat " "
                |> fun ls -> $"else lookaheads: {ls}"
            [
                $"state {state}:"
                itemsBlock |> Line.indentCodeBlock 4

                if properConflicts.IsEmpty then () else
                    conflictsBlock |> Line.indentCodeBlock 4

                if errorLookaheads.IsEmpty then () else
                    errorBlock |> Line.indentCodeBlock 4

            ] |> String.concat "\r\n"
        )
        |> String.concat "\r\n\r\n"

    let hh =
        if properConflicts.IsEmpty then
            ""
        else
            let ls = 
                properConflicts.Keys 
                |> Seq.map (fun i -> i.ToString())
                |> String.concat ","
            $"conflicted states: {ls}\r\n"
    hh + itemsBlock
