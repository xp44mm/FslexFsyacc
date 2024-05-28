module FslexFsyacc.Runtime.ParserTableAction

open FSharp.Idioms

let isStateOfShift (i:int) = i > 0

let isRuleOfReduce(i:int) = i < 0

let (|ToShift|ToReduce|ToAccept|) (i:int) =
    if i = 0 then
        ToAccept
    elif i > 0 then
        ToShift
    else 
        ToReduce

// states stateIndex

//getLexeme token
let shift(getLexeme,states,token,stateIndex) =
    let lexeme = getLexeme token
    let pushedStates = (stateIndex,lexeme)::states
    pushedStates

// rules actions
let reduce(
    rules: Map<int,string list*(obj list->obj)>,
    actions: Map<int,Map<string,int>>,
    states:list<int*obj>,
    ruleIndex:int
    ) =
    //产生式符号列表。比如产生式 e-> e + e 的符号列表为 [e,e,+,e]
    let symbols,reducer = rules.[ruleIndex]
    let leftside = symbols.[0]
    // 产生式右侧的长度
    let len = symbols.Length-1
    // 弹出产生式体符号对应的状态
    let children, restStates = 
        states 
        |> List.advance len 

    // 产生式头的数据
    let lexeme =
        children
        |> List.map snd
        |> reducer

    let pushedStates =
        // 剩下状态栈最顶部的状态编号
        let smr,_ = restStates.Head // = s_{m-r}
        // 根据顶部状态，产生式左侧，得到新状态
        let newstate = actions.[smr].[leftside] // GOTO
        // 压入新状态
        (newstate,lexeme) :: restStates

    pushedStates




