module FslexFsyacc.Yacc.AmbiguousCollectionUtils
open FSharp.Literals.Literal

/// from ambiguous collection to (itemcore,lookahead)
let getItems (conflicts:Map<string,Set<ItemCore>>) =
    let flat =
        conflicts
        |> Map.toList
        |> List.collect(fun(sym,st)->
            st
            |> Set.toList
            |> List.map(fun item -> item,sym)
        )

    let fReduces,fGotos =
        //conflicts
        //|> Map.partition(fun la (icores:Set<ItemCore>) -> icores.MinimumElement.dotmax)
        flat
        |> List.partition(fun(item,sym)->item.dotmax)

    let reduces =
        fReduces
        //|> Map.toList
        //|> List.collect(fun(icores,la) -> 
        //    icores 
        //    |> Set.map(fun item -> item, la)
        //    |> Set.toList
        //)
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
        //|> Map.values
        //|> Set.unionMany
        |> List.map(fun(icore,_)-> icore, Set.empty)
        |> Set.ofList

    // itemcore -> lookaheads
    let result:Map<ItemCore,Set<string>> =
        shifts+reduces
        |> Map.ofSeq
    result

let sortItemsByKernel (items:list<ItemCore*Set<string>>) =
    let kernel,ext =
        items
        |> List.partition(fun(ic,_)->
            ic.isKernel()
        )
    [
        yield! kernel
        yield! ext
    ]

open FslexFsyacc.Runtime

// map<item,lookaheads>
let renderItems (itemcores:list<int*(ItemCore*Set<string>)>) =
    itemcores
    |> List.map(fun(i,(ic,lookaheads)) ->
        let itemcore = RenderUtils.renderItemCore ic.production ic.dot
        if ic.dotmax then
            let la =
                lookaheads
                |> Set.map RenderUtils.renderSymbol
                |> String.concat " "
            $"{i} {itemcore} / {la}"
        else
            $"{i} {itemcore}"
    )
    |> String.concat "\r\n"

/// shift/reduce
let isSRConflict(itemcores:Set<ItemCore>) =
    let reduces =
        itemcores
        |> Set.filter(fun i -> i.dotmax)
    itemcores.Count > 1 && reduces.Count = 1

let isConflict (itemcores:Set<ItemCore>) =
    // 无需限定冲突符号必须是终结符号，
    // 冲突一定存在reduce，reduce的lookahead一定是终结符号
    itemcores.Count > 1 && 
    itemcores |> Set.exists(fun i -> i.dotmax)


open FSharp.Idioms

let renderConflict 
    (kernels: Map<Set<ItemCore>,int>)
    (productions:Map<string list,int>)
    (lookahead:string)
    (itemcores:Set<ItemCore>)
    =
    let itemcoresBlock = 
        itemcores
        |> Action.from
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

