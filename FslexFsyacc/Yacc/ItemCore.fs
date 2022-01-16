namespace FslexFsyacc.Yacc

open System.Collections.Concurrent
open System

module ItemCoreMemoiz =
    let status = ConcurrentDictionary<string list * int, string list*string list>(HashIdentity.Structural)

// Simple LR 的
type ItemCore =
    {
        production: string list
        dot: int
    }

    member this.leftside = this.production |> List.head
    member this.body = this.production |> List.tail
    /// 产生式体的长度
    member this.length = this.body |> List.length

    ///前进一半，留一半
    member this.status =
        let valueFactory(production,dot) =
            let body = List.tail production
            let rec loop i rev (ls: _ list) =
                if i = dot then
                    rev,ls
                else
                    loop (i+1) (ls.Head::rev) ls.Tail
            loop 0 [] body

        ItemCoreMemoiz.status.GetOrAdd((this.production,this.dot),valueFactory)


    /// 点号紧左侧的符号，终结符，或者非终结符
    /// 可以删除吗？
    member this.prevSymbol = fst this.status |> List.head

    /// 点号右侧的产生式体的切片
    member this.rest = snd this.status

    /// 点号在最右，所有符号之后
    member this.dotmax = List.isEmpty this.rest

    /// 点号紧右侧的符号，终结符，或者非终结符
    member this.nextSymbol = snd this.status |> List.head

    // 非终结符B右侧的串，A -> alpha @ B beta 。来源于4.7.2
    member this.beta = snd this.status |> List.tail

    /// 点号前进一个符号
    member this.dotIncr() = 
        let nextStatus (currentStatus:string list*string list) =
            let rev,ls = currentStatus
            ls.Head::rev, ls.Tail

        ItemCoreMemoiz.status.GetOrAdd(
            (this.production,this.dot+1),
            Func<_,_> (fun _ -> nextStatus this.status))
        |> ignore

        {
            production = this.production
            dot = this.dot+1
        }
