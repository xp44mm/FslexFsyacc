namespace FslexFsyacc.Runtime.Lex

///
type DFA<'tag when 'tag: comparison> =
    {
        /// DFA nextState(s,a)
        nextStates:Map<uint32,Map<'tag,uint32>>

        ///每个正则表达式对应的接受狀態：finals，实际取词状态：lexemes
        finalLexemes:(Set<uint32>*Set<uint32>)list
    }
