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

    member this.getNextStates(getTag: 'tok -> string, getLexeme: 'tok -> obj) =
        let rules = this.rules
        let actions = this.actions
        fun (states: list<int*obj>) (maybeToken: 'tok option) ->
            let sm = fst states.Head
            let lookaheads =
                if actions.ContainsKey sm then
                    actions.[sm]
                else
                    failwith $"栈内未知状态"

            let ai, lexeme =
                match maybeToken with
                | None -> "",null
                | Some token ->
                    let ai = getTag token
                    let lexeme = getLexeme token
                    ai,lexeme

            if not(lookaheads.ContainsKey ai) then
                let states = states |> List.map fst
                failwith $"未知的向前看符号:{ai},{stringify states}"

            let i = lookaheads.[ai]
            Console.WriteLine($"{i}")

            if i = 0 then
                if ai = "" then
                    states // 接受最后一步不压入栈
                else
                    failwith "应该不是接受状态"

            elif i > 0 then //shift
                (i,lexeme) :: states
            else // reduce
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
                (newstate,lexeme) :: restStates
            |> Pair.prepend i


