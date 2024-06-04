namespace FslexFsyacc.Runtime.Lex

/// DFA，他的状态是由NFA状态的集合表示
type PartitionDFA<'dstate,'a when 'dstate: comparison and 'a: comparison> = {
    dtran:Set<'dstate*'a*'dstate>
    allStates:Set<'dstate>
    dfinals:Set<'dstate> list
}
