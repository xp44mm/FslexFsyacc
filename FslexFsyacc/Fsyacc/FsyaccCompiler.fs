module FslexFsyacc.Fsyacc.FsyaccCompiler
open System

open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc

open FSharp.Idioms.Literal

let parser = Parser<Position<FsyaccToken>>(
    FsyaccParseTable.rules,
    FsyaccParseTable.actions,
    FsyaccParseTable.closures,

    FsyaccTokenUtils.getTag,
    FsyaccTokenUtils.getLexeme)

/// 解析文本为结构化数据
let compile (input:string) =
    //let mutable tokens = []
    let mutable states = [0,null]

    input
    |> FsyaccTokenUtils.tokenize 0
    //|> Seq.map(fun tok ->
    //    tokens <- tok::tokens
    //    tok
    //)
    |> Seq.iter(fun postok ->
        //match parser.tryReduce(states,lookahead) with
        //| Some x -> states <- x
        //| None -> ()

        states <- parser.shift(states, postok)
    )

    match parser.accept states with
    | [1,lxm; 0,null] ->
        FsyaccParseTable.unboxRoot lxm
    | _ ->
        failwith $"{stringify states}"
