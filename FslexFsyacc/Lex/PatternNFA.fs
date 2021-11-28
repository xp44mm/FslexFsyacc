namespace FslexFsyacc.Lex

/// 从Lex文件一个匹配模式翻译过来的NFA
type PatternNFA<'a when 'a:comparison> =
    {
        transition:Set<uint32*'a option*uint32>
        minState:uint32
        lexemeState:uint32
        maxState:uint32
    }
    /// s/t: an s but only if it is followed by an t. The t is not part of the matched text.
    static member lookahead(s:RegularNFA<'a>, trailingContext:RegularNFA<'a>) =
        {
            transition = 
                s.transition + trailingContext.transition
                |> Set.add(s.maxState, None, trailingContext.minState)
            minState = s.minState
            lexemeState = s.maxState
            maxState = trailingContext.maxState
        }
        
    static member sole(s:RegularNFA<'a>) =
        {
            transition = s.transition
            minState = s.minState
            lexemeState = s.maxState
            maxState = s.maxState
        }

    static member fromRgx(i:uint32, pattern:RegularExpression<'a> list) =
        match pattern with
        | [rgx] ->
            let n1 = RegularExpressionNFA.convertToNFA i rgx
            PatternNFA.sole(n1)
        | [rgx;trailling] ->
            let n1 = RegularExpressionNFA.convertToNFA i rgx
            let n2 = RegularExpressionNFA.convertToNFA (n1.maxState+1u) trailling
            PatternNFA.lookahead(n1, n2)
        | ls -> failwithf "never:%d" ls.Length
