module FslexFsyacc.Lex.DFATools

open FSharp.Idioms

/// DFA nextState(s,a)
let getNextStates (transitions:Set<uint32*'tag*uint32>)=
    let result:Map<uint32,Map<'tag,uint32>> = 
        Set.toUniqueJaggedMap transitions
    result
