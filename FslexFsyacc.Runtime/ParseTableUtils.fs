[<System.Obsolete("ParserTable()")>]
module FslexFsyacc.Runtime.ParseTableUtils

open FSharp.Idioms
open FSharp.Literals

/// action -> rule(prod,mapper)
let refineRules (rules: (string list*(obj[]->obj))[]) =
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
    rules
/// state -> lookahead -> action
let refineActions (actions: (string*int)[][]) =
    let actions:Map<int,Map<string,int>> =
        actions
        |> Array.mapi(fun src pairs ->
            let mp = Map.ofArray pairs
            src,mp)
        |> Map.ofArray
    actions

let refineClosures 
    (rules: Map<int,string list*(obj[]->obj)>)
    (closures: (int*int*string[])[][]) 
    =
    closures
    |> Array.map(fun closure ->
        closure
        |> Array.map(fun(prod,dot,las) ->
            let prod = fst rules.[prod]
            prod,dot,las
        )
    )

let currentConfig<'tok>
    (closures:(string list*int*string[])[][]) 
    (sm:int) 
    (maybeToken: 'tok option) 
    =
    let closure = closures.[sm]
    let closure = RenderUtils.renderClosure closure
    let tok =
        match maybeToken with
        | None -> "EOF"
        | Some tok -> Literal.stringify tok
    $"\r\nlookahead:{tok}\r\nclosure {sm}:\r\n{closure}"

let shift 
    (getLexeme:'tok->obj)
    (trees:obj list)
    (states:int list)
    (state:int)
    (maybeToken: _ option)
    =
    let tree = getLexeme(maybeToken.Value)
    let pushedTrees = tree::trees
    let pushedStates = state::states
    pushedTrees, pushedStates

let reduce 
    (rules:Map<int,string list*(obj[]->obj)>)
    (actions:Map<int,Map<string,int>>)
    (trees:obj list)
    (states:int list)
    (ruleindex:int)
    =
    let symbols,mapper = rules.[ruleindex] //产生式符号列表。比如产生式 e-> e + e 的符号列表为 [e,e,+,e]
    let leftside = symbols.[0]
    let len = symbols.Length-1 // 产生式右侧的长度
    let children, popedTrees = List.advance len trees

    let result = mapper(Array.ofList children)
    let pushedTrees = result::popedTrees

    let pushedStates =
        //弹出状态，产生式体
        let popedStates = List.skip len states
        let smr = popedStates.Head // = s_{m-r}
        //压入状态，产生式的头：GOTO new state
        let newstate = actions.[smr].[leftside]
        newstate :: popedStates

    pushedTrees, pushedStates

