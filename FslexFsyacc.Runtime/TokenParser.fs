namespace FslexFsyacc.Runtime

open System
open FSharp.Idioms
open FSharp.Idioms.Literal
open FslexFsyacc.Runtime.Precedences

/// without getTag, getLexeme
type TokenParser =
    {
        rules: Map<int,string list*(obj list->obj)>
        tokens:Set<string>
        actions: Map<int,Map<string,int>>
        closures: (string list*int*string list)list list
    }

    static member create(
        rules: (string list*(obj list->obj))list,
        tokens: Set<string>,
        actions: (string*int)list list,
        closures: (int*int*string list)list list
        ) =
        let augmentProduction = fst rules.Head
        if augmentProduction.Head > "" then 
            raise <| ArgumentException("augment rule Set")

        // code -> production * reducer
        let rules: Map<int,string list*(obj list->obj)> =
            rules
            //|> List.sortBy fst
            |> List.mapi(fun i entry -> -i, entry)
            |> Map.ofList

        /// state -> lookahead -> action
        let actions:Map<int,Map<string,int>> =
            actions
            |> List.mapi(fun src pairs ->
                let mp = Map.ofList pairs
                src,mp)
            |> Map.ofList

        let closures =
            closures
            |> List.map(fun closure ->
                closure
                |> List.map(fun(prod,dot,las) ->
                    let prod = fst rules.[prod]
                    prod,dot,las
                )
            )

        {
            rules = rules
            tokens = tokens
            actions = actions
            closures = closures
        }

    ///状态的闭包
    /// for state in [0..len-1]
    member this.getClosure(state) = this.closures.[state]

    ///状态的符号：todo:符号的别名，例如 uminus
    member this.getStateSymbolPairs() =
        this.closures 
        |> List.map(fun closure ->
            match
                closure
                |> Seq.find(fun (prod,dot,_) -> 
                    // kernel
                    List.head prod = "" || dot > 0)
            with prod,dot,_ ->
                prod.[dot]
        )

    /// print state
    member this.collection() =
        let symbols = this.getStateSymbolPairs()

        this.closures
        |> List.mapi(fun i cls ->
            let symbol =
                symbols.[i]
                |> RenderUtils.renderSymbol

            let ls =
                RenderUtils.renderClosure cls
                |> Line.indentCodeBlock 4
            $"state {i} {symbol} :\r\n{ls}"
        )
        |> String.concat "\r\n"

    /// theory method: token抽象成字符串
    member this.tryNextAction(states: (int*obj)list, ai:string) =
        if ai > "" && this.tokens.Contains ai = false then
            failwith $"'{ai}' not in {stringify(List.ofSeq this.tokens)}"

        let actions = this.actions
        let sm,_ = states.Head
        if actions.ContainsKey sm && actions.[sm].ContainsKey ai then
            Some actions.[sm].[ai]
        else
            None

    member this.next<'tok>(
        getTag: 'tok -> string,
        getLexeme: 'tok->obj,
        states: (int*obj) list,
        token:'tok
        ) =

        let rules = this.rules
        let actions = this.actions

        this.tryNextAction(states, getTag token)
        |> Option.map(fun i ->
            match i with
            | _ when ParserTableAction.isStateOfShift i ->
                let pushedStates = ParserTableAction.shift(getLexeme,states,token,i)
                i,pushedStates
            | _ when ParserTableAction.isRuleOfReduce i ->
                let pushedStates = ParserTableAction.reduce(rules,actions,states,i)
                i,pushedStates
            | _ ->
                i,states
        )

    // accept, done, final
    member this.complete(states:(int*obj) list) =
        let rules = this.rules
        let actions = this.actions
        this.tryNextAction(states,"")
        |> Option.map(fun i -> // i is action's code. shift nextstate or reduce rule.
            match i with
            | _ when ParserTableAction.isStateOfShift i ->
                failwith $"no more shift."
            | _ when ParserTableAction.isRuleOfReduce i ->
                let pushedStates = ParserTableAction.reduce(rules,actions,states,i)
                i,pushedStates
            | _ ->
                i,states
        )


