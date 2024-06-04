namespace FslexFsyacc

open FSharp.Idioms
open FSharp.Idioms.Literal
open System

/// 相比BaseParser合并了一些步骤
type MoreParser<'tok> =
    {
    baseParser: BaseParser
    getNextState: list<int*obj> -> 'tok option -> NextState
    }

    static member from (
        rules: (string list*(obj list->obj))list,
        actions: (string*int)list list,
        getTag: 'tok -> string,
        getLexeme: 'tok -> obj
    ) =
        let baseParser = BaseParser.create(rules, actions)
        {
        baseParser = baseParser
        getNextState = baseParser.getNextState(getTag,getLexeme)
        }

    /// 返回 shift token 执行完成后的状态
    member this.shift(states: list<int*obj>, token: 'tok) =
        let rec loop states =
            match this.getNextState states (Some token) with
            | Shifted nextStates -> nextStates
            | Reduced nextStates -> loop nextStates
            | x -> failwith (stringify x)
        loop states

    /// 返回最后一次reduce完成的状态，None表示未改变
    member this.tryReduce(states: list<int*obj>, token:'tok) =
        let rec loop times states =
            match this.getNextState states (Some token) with
            | Reduced nextStates -> loop (times+1) nextStates
            | Shifted _
            //| Accepted nextStates
                ->
                if times > 0 then
                    Some states
                else None
            | x -> failwith (stringify x)
        loop 0 states

    /// 返回若干次reduce完成的状态，None表示未改变
    member this.tryReduce(states: list<int*obj>) =
        let rec loop times states =
            match this.getNextState states None with
            | Reduced nextStates -> loop (times+1) nextStates
            | Accepted ->
                if times > 0 then
                    Some states
                else None
            | x -> failwith (stringify x)

        loop 0 states

    member this.tryAccept(states: list<int*obj>) =
        let rec loop states =
            match this.getNextState states None with
            | Reduced nextStates -> loop nextStates
            | Accepted ->
                match states with
                | [1,lxm; 0,null] ->
                    Some lxm
                | _ ->
                    None
            | _ -> None
        loop states

    /// 简单解析过程，一步到底
    member this.parse(tokens:seq<'tok>) =
        let iterator = Iterator tokens

        let rec loop (states: list<int*obj>) (maybeToken:'tok option) =
            //Console.WriteLine($"{stringify states},{stringify maybeToken}")
            match this.getNextState states maybeToken with
            | Accepted -> states
            | Reduced nextStates -> loop nextStates maybeToken
            | Shifted nextStates ->
                iterator.tryNext()
                |> loop nextStates
            | x -> failwith (stringify x)

        iterator.tryNext()
        |> loop [0,null] // init states
        |> List.head
        |> snd

