namespace FslexFsyacc.Lex
open FSharp.Idioms

/// DFA，他的状态是由NFA状态的集合表示
type SubsetDFA<'nstate,'a when 'nstate: comparison and 'a: comparison> =
    {
        dtran :Set<Set<'nstate>*'a*Set<'nstate>>
        allStates :Set<Set<'nstate>>
    }

    static member create (ntran:Set<'nstate*option<'a>*'nstate>) =
        let nfa = NFAOperations.create ntran

        let rec loop
            (dtran:Set<Set<'nstate>*'a*Set<'nstate>>)
            (dstates:Set<Set<'nstate>>)
            (newDstates:Set<Set<'nstate>>) =
        
            //由未完成遍歷的狀態，推導得到新的轉換詞條。
            let newDtrans:Set<Set<'nstate>*'a*Set<'nstate>> =
                newDstates
                |> Seq.collect(fun T ->
                    T
                    |> Set.filter(nfa.moves.ContainsKey)
                    |> Seq.collect(fun s -> nfa.moves.[s] |> Map.toSeq) // s in T, 起點具體來自哪裏不重要，丟棄數據。
                    |> Set.unionByKey // 相同字符可以到達的目標集合合並，並集運算
                    |> Set.map(fun(k, v) ->
                        let vv =
                            v
                            |> Set.map(fun t -> nfa.closures.[t]) // 每個目標向前空變換
                            |> Set.unionMany
                        k,vv
                    )
                    |> Set.map(fun (k, v) -> T, k, v)
                )
                |> Set.ofSeq
        
            //已經完成遍歷的狀態
            let overDstates = dstates + newDstates
        
            let dtran = dtran + newDtrans
        
            //未遍歷的狀態
            let newDstates =
                newDtrans
                |> Seq.map(fun(_,_,tgt)->tgt)
                |> Set.ofSeq
                |> Set.difference <| overDstates
        
            if newDstates.IsEmpty then
                dtran, (overDstates.Remove Set.empty)
            else
                loop dtran overDstates newDstates
        
        let startState = nfa.closures |> Seq.map(fun x -> x.Key) |> Seq.min
        
        let dtran, dstates =
            Set.singleton nfa.closures.[startState]
            |> loop Set.empty (Set.singleton Set.empty)
        
        {
            dtran = dtran;
            allStates = dstates;
        }
    
    ///3.8.3 查找接受狀態的算法
    member this.getAcceptingStates(nfinals:'nstate list) =
        let dstates = this.allStates
        //领取赠品，先到先得
        (dstates,nfinals)
        |> List.unfold(fun(states,ls) ->
            match ls with
            | [] -> None
            | final::tail ->
                let dstates,restStates =
                    states
                    |> Set.partition(Set.contains final)
                let nextState = restStates,tail
                Some(dstates,nextState)
        )

