module FslexFsyacc.Lex.AnalyzerNFAUtils

let unionMany (nfas:PatternNFA<'a> list) =
    {
        transition =
            nfas
            |> List.map(fun nfa ->
                nfa.transition
                |> Set.add(0u,None,nfa.minState)
            )
            |> Set.unionMany

        finalLexemes =
            nfas
            |> List.map(fun nfa -> nfa.maxState,nfa.lexemeState)
        
    }

let fromRgx(lookaheads:RegularExpression<'a> list list) =
    (1u, lookaheads)
    |> List.unfold(fun(i,lookaheads) ->
        match lookaheads with
        | [] -> None
        | h::t ->
            let nfa = PatternNFAUtils.fromRgx(i, h)
            let nextState = nfa.maxState+1u, t
            Some(nfa,nextState)
    )
    |> unionMany
