namespace FslexFsyacc.Runtime

open FSharp.Idioms

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

    member this.next<'tok>(
        getTag: 'tok -> string,
        getLexeme: 'tok->obj,
        states: (int*obj) list,
        token:'tok
        ) =

        let rules = this.rules
        let actions = this.actions

        let sm,_ = states.Head
        let ai = getTag token
        //System.Console.WriteLine($"states:{states},la:'{ai}'")
        if actions.ContainsKey sm && actions.[sm].ContainsKey ai then
            match actions.[sm].[ai] with
            | stateIndex when stateIndex > 0 ->
                ParserTableAction.shift(getLexeme,states,token,stateIndex) 
            | ruleindex when ruleindex < 0 ->
                ParserTableAction.reduce(rules,actions,states,ruleindex)
            | 0 -> Accept
            | _ -> failwith "never"
        else
            Dead(sm,ai)

    member this.complete(states: (int*obj) list) =
        let rules = this.rules
        let actions = this.actions

        let sm,_ = states.Head
        let ai = ""
        //System.Console.WriteLine($"states:{states},la:'{ai}'")

        if actions.ContainsKey sm && actions.[sm].ContainsKey ai then
            match actions.[sm].[ai] with
            | state when state > 0 ->
                failwith $"no more shift."
            | ruleindex when ruleindex < 0 ->
                ParserTableAction.reduce(rules,actions,states,ruleindex)
            | 0 -> Accept
            | _ -> failwith "never"
        else
            Dead(sm,ai)



