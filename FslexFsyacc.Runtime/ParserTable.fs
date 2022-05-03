namespace FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Literals

type ParserTable<'tok> = 
    {
        rules: Map<int,string list*(obj[]->obj)>
        actions: Map<int,Map<string,int>>
        closures: (string list*int*string[])[][]
        getTag: 'tok -> string
        getLexeme: 'tok->obj

    }

    static member create(
        rules: (string list*(obj[]->obj))[],
        actions: (string*int)[][],        
        closures: (int*int*string[])[][],
        getTag, getLexeme
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
            getTag = getTag
            getLexeme = getLexeme
        }

    member this.execute(
        states: int list, 
        trees: obj list, 
        maybeToken:'tok option
        ) =
        let rules = this.rules
        let actions = this.actions
        let closures = this.closures
        let getTag = this.getTag
        let getLexeme = this.getLexeme

        let sm = states.Head

        let ai =
            maybeToken
            |> Option.map getTag
            |> Option.defaultValue ""

        //System.Console.WriteLine($"states:{states},la:'{ai}'")

        if actions.ContainsKey sm && actions.[sm].ContainsKey ai then
            match actions.[sm].[ai] with
            | state when state > 0 ->
                let pushedStates = state::states
                let tree = getLexeme(maybeToken.Value)
                let pushedTrees = tree::trees
                Shift(pushedStates, pushedTrees)
            | ruleindex when ruleindex < 0 ->
                //产生式符号列表。比如产生式 e-> e + e 的符号列表为 [e,e,+,e]
                let symbols,mapper = rules.[ruleindex] 
                let leftside = symbols.[0]
                // 产生式右侧的长度
                let len = symbols.Length-1 
                let children, popedTrees = List.advance len trees

                let pushedStates =
                    //弹出状态，产生式体
                    let popedStates = List.skip len states
                    let smr = popedStates.Head // = s_{m-r}
                    //压入状态，产生式的头
                    let newstate = actions.[smr].[leftside] // GOTO
                    newstate :: popedStates

                let result = mapper(Array.ofList children)
                let pushedTrees = result::popedTrees

                Reduce(pushedStates, pushedTrees)
            | 0 | _ -> Accept
        else
            Dead



