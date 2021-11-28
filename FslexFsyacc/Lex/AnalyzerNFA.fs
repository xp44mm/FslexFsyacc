namespace FslexFsyacc.Lex

/// analysis
type AnalyzerNFA<'a when 'a:comparison> =
    {
        transition:Set<uint32*'a option*uint32>
        finalLexemes:(uint32*uint32) list
    }

    static member unionMany (nfas:PatternNFA<'a> list) =
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

    static member fromRgx(lookaheads:RegularExpression<'a> list list) =
        (1u, lookaheads)
        |> List.unfold(fun(i,lookaheads) ->
            match lookaheads with
            | [] -> None
            | h::t ->
                let nfa = PatternNFA.fromRgx(i, h)
                let nextState = nfa.maxState+1u, t
                Some(nfa,nextState)
        )
        |> AnalyzerNFA.unionMany