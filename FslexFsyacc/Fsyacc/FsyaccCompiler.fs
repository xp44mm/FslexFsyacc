module FslexFsyacc.Fsyacc.FsyaccCompiler

open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc

open System.Reactive
open System.Reactive.Linq

open FSharp.Literals.Literal

let parser = Parser<Position<FsyaccToken>>(
    Fsyacc2ParseTable.rules,
    Fsyacc2ParseTable.actions,
    Fsyacc2ParseTable.closures,
    FsyaccTokenUtils.getTag,
    FsyaccTokenUtils.getLexeme)

let parse(tokens:seq<Position<FsyaccToken>>) =
    tokens
    |> parser.parse
    |> Fsyacc2ParseTable.unboxRoot

//let compile (inp:string) =
//    inp
//    |> FsyaccTokenUtils.tokenize 0
//    |> parse


/// 解析文本为结构化数据
let compile (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]
    let mutable result = defaultValue<_>
    let sq =
        txt
        |> FsyaccTokenUtils.tokenize 0
        |> Seq.map(fun tok ->
            tokens <- tok::tokens
            tok
        )
        |> Seq.map(fun lookahead ->
            match parser.tryReduce(states,lookahead) with
            | Some x -> states <- x
            | None -> ()

            states <- parser.shift(states,lookahead)
        )
    use _ = sq.Subscribe(Observer.Create(
        (fun () -> ()),
        (fun () ->
            match parser.tryReduce(states) with
            | Some x -> states <- x
            | None -> ()

            match states with
            |[1,lxm; 0,null] ->
                result <- Fsyacc2ParseTable.unboxRoot lxm
            | _ ->
                failwith $"{stringify states}"
        )
    ))

    result

//bnf的语法检测代码


