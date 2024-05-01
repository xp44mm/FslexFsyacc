module FslexFsyacc.Runtime.Lex.DFAUtils

open FSharp.Idioms

let fromNFA(ntran:Set<uint32*'tag option*uint32>, nfaFinalLexemes:(uint32*uint32) list) =
    let subsetDfa = SubsetDFAUtils.create(ntran)
    let nfinals = 
        nfaFinalLexemes
        |> List.map fst

    let pdfa = PartitionDFAUtils.create(subsetDfa,nfinals)
    let encodeDfa, decodes, _ = pdfa|>PartitionDFAUtils.encode
    let mdfa = encodeDfa|>PartitionDFAUtils.minimize(decodes)

    let encodemDfa,_,encodes = mdfa|>PartitionDFAUtils.encode

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
        ||> List.zip
        |> List.map(fun((fnls,lxms) as pair)->
            let x = Set.intersect fnls lxms
            if x.IsEmpty then
                pair
            else
                failwithf "DFA.fromNFA:\r\nfinals=%A;\r\nlexemes=%A;" fnls lxms
        )

    {
        nextStates = nextStates
        finalLexemes = finalLexemes
    }

let fromRgx(patterns:RegularExpression<'tag> list list) =
    let nfa = AnalyzerNFAUtils.fromRgx patterns
    fromNFA(nfa.transition, nfa.finalLexemes)
