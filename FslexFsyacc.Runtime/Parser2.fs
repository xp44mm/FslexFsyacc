namespace FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Literals

type Parser2 
    (
        productions: (string list)[],
        closures   : (int*int*string[])[][],
        actions    : (string*int)[][],
        mappers    : (obj[]->obj)[]
    ) =
    /// action(reduce) -> production
    let productions =
        productions
        |> Array.mapi(fun i prod -> -i,prod)
        |> Map.ofArray

    let closures =
        closures
        |> Array.map(fun closure ->
            closure
            |> Array.map(fun(prod,dot,las)->
                productions.[prod],dot,las
            )
        )

    /// state -> lookahead -> action
    let actions =
        actions
        |> Array.mapi(fun src pairs -> 
            let mp = Map.ofArray pairs
            src,mp)
        |> Map.ofArray

    let mappers =
        mappers
        |> Array.mapi(fun i mpr -> 
            let iprod = -(i+1)
            iprod,mpr
        )
        |> Map.ofArray
    ///
    member _.parse<'tok>(tokens:seq<'tok>, getTag:'tok -> string, getLexeme:'tok->obj) =
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

            if actions.ContainsKey sm && actions.[sm].ContainsKey ai then
                ()
            else
                let closure = Utils.renderClosure closures.[sm]
                let tok =
                    if maybeToken.IsNone then "EOF"
                    else Literal.stringify maybeToken.Value
                failwithf "lookahead:%s\r\nstate:\r\n%s" tok closure

            let action = actions.[sm].[ai]
            if action = 0 then
                trees
            elif action > 0 then
                let state = action
                let tree = getLexeme(maybeToken.Value)
                let pushedTrees = tree::trees
                let newStates = state::states
                loop pushedTrees newStates (iterator.tryNext())
            elif action < 0 then
                let symbols = productions.[action] //产生式符号列表。比如产生式 e-> e + e 的列表为 [e,e,+,e]
                let leftside = symbols.[0]
                let len = symbols.Length-1 // 产生式右侧的长度
                let children, popedTrees = List.advance len trees

                let mapper = mappers.[action]
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

        loop [] [0] <| iterator.tryNext()
        |> List.exactlyOne
