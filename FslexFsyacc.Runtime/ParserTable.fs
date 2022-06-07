namespace FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ParserTableAction

open FSharp.Idioms
open FSharp.Idioms.ActivePatterns

type ParserTable =
    {
        rules: Map<int,string list*(obj[]->obj)>
        actions: Map<int,Map<string,int>>
        closures: (string list*int*string[])[][]
    }

    static member create(
        rules: (string list*(obj[]->obj))[],
        actions: (string*int)[][],
        closures: (int*int*string[])[][]
        ) =
        /// action -> rule(prod,mapper)
        let startSymbol =
            rules.[0]
            |> fst  // production
            |> List.head

        let rules: Map<int,string list*(obj[]->obj)> =
            [|
                yield ["";startSymbol], (fun _ -> null)
                yield! rules
            |]
            |> Array.sortBy fst
            |> Array.mapi(fun i entry -> -i, entry)
            |> Map.ofArray
        /// state -> lookahead -> action
        let actions:Map<int,Map<string,int>> =
            actions
            |> Array.mapi(fun src pairs ->
                let mp = Map.ofArray pairs
                src,mp)
            |> Map.ofArray

        let closures =
            closures
            |> Array.map(fun closure ->
                closure
                |> Array.map(fun(prod,dot,las) ->
                    let prod = fst rules.[prod]
                    prod,dot,las
                )
            )

        {
            rules = rules
            actions=actions
            closures=closures
            //getTag = getTag
            //getLexeme = getLexeme
        }

    ///状态的闭包
    member this.getClosure(state) =
        this.closures.[state]

    ///状态的符号
    member this.getSymbol(state) =
        //闭包的kernel
        let kernel =
            this.closures.[state]
            |> Array.filter(fun(prod,dot,_)-> List.head prod = "" || dot > 0)
        kernel
        |> Seq.map(fun(prod,dot,_)->prod.[dot])
        |> Seq.head

    member this.collection() =
        this.closures
        |> Array.mapi(fun i cls ->
            let symbol =
                i
                |> this.getSymbol
                |> RenderUtils.renderSymbol
            let ls =
                RenderUtils.renderClosure cls
                |> Line.indentCodeBlock 4
            $"{i}/{symbol} :\r\n{ls}"
        )
        |> String.concat "\r\n"

    member this.tryNextAction(
        states: (int*obj) list,
        ai:string
    ) =
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

        this.tryNextAction(states,getTag token)
        |> Option.map(fun i ->
            match i with
            | _ when isStateOfShift i ->
                let pushedStates = shift(getLexeme,states,token,i)
                i,pushedStates
            | _ when isRuleOfReduce i ->
                let pushedStates = reduce(rules,actions,states,i)
                i,pushedStates
            | _ ->
                i,states
        )

    member this.complete(states:(int*obj) list) =
        let rules = this.rules
        let actions = this.actions
        this.tryNextAction(states,"")
        |> Option.map(fun i ->
            match i with
            | _ when isStateOfShift i ->
                failwith $"no more shift."
            | _ when isRuleOfReduce i ->
                let pushedStates = reduce(rules,actions,states,i)
                i,pushedStates
            | _ ->
                i,states
        )

