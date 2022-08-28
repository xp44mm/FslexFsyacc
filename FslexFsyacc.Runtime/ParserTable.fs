namespace FslexFsyacc.Runtime

open FslexFsyacc.Runtime.ParserTableAction
open FSharp.Idioms
open FSharp.Literals

type ParserTable =
    {
        rules: Map<int,string list*(obj list->obj)>
        actions: Map<int,Map<string,int>>
        closures: (string list*int*string list)list list
    }

    static member create(
        rules: (string list*(obj list->obj))list,
        actions: (string*int)list list,
        closures: (int*int*string list)list list
        ) =
        /// action -> rule(prod,mapper)
        let startSymbol =
            rules.[0]
            |> fst  //production
            |> List.head

        let rules: Map<int,string list*(obj list->obj)> =
            [
                yield ["";startSymbol], (fun _ -> null)
                yield! rules
            ]
            |> List.sortBy fst
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
            actions=actions
            closures=closures
        }

    ///状态的闭包
    member this.getClosure(state) =
        this.closures.[state]

    ///状态的符号
    member this.getSymbol(state) =
        //闭包的kernel
        let kernel =
            this.closures.[state]
            |> List.filter(fun(prod,dot,_)-> List.head prod = "" || dot > 0)
        kernel
        |> Seq.map(fun(prod,dot,_)->prod.[dot])
        |> Seq.head
    // print state
    member this.collection() =
        this.closures
        |> List.mapi(fun i cls ->
            let symbol =
                i
                |> this.getSymbol
                |> Literal.stringify
                //|> RenderUtils.renderSymbol

            let ls =
                RenderUtils.renderClosure cls
                |> Line.indentCodeBlock 4
            $"state {i} {symbol} :\r\n{ls}"
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

