namespace FslexFsyacc.Lex

//open FSharp.Idioms

///// DFA
//type DFA<'tag when 'tag: comparison> =
//    {
//        /// DFA nextState(s,a)
//        nextStates:Map<uint32,Map<'tag,uint32>>
//        //鍵：單個的接受狀態，查找值：lexeme State集合。
//        lexemesFromFinal:Map<uint32,Set<uint32>>
//        ///所有的finals狀態，从lexeme状态，回退到final状态。
//        universalFinals:Set<uint32>
//        ///接受状态查找对应的索引
//        indicesFromFinal:Map<uint32,int>

//    }

//    static member fromNFA(ntran:Set<uint32*'tag option*uint32>, nfaFinalLexemes:(uint32*uint32) list) =
//        let subsetDfa = SubsetDFA<_,_>.create(ntran)
//        let nfinals = 
//            nfaFinalLexemes
//            |> List.map fst

//        let pdfa = PartitionDFA<_,_>.create(subsetDfa,nfinals)
//        let encodeDfa, decodes, _ = pdfa.encode()
//        let mdfa = encodeDfa.minimize(decodes)

//        let encodemDfa,_,encodes = mdfa.encode()
//        let lexemes =
//            nfaFinalLexemes
//            |> List.map(fun(f,l) ->
//                if f = l then
//                    Set.empty
//                else
//                    //the accepting states of D are all those sets of N's states that include at least one accepting state of N.
//                    mdfa.allStates
//                    |> Set.filter(fun dstate -> dstate.Contains l)
//                    |> Set.map(fun dstate -> encodes.[dstate])
//            )

//        let transitions = encodemDfa.dtran
//        let finalLexemes = List.zip encodemDfa.dfinals lexemes

//        let nextStates = 
//            transitions
//            |> DFATools.getNextStates
        
//        let lexemesFromFinal =
//            finalLexemes
//            |> DFATools.getLexemesFromFinal

        
//        let universalFinals = 
//            finalLexemes 
//            |> DFATools.getUniversalFinals
                    
//        let indeciesFromFinal =
//            finalLexemes
//            |> DFATools.getIndeciesFromFinal

//        {
//            nextStates       = nextStates
//            lexemesFromFinal = lexemesFromFinal
//            universalFinals  = universalFinals
//            indicesFromFinal = indeciesFromFinal
//        }

//    static member fromRgx(patterns:RegularExpression<'tag> list list) =
//        let nfa = AnalyzerNFA.fromRgx patterns
//        DFA.fromNFA(nfa.transition, nfa.finalLexemes)
    