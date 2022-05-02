namespace FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Literals
open FslexFsyacc.Runtime.ParseTableUtils

type Parser
    (
        rules: (string list*(obj[]->obj))[],
        actions: (string*int)[][],
        closures: (int*int*string[])[][]
    ) =

    /// action -> rule(prod,mapper)
    let rules = refineRules rules

    /// state -> lookahead -> action
    let actions = refineActions actions

    let closures = refineClosures rules closures

    ///
    member _.parse<'tok>(
        tokens:seq<'tok>,
        getTag:'tok -> string,
        getLexeme:'tok->obj
        ) =

        let iterator = Iterator(tokens.GetEnumerator())

        let rec loop
            (trees: obj list)
            (states: int list)
            (maybeToken:'tok option)
            =
            let sm = states.Head

            let ai =
                maybeToken
                |> Option.map getTag
                |> Option.defaultValue ""

            //System.Console.WriteLine($"states:{states},la:'{ai}'")

            if actions.ContainsKey sm && actions.[sm].ContainsKey ai then
                ()
            else
                let closure =
                    closures.[sm]
                    |> RenderUtils.renderClosure
                let tok =
                    match maybeToken with
                    | None -> "EOF"
                    | Some tok -> Literal.stringify tok
                failwith $"\r\nlookahead:{tok}\r\nclosure {sm}:\r\n{closure}"

            let action = actions.[sm].[ai]
            if action = 0 then
                trees
            elif action > 0 then
                let state = action
                let tree = getLexeme(maybeToken.Value)
                let pushedTrees = tree::trees
                let pushedStates = state::states
                loop pushedTrees pushedStates (iterator.tryNext())
            elif action < 0 then
                let symbols,mapper = rules.[action] //产生式符号列表。比如产生式 e-> e + e 的符号列表为 [e,e,+,e]
                let leftside = symbols.[0]
                let len = symbols.Length-1 // 产生式右侧的长度
                let children, popedTrees = List.advance len trees

                let result = mapper(Array.ofList children)
                let pushedTrees = result::popedTrees

                let pushedStates =
                    //弹出状态，产生式体
                    let popedStates = List.skip len states
                    let smr = popedStates.Head // = s_{m-r}
                    //压入状态，产生式的头
                    let newstate = actions.[smr].[leftside] // GOTO
                    newstate :: popedStates

                loop pushedTrees pushedStates maybeToken
            else failwith "never"

        iterator.tryNext()
        |> loop [] [0]
        |> List.exactlyOne

    /// 取出开头的第一个树
    member _.head<'tok>(
        tokens:seq<'tok>,
        getTag:'tok -> string,
        getLexeme:'tok->obj,
        tryFinal: (obj list) -> (int list) -> string -> option<obj list>
        ) =

        let iterator = Iterator(tokens.GetEnumerator())

        let rec loop
            (trees: obj list)
            (states: int list)
            (maybeToken:'tok option)
            =
            //当向前看时，此token已经被取得了。
            let ai =
                maybeToken
                |> Option.map getTag
                |> Option.defaultValue ""

            System.Console.WriteLine($"states:{states},la:'{ai}'")

            match tryFinal trees states ai with
            | Some trees -> trees, maybeToken.Value
            | None ->
                let sm = states.Head
                if actions.ContainsKey sm && actions.[sm].ContainsKey ai then
                    ()
                else
                    let closure = closures.[sm]
                    let closure = RenderUtils.renderClosure closure
                    let tok =
                        if maybeToken.IsNone then "EOF"
                        else Literal.stringify maybeToken.Value
                    failwith $"\r\nlookahead:{tok}\r\nclosure {sm}:\r\n{closure}"

                let action = actions.[sm].[ai]
                if action = 0 then
                    failwith $"{trees}"
                elif action > 0 then
                    let state = action
                    let tree = getLexeme(maybeToken.Value)
                    let pushedTrees = tree::trees
                    let newStates = state::states
                    loop pushedTrees newStates (iterator.tryNext())
                elif action < 0 then
                    let symbols,mapper = rules.[action] //产生式符号列表。比如产生式 e-> e + e 的符号列表为 [e,e,+,e]
                    let leftside = symbols.[0]
                    let len = symbols.Length-1 // 产生式右侧的长度
                    let children, popedTrees = List.advance len trees

                    let result = mapper(Array.ofList children)
                    let pushedTrees = result::popedTrees

                    let pushedStates =
                        //弹出状态，产生式体
                        let popedStates = List.skip len states
                        let smr = popedStates.Head // = s_{m-r}
                        //压入状态，产生式的头
                        let newstate = actions.[smr].[leftside] // GOTO
                        newstate :: popedStates

                    loop pushedTrees pushedStates maybeToken
                else failwith "never"

        iterator.tryNext()
        |> loop [] [0]
