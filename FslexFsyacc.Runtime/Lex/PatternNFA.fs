namespace FslexFsyacc.Runtime.Lex

/// 从Lex文件一个匹配模式翻译过来的NFA
type PatternNFA<'a when 'a:comparison> = {
    transition:Set<uint32*'a option*uint32>
    minState:uint32
    lexemeState:uint32
    maxState:uint32
}
