namespace FslexFsyacc.Runtime.Lex

/// DFA，他的状态是由NFA状态的集合表示
type SubsetDFA<'nstate,'a when 'nstate: comparison and 'a: comparison> =
    {
        dtran :Set<Set<'nstate>*'a*Set<'nstate>>
        allStates :Set<Set<'nstate>>
    }
