namespace FslexFsyacc.Runtime.Lex

/// 假设操作数的状态编号不需要重新整理
type RegularNFA<'a when 'a:comparison> = {
    transition: Set<uint32*'a option*uint32>
    minState: uint32
    maxState: uint32
}
