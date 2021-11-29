namespace FslexFsyacc.Runtime

open FSharp.Idioms

type Parser 
    (
        productions  : Map<int,string list>,
        actions      : Map<int,Map<string,int>>,
        kernelSymbols: Map<int,string>,
        mappers      : Map<int,(obj[]->obj)>
    ) =

    ///
    member this.parse<'tok>(tokens: seq<'tok>,getTag:'tok -> string,getLexeme:'tok->obj) =
        let iterator = tokens.GetEnumerator()
        let nextElement() =
            if iterator.MoveNext() then
                Some iterator.Current
            else //EOF
                None

        let rec loop
            (trees: obj list)
            (states: int list)
            (element:'tok option) 
            =

            let sm = states.Head

            let ai = // ,remainInput
                element
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
                let ai = if ai = "" then "EOF" else ai
                failwithf "syntactic error: symbol='%s'; lookahead='%s'; state='%d'" symbol ai sm

            let action = actions.[sm].[ai]
            if action = 0 then
                trees
            elif action > 0 then
                let state = action
                let tree = getLexeme(element.Value)
                let pushedTrees = tree::trees
                let newStates = state::states //状态以及对应的票根
                loop pushedTrees newStates (nextElement())
            elif action < 0 then // 非结合性弹出错误？
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

                loop pushedTrees pushedStates element
            else failwith "never"

        loop [] [0] <| nextElement()
        |> List.exactlyOne
