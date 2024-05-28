namespace FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Idioms.Literal
open System

/// 相比BaseParser合并了一些步骤
type MoreParser<'tok> =
    {
    baseParser:BaseParser
    getNextStates:list<int*obj> -> 'tok option -> int * list<int*obj>
    }

    static member from (
        rules : (string list*(obj list->obj))list,
        actions : (string*int)list list,
        getTag : 'tok -> string,
        getLexeme : 'tok -> obj
    ) =
        let baseParser = BaseParser.create(rules, actions)
        {
        baseParser = baseParser
        getNextStates =baseParser.getNextStates(getTag,getLexeme)
        }

    /// 返回 shift token 执行完成后的状态
    member this.shift(states: list<int*obj>, token:'tok) =
        let maybeToken = Some token
        let rec loop states =
            let act, nextStates = this.getNextStates states maybeToken
            if act > 0 then
                nextStates
            elif act < 0 then
                loop nextStates
            else
                failwith "never accept in this case."
        loop states

    /// 返回最后一次reduce完成的状态，None表示未改变
    member this.tryReduce(states: list<int*obj>, token:'tok) =
        let maybeToken = Some token
        let rec loop times states =
            let act, nextStates = this.getNextStates states maybeToken
            if act < 0 then
                loop (times+1) nextStates
            elif act > 0 then
                if times > 0 then
                    Some states
                else None
            else
                failwith "never accept in this case."
        loop 0 states

    /// 返回若干次reduce完成的状态，None表示未改变
    member this.tryReduce(states: list<int*obj>) =
        let rec loop times states =
            let act, nextStates = this.getNextStates states None
            if act < 0 then
                loop (times+1) nextStates
            elif act = 0 then
                if times > 0 then
                    Some states
                else None
            else
                failwith "never shift in this case."
        loop 0 states

    [<System.Obsolete("示例用")>]
    member this.isAccept(states: list<int*obj>) =
        let act, _ = this.getNextStates states None
        act = 0

    /// 简单解析过程，一步到底
    member this.parse(tokens:seq<'tok>) =
        let iterator = Iterator tokens

        let rec loop (states: list<int*obj>) (maybeToken:'tok option) =
            Console.WriteLine($"{stringify states},{stringify maybeToken}")
            let act, nextStates = this.getNextStates states maybeToken
            if act = 0 then
                // accept
                states
            elif act < 0 then
                // reduce
                loop nextStates maybeToken
            else
                // shift next token
                iterator.tryNext()
                |> loop nextStates

        iterator.tryNext()
        |> loop [0,null] // init states
        |> List.head
        |> snd

