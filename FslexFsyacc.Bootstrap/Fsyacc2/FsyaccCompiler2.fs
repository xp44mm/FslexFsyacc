module FslexFsyacc.Fsyacc.FsyaccCompiler2

open System

open FslexFsyacc
open FslexFsyacc.Grammars

open FSharp.Idioms.Literal
open FslexFsyacc.Fsyacc

let parser = FsyaccParseTable2.app.getParser<Position<FsyaccToken2>>(
                FsyaccToken2Utils.getTag,
                FsyaccToken2Utils.getLexeme)

let tbl = FsyaccParseTable2.app.getTable parser

/// 解析文本为结构化数据
let compile (input:string) =
    let mutable tokens = []
    let mutable states = [0,null]

    input
    |> FsyaccToken2Utils.tokenize 0
    |> Seq.map(fun tok ->
        tokens <- tok::tokens
        tok
    )
    |> Seq.iter(fun postok ->
        //match parser.tryReduce(states,lookahead) with
        //| Some x -> states <- x
        //| None -> ()

        states <- parser.shift(states, postok)
    )
    match parser.tryReduce(states) with
    | None -> ()
    | Some x -> states <- x

    match states with
    | [1,lxm; 0,null] ->
        FsyaccParseTable2.unboxRoot lxm
    | _ ->
        failwith $"{stringify states}"
