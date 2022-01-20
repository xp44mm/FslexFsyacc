namespace FslexFsyacc.Lex

open FSharp.Idioms

/// DFA
type DFA2<'tag when 'tag: comparison> =
    {
        /// DFA nextState(s,a)
        nextStates:Map<uint32,Map<'tag,uint32>>

        ///每个正则表达式对应的接受狀態：finals，实际取词状态：lexemes
        finalLexemes:(Set<uint32>*Set<uint32>)[]
    }

    static member fromNFA(ntran:Set<uint32*'tag option*uint32>, nfaFinalLexemes:(uint32*uint32) list) =
        let subsetDfa = SubsetDFA<_,_>.create(ntran)
        let nfinals = 
            nfaFinalLexemes
            |> List.map fst

        let pdfa = PartitionDFA<_,_>.create(subsetDfa,nfinals)
        let encodeDfa, decodes, _ = pdfa.encode()
        let mdfa = encodeDfa.minimize(decodes)

        let encodemDfa,_,encodes = mdfa.encode()

        //每个位置对应的lexemes状态
        let lexemes =
            nfaFinalLexemes
            |> List.map(fun(fnl,lxm) ->
                if fnl = lxm then
                    Set.empty
                else
                    // the accepting states of D are all those sets of N's states 
                    // that include at least one accepting state of N.
                    mdfa.allStates
                    |> Set.filter(fun dstate -> dstate.Contains lxm) // nfa -> dfa
                    |> Set.map(fun dstate -> encodes.[dstate]) // encode dfa
            )

        let transitions = encodemDfa.dtran

        let nextStates = 
            transitions
            |> DFATools.getNextStates
        
        let finalLexemes = 
            (encodemDfa.dfinals, lexemes)
            ||> Seq.zip
            |> Array.ofSeq
            |> Array.map(fun((fnls,lxms) as pair)->
                let x = Set.intersect fnls lxms
                if x.IsEmpty then
                    pair
                else
                    failwith "猜测finals和lexemes应该没有交集"
            )

        {
            nextStates = nextStates
            finalLexemes = finalLexemes
        }

    static member fromRgx(patterns:RegularExpression<'tag> list list) =
        let nfa = AnalyzerNFA.fromRgx patterns
        DFA2.fromNFA(nfa.transition, nfa.finalLexemes)
    