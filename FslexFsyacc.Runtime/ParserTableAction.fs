module FslexFsyacc.Runtime.ParserTableAction
open FSharp.Idioms

//type ParserTableAction =
//    | Shift of states:(int*obj) list
//    | Reduce of states:(int*obj) list
//    | Accept of states:(int*obj) list
//    | Dead of sm:int * tag:string

let isStateOfShift (i:int) = i > 0

let isRuleOfReduce(i:int) = i < 0 

//let (|StateOfShift|RuleOfReduce|Accept|) (i:int) =
//    if i > 0 then
//        StateOfShift
//    elif i < 0 then
//        RuleOfReduce
//    else
//        Accept

let shift(getLexeme,states,token,stateIndex) =
    let tree = getLexeme token
    let pushedStates = (stateIndex,tree)::states
    pushedStates

let reduce(
    rules: Map<int,string list*(obj[]->obj)>,
    actions: Map<int,Map<string,int>>,
    states,
    ruleIndex
    ) =
    //产生式符号列表。比如产生式 e-> e + e 的符号列表为 [e,e,+,e]
    let symbols,mapper = rules.[ruleIndex]
    let leftside = symbols.[0]
    // 产生式右侧的长度
    let len = symbols.Length-1
    let children, popedStates = List.advance len states

    let tree =
        children
        |> List.map snd
        |> List.toArray
        |> mapper

    let pushedStates =
        //弹出状态，产生式体
        //let popedStates = List.skip len states
        let smr,_ = popedStates.Head // = s_{m-r}
        //压入状态，产生式的头
        let newstate = actions.[smr].[leftside] // GOTO
        (newstate,tree) :: popedStates

    pushedStates



