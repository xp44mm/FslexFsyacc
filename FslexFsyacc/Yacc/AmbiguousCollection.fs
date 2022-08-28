namespace FslexFsyacc.Yacc

type AmbiguousCollection =
    {
        grammar : Grammar
        kernels : Map<Set<ItemCore>,int> // kernel -> index
        closures: Map<int,Map<string,Set<ItemCore>>> // index -> symbol -> itemcores
        GOTOs   : Map<int,Map<string,Set<ItemCore>>> // index -> symbol -> kernel
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

    /// 汇总产生式
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
        names:Map<string list,string>,
        precedences:Map<string,int>
        ) =

        let eliminator =
            {
                terminals = this.grammar.terminals
                names = names
                precedences = precedences
            }:AmbiguityEliminator

        let unambiguousClosures =
            this.closures
            |> Map.map(fun i closure ->
                closure
                |> Map.map(fun la itemcores ->
                    if itemcores.Count > 1 then 
                        eliminator.disambiguate(itemcores)
                    else 
                        itemcores
                )
                |> Map.filter(fun la itemcores -> not itemcores.IsEmpty)
            )
        {
            this with
                closures = unambiguousClosures
        }

