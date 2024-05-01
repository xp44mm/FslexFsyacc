namespace FslexFsyacc.Runtime.LALRs

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FSharp.Idioms

type SpreadClosure =
    {
        items:Set<string*ItemCore> // lookahead * itemcore
    }
    static member just(items) = { items = items }

    ///展开lookaheads的closure
    static member from (closure:Set<ItemCore*Set<string>>) = 
        closure
        |> Seq.collect(fun (itemCore,lookaheads) ->
            let lookaheads =
                if itemCore.dotmax then
                    lookaheads // 只有reduce需要lookahead符号，其他情况不需要。
                else
                    //GOTO的nextSymbol分为两种情况：
                    //当nextSymbol是terminal时用于侦察冲突，当nextSymbol是nonterminal时仅用于占位
                    Set.singleton itemCore.nextSymbol
            lookaheads
            |> Set.toList
            |> List.map(fun lookahead -> lookahead, itemCore)
        )
        |> Set.ofSeq
        |> SpreadClosure.just

    member this.getKernel () =
        this.items
        |> Set.filter(fun (_, itemCore) -> itemCore.isKernel )
        |> Set.map snd

    member this.getConflicts () =
        this.items
        |> Seq.groupBy fst // lookahead
        |> Map.ofSeq
        |> Map.map(fun lookahead sq ->
            sq
            |> Seq.map snd // itemcore
            |> Set.ofSeq
        )

