namespace FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Literals

type Parser 
    (
        productions  : (int*string[])[], 
        actions      : (int*(string*int)[])[],
        kernelSymbols: (int*string)[],
        mappers      : (int*(obj[]->obj))[]
    ) =
    /// action(reduce) -> production
    let productions =
        let t = new System.Collections.Generic.Dictionary<_,_>(HashIdentity.Structural)
        for k,v in productions do t.Add(k,v)
        t
    /// state -> lookahead -> action
    let actions =
        let t = new System.Collections.Generic.Dictionary<_,_>(HashIdentity.Structural)
        for k,v in actions do 
            let tt = new System.Collections.Generic.Dictionary<_,_>(HashIdentity.Structural)
            for kk,vv in v do tt.Add(kk,vv)
            t.Add(k,tt)
        t
    /// state -> symbol
    let kernelSymbols =
        let t = new System.Collections.Generic.Dictionary<_,_>(HashIdentity.Structural)
        for k,v in kernelSymbols do t.Add(k,v)
        t
    /// action(reduce) -> mapper
    let mappers =
        let t = new System.Collections.Generic.Dictionary<_,_>(HashIdentity.Structural)
        for k,v in mappers do t.Add(k,v)
        t

    ///
    member _.parse<'tok>(tokens: seq<'tok>, getTag:'tok -> string, getLexeme:'tok->obj) =
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
                let symbol =
                    if sm = 0 then
                        "<undefined>"
                    else
                        kernelSymbols.[sm]
                let la = if maybeToken.IsNone then "EOF" else Literal.stringify maybeToken.Value
                failwithf "syntactic error: lookahead='%s' symbol='%s' state='%d'" la symbol sm

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
                    let newstate = actions.[smr].[leftside]
                    newstate :: popedStates

                loop pushedTrees pushedStates maybeToken
            else failwith "never"

        loop [] [0] <| iterator.tryNext()
        |> List.exactlyOne
