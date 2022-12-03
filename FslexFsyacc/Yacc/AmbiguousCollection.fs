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

    /// 显示冲突状态的冲突项目
    [<Obsolete("this.render")>]
    member this.filterConflictedClosures() =
        this.conflicts
        |> Map.map(fun i closure ->
            closure
            |> Map.filter(fun la st ->
                this.grammar.terminals.Contains la
                && st.Count > 1
                && st |> Set.exists(fun ic -> ic.dotmax)
            )
        )
        |> Map.filter(fun i closure -> not closure.IsEmpty)

    /// 去重，即冲突项目仅保留一个，但是grammar,kernels,gotos仍然保持去重前的数值
    member this.toUnambiguousCollection(
        dummyTokens:Map<string list,string>,
        precedences:Map<string,int>
        ) =

        let eliminator =
            {
                terminals = this.grammar.terminals
                dummyTokens = dummyTokens
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
                |> Map.filter(fun sym itemcores -> not itemcores.IsEmpty)
            )
        {
            this with
                conflicts = unambiguousClosures
        }

    member this.render() =
        ///变换为(itemcore,lookaheads)list
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

        ///过滤出真正的冲突
        let getProperConflicts (conflicts:Map<string,Set<ItemCore>>) =
            conflicts
            |> Map.filter(fun la items ->
                AmbiguousCollectionUtils.isConflict items
            )
            |> Map.toList
        //偏应用
        let renderConflict = AmbiguousCollectionUtils.renderConflict this.kernels

        let itemsBlock =
            this.conflicts
            |> Map.toList
            |> List.map(fun (state,conflicts) ->
                let itemlist = getItemcores conflicts

                let productions = getReducedProductions itemlist
                let properConflicts = getProperConflicts conflicts

                let renderConflict = renderConflict productions

                let itemsBlock =
                    itemlist
                    |> AmbiguousCollectionUtils.renderItems

                let conflictsBlock =
                    if properConflicts.IsEmpty then
                        ""
                    else
                        properConflicts
                        |> List.map(fun(la,items)->
                            renderConflict la items
                        )
                        |> String.concat "\r\n"

                [
                    $"state {state}:"
                    itemsBlock |> Line.indentCodeBlock 4
                    conflictsBlock |> Line.indentCodeBlock 4
                ] |> String.concat "\r\n"
            )
            |> String.concat "\r\n"

        itemsBlock
