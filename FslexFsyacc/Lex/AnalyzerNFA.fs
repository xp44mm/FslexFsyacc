namespace FslexFsyacc.Lex

/// analysis
type AnalyzerNFA<'a when 'a:comparison> = {
    transition:Set<uint32*'a option*uint32>
    finalLexemes:(uint32*uint32) list
}

