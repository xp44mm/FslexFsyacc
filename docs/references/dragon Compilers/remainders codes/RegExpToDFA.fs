namespace OptimizationLex

open Compiler

///re:augment, position
type RegExpToDFA<'a when 'a:comparison>(re:RegularExpression<int*'a>) =
    let startStates = RegExpPositions.firstpos re

    let symbols = re |> RegExpModule.leaves |> Map.ofList

    let followpos i =
        let mp = RegExpPositions.followpos re
        if mp.ContainsKey i then mp.[i] else Set.empty

    /// out trans from a set of states
    let getDtransFrom (src:Set<int>) =
        src
        |> Seq.map(fun pos -> symbols.[pos], pos)
        |> Seq.groupBy fst //符号相同，或者说字母相同
        |> Seq.map(fun (lbl,ls) ->
            let tgt =
                ls
                |> Seq.map snd
                |> Seq.map(followpos)
                |> Set.unionMany
            src,lbl,tgt
        )
        |> Seq.toList

    member this.Dstart = startStates
    //member this.Dtrans = Dtransitions.getDtrans getDtransFrom startStates

