namespace FslexFsyacc.Yacc
open FslexFsyacc.Runtime
open FSharp.Idioms
open System

type AmbiguousCollection =
    {
        grammar : Grammar
        /// kernel -> state
        kernels : Map<Set<ItemCore>,int>
        /// state -> (lookahead/leftside) -> kernel
        GOTOs   : Map<int,Map<string,Set<ItemCore>>>
        /// state -> (lookahead/leftside) -> conflicts
        conflicts: Map<int,Map<string,Set<ItemCore>>>
    }

    ///此函数的结果数据是焦点，交通要道。
    static member create(mainProductions:string list list) =
        let x = LALRCollection.create mainProductions
        let conflicts =
            x.closures
            |> Map.map(fun i closure ->
                closure
                |> Seq.groupBy fst
                |> Map.ofSeq
                |> Map.map(fun _ sq ->
                    sq
                    |> Seq.map snd // itemcore
                    |> Set.ofSeq
                )
            )

        {
            grammar = x.grammar
            kernels = x.kernels
            GOTOs = x.getGOTOs()
            conflicts = conflicts
        }

    /// 过滤掉假冲突，保留真冲突
    member this.filterProperConflicts() =
        this.conflicts
        |> Map.map(fun state conflicts ->
            conflicts
            |> Map.filter(fun la items ->
                AmbiguousCollectionUtils.isConflict items
            )
        )
        |> Map.filter(fun state conflicts ->
            //内层空，外层也排除
            not conflicts.IsEmpty
        )

    //[<Obsolete("this.filterProperConflicts")>]
    //member this.filterConflictedClosures() = 
    //    this.filterProperConflicts()

    /// 从冲突汇总产生式，以此得知哪些产生式必须指定优先级，以排除歧义。
    member this.collectConflictedProductions() =
        set [
            for KeyValue(state,cnflcts) in this.filterProperConflicts() do
            for KeyValue(sym,st) in cnflcts do
            for icore in st do
            icore.production
        ]

    /// 去重，即冲突项目仅保留一个，但是grammar,kernels,gotos仍然保持去重前的数值
    member this.toUnambiguousCollection(
        prodTokens:Map<string list,string>,
        precedences:Map<string,int>
        ) =

        let eliminator =
            {
                terminals = this.grammar.terminals
                prodTokens = prodTokens
                precedences = precedences
            }:AmbiguityEliminator

        let unambiguousClosures =
            this.conflicts
            |> Map.map(fun i closure ->
                closure
                |> Map.map(fun sym itemcores ->
                    if AmbiguousCollectionUtils.isSRConflict(itemcores) then
                        eliminator.disambiguate(itemcores)
                    else
                        itemcores
                )
                |> Map.filter(fun sym itemcores -> 
                    // empty is nonassoc, will be error
                    not itemcores.IsEmpty
                    )
            )
        {
            this with
                conflicts = unambiguousClosures
        }


    member this.render() =
        ///变换为(itemcore,lookaheads) list
        let getItemcores (conflicts:Map<string,Set<ItemCore>>) = 
            conflicts
            |> AmbiguousCollectionUtils.getItemcores
            |> AmbiguousCollectionUtils.sortItemsByKernel
            |> List.mapi(fun i x -> i+1,x)

        //过滤出点号在最后的产生式
        let getReducedProductions (itemlist:list<int*(ItemCore*_)>) =
            itemlist
            |> List.choose(fun(i,(ic,_))->
                if ic.dotmax then
                    Some(ic.production,i)
                else None
            )
            |> Map.ofList

        let properConflicts = this.filterProperConflicts()

        //偏应用
        let renderConflict = AmbiguousCollectionUtils.renderConflict this.kernels

        let itemsBlock =
            this.conflicts
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
                    |> AmbiguousCollectionUtils.renderItems

                let conflictsBlock =
                    properConflicts
                    |> Map.toList
                    |> List.map(fun(la,items)->
                        renderConflict la items
                    )
                    |> String.concat "\r\n"

                let errorLookaheads = 
                    this.grammar.terminals - Map.keys conflicts

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
