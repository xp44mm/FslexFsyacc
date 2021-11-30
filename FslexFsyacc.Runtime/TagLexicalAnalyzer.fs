namespace FslexFsyacc.Runtime

/// 3.8 Lex DFA 模擬器，解析标签
type TagLexicalAnalyzer<'tag when 'tag:comparison>
    (
        nextStates       :Map<uint32,Map<'tag,uint32>>, // state -> symbol -> nextState
        lexemesFromFinal :Map<uint32,Set<uint32>>, // final state -> lexeme state set
        universalFinals  :Set<uint32>, // all of the final state
        indeciesFromFinal:Map<uint32,int> // final state -> index
    ) =
    //final状态是包括向前看的最长状态。
    //lexeme状态是回退后最终匹配的较短状态。
    let tryNextState state elem =
        if nextStates.ContainsKey(state) && nextStates.[state].ContainsKey(elem) then
            let nextState = nextStates.[state].[elem]
            Some nextState
        else None //死狀態 
        //todo:当死状态时，默认前进一个token，索引号为dfinalLexemes.Length

    /// 從臨死狀態(死狀態的前一個狀態)回溯到lexeme狀態
    let retract (context: StateContext<uint32,'tag>) =
        //回溯查找接受狀態
        let finalContext = context.backword universalFinals
        //回溯從向前看接受狀態到詞素接受狀態
        let lexemeContext = 
            if lexemesFromFinal.ContainsKey finalContext.state then
                finalContext.backword lexemesFromFinal.[finalContext.state]
            else
                finalContext
        let lexeme = lexemeContext.getLexeme()
        (finalContext.state, lexeme), lexemeContext.buffer

    ///枚举器中的所有内容已经读入到缓存中了, canMoveNext = false，一旦变假，就不会恢复真了。
    let lastForward (paths:(uint32*'tag)list) state (buffer:'tag list) =
        let rec loop paths state buffer =
            match buffer with
            | [] ->
                paths,state,buffer
            | elem::tail ->
                match tryNextState state elem with
                | Some nextState ->
                    loop ((state,elem)::paths) nextState tail
                | None ->
                    paths,state,buffer

        let paths,state,buffer = loop paths state buffer

        {
            paths = paths
            state = state
            buffer = buffer
            canMoveNext = false
        }

    ///前進到死狀態，在經歷過的狀態中，查找最后面的接受狀態，進一步確定詞素狀態。
    ///截取詞素前綴后，剩餘的輸入序列作爲輸入，重新開始迭代，直到耗盡序列。
    member this.simulate<'tag> (inp:seq<'tag>) =
        let iterator = inp.GetEnumerator()

        let nextTag() =
            if iterator.MoveNext() then
                Some iterator.Current
            else //EOF
                None

        // 枚举器中向前进，buffer = []
        let enumeratorForward (paths:(uint32*'tag)list) state (elem:'tag option) =
            let rec loop paths state elem =
                match elem with
                | None -> //EOF
                    paths,state,false
                | Some elem ->
                    match tryNextState state elem with
                    | Some nextState ->
                        nextTag()
                        |> loop ((state,elem)::paths) nextState
                    | None ->
                        paths,state,true

            let paths,state,canMoveNext = loop paths state elem
            {
                paths = paths
                state = state
                buffer = [] //枚举器前进时，缓存一定是空
                canMoveNext = canMoveNext
            }

        // 缓存中向前，canMoveNext = true
        let rec bufferForward (paths:(uint32*'tag)list) state (buffer:'tag list) =
            match buffer with
            | [] ->
                Some iterator.Current
                |> enumeratorForward paths state
            | elem :: tail ->
                match tryNextState state elem with
                | Some nextState ->
                    bufferForward ((state,elem)::paths) nextState tail
                | None -> // 臨死狀態：死狀態的前一個狀態
                    {
                        paths = paths
                        state = state
                        buffer = buffer
                        canMoveNext = true // 一定是true, 为false时在lastForward中
                    }

        /// 产生tokens序列的循环
        let rec getPairs (context:StateContext<uint32, 'tag>) =
            let deadContext = 
                {
                    paths = []
                    state = 0u
                    buffer = []
                    canMoveNext = false
                    }

            seq {
                if context = deadContext then
                    ()// 不撞南墻不回頭，不見棺材不落淚，不見兔子不撒鷹
                else
                    let (finalState, lexeme), buffer =
                        try
                            retract context
                        with _ -> failwithf "lex retract: %A" context

                    yield indeciesFromFinal.[finalState], lexeme

                    let nextContext =
                        if context.canMoveNext then
                            bufferForward [] 0u buffer
                        else
                            lastForward [] 0u buffer
                    yield! getPairs nextContext

            }

        //返回匹配模式的索引（基於0），和讀取的詞素。
        nextTag()
        |> enumeratorForward [] 0u // paths, state
        |> getPairs // tokens

