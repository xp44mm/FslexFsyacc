namespace FslexFsyacc.Yacc
open FslexFsyacc.Runtime
open FSharp.Idioms

type AmbiguousCollection =
    {
        grammar : Grammar
        kernels : Map<Set<ItemCore>,int> // kernel -> state
        closures: Map<int,Map<string,Set<ItemCore>>> // state -> (lookahead/leftside) -> conflicts
        GOTOs   : Map<int,Map<string,Set<ItemCore>>> // state -> (lookahead/leftside) -> kernel
    }

    ///此函数的结果数据是焦点，交通要道。
    static member create(mainProductions:string list list) =
        let x = LALRCollection.create mainProductions
        let closures =
            x.closures
            |> Map.map(fun i closure ->
                closure
                |> Seq.groupBy(fun (la,_) -> la)
                |> Map.ofSeq
                |> Map.map(fun la ls ->
                    ls
                    |> Seq.map snd // itemcore
                    |> Set.ofSeq
                )
            )
        let GOTOs = x.getGOTOs()

        {
            grammar = x.grammar
            kernels = x.kernels
            closures = closures
            GOTOs = GOTOs
        }

    /// 显示冲突状态的冲突项目
    member this.filterConflictedClosures() =
        this.closures
        |> Map.map(fun i closure ->
            closure
            |> Map.filter(fun la st ->
                this.grammar.terminals.Contains la
                && st.Count > 1
                && st |> Set.exists(fun ic -> ic.dotmax)
            )
        )
        |> Map.filter(fun i closure -> not closure.IsEmpty)

    /// 汇总整个Augment Grammar的产生式
    static member gatherProductions (closures:Map<int,Map<string,Set<ItemCore>>>) =
        closures
        |> Map.toSeq
        |> Seq.map (fun(i,closure)->
            closure
            |> Map.toSeq
            |> Seq.map (fun(s,ls)->
                ls |> Set.map(fun icore -> icore.production)
            )
            |> Seq.concat
            |> Set.ofSeq
        )
        |> Set.unionMany

    /// 去重
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
            this.closures
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
                closures = unambiguousClosures
        }

    /// convert real kernel to int state
    member this.getGotos() =
        this.GOTOs
        |> Map.map(fun state mp ->
            mp
            |> Map.map(fun symbol items ->
                this.kernels.[items] // convert to int state
            )
        )

    member this.render() =

        let gotos = this.getGotos()

        let itemBlock = 
            this.closures
            |> Map.toList
            |> List.map(fun(state,conflicts) ->
                let gotos1 = gotos.[state]
                let itemslist = 
                    conflicts
                    |> AmbiguousCollectionUtils.getItems
                    |> Map.toList
                    |> AmbiguousCollectionUtils.sortItemsByKernel
                    |> List.mapi(fun i x -> i+1,x)

                let itemsBlock =
                    itemslist
                    |> AmbiguousCollectionUtils.renderItems

                // for reduce
                let productions =
                    itemslist
                    |> List.choose(fun(i,(ic,_))->
                        if ic.dotmax then
                            Some(ic.production,i)
                        else None
                    )
                    |> Map.ofList
                    
                let properConflicts =
                    conflicts
                    |> Map.filter(fun la items -> 
                        //this.grammar.terminals.Contains la && 
                        AmbiguousCollectionUtils.isConflict items
                    )
                    |> Map.toList

                let renderConflict = AmbiguousCollectionUtils.renderConflict this.kernels productions
                let conflictsBlock =
                    if properConflicts.IsEmpty then
                        ""
                    else
                        properConflicts
                        |> List.map(fun(la,items)-> 
                            renderConflict la items
                            //AmbiguousCollectionUtils.renderConflict la items gotos1.[la] itemsMap
                        )
                        |> String.concat "\r\n"

                [
                    $"state {state}:"
                    itemsBlock |> Line.indentCodeBlock 4
                    conflictsBlock |> Line.indentCodeBlock 4
                ] |> String.concat "\r\n"
            )
            |> String.concat "\r\n"

        itemBlock
