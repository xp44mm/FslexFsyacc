namespace FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Literals
open System.Collections.Generic

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



    //member this.next(
    //    states: Stack<int>,
    //    trees: Stack<obj>,
    //    token:'tok
    //    ) =

    //    let rules = this.rules
    //    let actions = this.actions
    //    let closures = this.closures
    //    let getTag = this.getTag
    //    let getLexeme = this.getLexeme

    //    let sm = states.Peek()
    //    let ai = getTag token

    //    //System.Console.WriteLine($"states:{states},la:'{ai}'")

    //    if actions.ContainsKey sm && actions.[sm].ContainsKey ai then
    //        match actions.[sm].[ai] with
    //        | state when state > 0 ->
    //            //let pushedStates = state::states
    //            states.Push(state)
    //            let tree = getLexeme token
    //            //let pushedTrees = tree::trees
    //            trees.Push(tree)
    //            Shift([],[])
    //        | ruleindex when ruleindex < 0 ->
    //            //产生式符号列表。比如产生式 e-> e + e 的符号列表为 [e,e,+,e]
    //            let symbols,mapper = rules.[ruleindex]
    //            let leftside = symbols.[0]
    //            // 产生式右侧的长度
    //            let len = symbols.Length-1

    //            // 从栈顶弹出产生式体
    //            let rec getChildren (ls:obj list) i =
    //                if i = 0 then
    //                    ls
    //                else
    //                    let s = trees.Pop()
    //                    states.Pop() |> ignore
    //                    getChildren (s::ls) (i-1)

    //            let children = getChildren [] len

    //            //弹出状态，产生式体
    //            //let popedStates = List.skip len states

    //            let smr = states.Peek() // = s_{m-r}
    //            let newstate = actions.[smr].[leftside] // GOTO

    //            //压入状态，产生式的头
    //            states.Push(newstate)

    //            let result = mapper(Array.ofList children)
    //            trees.Push(result)

    //            //Reduce(pushedStates, pushedTrees)
    //            this.next(states,trees,token)
    //        | 0 | _ -> Accept
    //    else
    //        Dead(sm,ai)

    //member this.complete(states: Stack<int>,trees: Stack<obj>) =

    //    let rules = this.rules
    //    let actions = this.actions
    //    let closures = this.closures
    //    let getTag = this.getTag
    //    let getLexeme = this.getLexeme

    //    let sm = states.Peek()
    //    let ai = ""

    //    //System.Console.WriteLine($"states:{states},la:'{ai}'")

    //    if actions.ContainsKey sm && actions.[sm].ContainsKey ai then
    //        match actions.[sm].[ai] with
    //        | state when state > 0 ->
    //            failwith $"{state}"
    //        | ruleindex when ruleindex < 0 ->
    //            //产生式符号列表。比如产生式 e-> e + e 的符号列表为 [e,e,+,e]
    //            let symbols,mapper = rules.[ruleindex]
    //            let leftside = symbols.[0]
    //            // 产生式右侧的长度
    //            let len = symbols.Length-1

    //            // 从栈顶弹出产生式体
    //            let rec getChildren (ls:obj list) i =
    //                if i = 0 then
    //                    ls
    //                else
    //                    let s = trees.Pop()
    //                    states.Pop() |> ignore
    //                    getChildren (s::ls) (i-1)

    //            let children = getChildren [] len

    //            //弹出状态，产生式体
    //            //let popedStates = List.skip len states

    //            let smr = states.Peek() // = s_{m-r}
    //            let newstate = actions.[smr].[leftside] // GOTO

    //            //压入状态，产生式的头
    //            states.Push(newstate)

    //            let result = mapper(Array.ofList children)
    //            trees.Push(result)

    //            //Reduce(pushedStates, pushedTrees)
    //            this.complete(states,trees)
    //        | 0 | _ -> Accept
    //    else
    //        Dead(sm,ai)
