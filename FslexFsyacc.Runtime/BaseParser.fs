namespace FslexFsyacc.Runtime

open System
open FSharp.Idioms
open FSharp.Idioms.Literal

/// without getTag, getLexeme
type BaseParser =
    {
        rules: Map<int,string list*(obj list->obj)>
        actions: Map<int,Map<string,int>>
    }

    static member create(
        rules: (string list*(obj list->obj))list,
        actions: (string*int)list list
        ) =
        let augmentProduction = fst rules.Head
        if augmentProduction.Head > "" then
            raise <| ArgumentException("augment rule Set")

        // code -> production * reducer
        let rules =
            rules
            |> List.mapi(fun i entry -> -i, entry)
            |> Map.ofList

        /// state -> lookahead -> action
        let actions =
            actions
            |> List.mapi(fun src pairs ->
                let mp = Map.ofList pairs
                src,mp)
            |> Map.ofList

        {
            rules = rules
            actions = actions
        }

    member this.getNextState<'tok>(getTag: 'tok -> string, getLexeme: 'tok -> obj) =
        let rules = this.rules
        let actions = this.actions
        fun (states: list<int*obj>) (maybeToken: 'tok option) ->
            let sm = fst states.Head

            if not(actions.ContainsKey sm) then NoSource sm else

            let lookaheads = actions.[sm]
            let ai, lexeme =
                match maybeToken with
                | None -> "",null
                | Some token ->
                    let ai = getTag token
                    let lexeme = getLexeme token
                    ai,lexeme

            if not(lookaheads.ContainsKey ai) then NoSymbol ai else

            let i = lookaheads.[ai]
            if i = 0 then
                if ai = "" then
                    Accepted
                else
                    NoZero ai
            elif i > 0 then
                Shifted((i,lexeme) :: states)
            else
                //产生式符号列表。比如产生式 e-> e + e 的符号列表为 [e;e;+;e]
                let symbols,reducer = rules.[i]
                let leftside = symbols.[0]
                // 产生式右侧的长度
                let len = symbols.Length-1
                // 弹出产生式体符号对应的状态
                let children, restStates =
                    states
                    |> List.advance len

                // 产生式头的数据
                let lexeme =
                    children
                    |> List.map snd
                    |> reducer

                // 剩下状态栈最顶部的状态编号
                let smr = fst restStates.Head // = s_{m-r}
                // 根据顶部状态，产生式左侧，得到新状态
                let newstate = actions.[smr].[leftside] // GOTO
                // 压入新状态
                Reduced((newstate,lexeme) :: restStates)


