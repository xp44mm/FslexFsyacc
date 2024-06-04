namespace FslexFsyacc.Runtime.Lex

/// Figure 3.31: Operations on NFA states
type NFAOperations<'state,'a when 'state:comparison and 'a:comparison> = {
    moves:Map<'state,Map<'a,Set<'state>>>
    closures:Map<'state,Set<'state>>
}

