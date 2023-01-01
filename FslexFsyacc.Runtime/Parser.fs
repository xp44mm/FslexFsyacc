namespace FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Literals.Literal
open FslexFsyacc.Runtime.ParserTableAction

type Parser<'tok> (
        rules    : (string list*(obj list->obj))list,
        actions  : (string*int)list list,
        closures : (int*int*string list)list list,
        getTag   : 'tok -> string,
        getLexeme: 'tok->obj
    ) =

    let tbl =
        TheoryParser.create(rules, actions, closures)

    /// 将lookahead token压入状态栈中。
    member this.shift(states,token:'tok) =
        let ai = getTag token
        let rec loop states =
            match tbl.tryNextAction(states,ai) with
            | Some i when isStateOfShift i ->
                let statei = i
                let states = shift(getLexeme,states,token,statei)
                states
            | Some i when isRuleOfReduce i ->
                let rulei = i
                let states = reduce(tbl.rules,tbl.actions,states,rulei)
                loop states
            | Some i -> 
                failwith $"unexpected action:{i}."
            | None ->
                failwith $"next state is dead state.{stringify token}"

        loop states

    /// 对状态栈连续执行reduce，直到非reduce动作:shift,accept（不执行）
    /// 如果改变了states返回Some newStates，如果states保持不变，返回None
    member this.tryReduce(states,token:'tok) =
        let ai = getTag token
        let rec loop times states =
            match tbl.tryNextAction(states,ai) with
            | None ->
                failwith $"next state is dead state.{stringify token}"
            | Some i when isRuleOfReduce i ->
                let pushedStates = reduce(tbl.rules,tbl.actions,states,i)
                loop (times+1) pushedStates
            | Some i ->
                if times > 0 then
                    Some states
                else None
        loop 0 states

    /// 对状态栈执行reduce，直到非reduce动作:shift,accept（不执行）
    /// 如果改变了states返回Some newStates，如果states保持不变，返回None
    member this.tryReduce(states) =
        let rec loop times states =
            match tbl.tryNextAction(states,"") with
            | None ->
                failwith $"next state is dead state."
            | Some i when isRuleOfReduce i ->
                let pushedStates = reduce(tbl.rules,tbl.actions,states,i)
                loop (times+1) pushedStates
            | Some 0 ->
                if times > 0 then
                    Some states
                else None
            | Some i -> failwith $"unexpected action:{i}."

        loop 0 states

    /// 返回接受状态
    member this.isAccept(states) =
        match tbl.tryNextAction(states,"") with
        | None ->
            failwith $"next state is dead state."
        | Some 0 -> true
        | Some _ -> false

    member this.parse(tokens:seq<'tok>) =
        let iterator =
            tokens.GetEnumerator()
            |> Iterator

        let rec loop (states:(int*obj)list) (maybeToken:'tok option) =
            let act() =
                match maybeToken with
                | Some token ->
                    tbl.next(getTag,getLexeme,states,token)
                | None ->
                    tbl.complete(states)

            match act() with
            | Some(i,nextStates) ->
                if isStateOfShift i then
                    loop nextStates (iterator.tryNext())

                elif isRuleOfReduce i then
                    loop nextStates maybeToken

                elif i = 0 then
                    nextStates

                else failwith ""
            | None -> // fail
                let sm,_ = states.Head
                let closure =
                    tbl.closures.[sm]
                    |> RenderUtils.renderClosure

                let tok =
                    match maybeToken with
                    | None -> "EOF"
                    | Some tok -> stringify tok
                failwith $"\r\nlookahead:{tok}\r\nclosure {sm}:\r\n{closure}"

        iterator.tryNext()
        |> loop [0,null]
        |> List.head
        |> snd

    [<System.Obsolete("use ParseTable.theoryParser")>]
    member this.getParserTable() = tbl

    [<System.Obsolete("use ParseTable.theoryParser")>]
    member this.getStateSymbolPairs() = 
        tbl.getStateSymbolPairs()
        |> List.mapi Pair.ofApp
        |> Map.ofList

    /// 状态的符号
    [<System.Obsolete("use ParseTable.theoryParser")>]
    member this.getSymbol(state) = tbl.getStateSymbolPairs().[state]

