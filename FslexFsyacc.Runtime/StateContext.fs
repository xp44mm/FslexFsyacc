namespace FslexFsyacc.Runtime

/// 表示词法分析器当前状态的所有属性
type StateContext<'state, 'tag when 'state:comparison> =
    {
        /// 状态后面的字符和字符之后的状态
        paths  : ('state*'tag) list
        /// 当前状态
        state  : 'state
        /// 已经读取的字符放到缓存中
        buffer : 'tag list
        /// 保存IEnumerator.MoveNext()方法的返回值，表示是否可以继续被调用。
        canMoveNext: bool
    }

    /// 回退到第一个位于输入集合中的状态
    member context.backword (states:Set<'state>) =
        //找到接受狀態
        if states.Contains context.state then
            context
        else
            match context.paths with
            | (prevState,elem) :: tail ->
                {
                    paths = tail
                    state = prevState
                    buffer = elem::context.buffer
                    canMoveNext = context.canMoveNext
                }.backword(states)
            | [] ->
                failwithf "backward no found %A in %A" context states

    /// 提取词素
    member context.getLexeme() = context.paths |> List.map snd |> List.rev

