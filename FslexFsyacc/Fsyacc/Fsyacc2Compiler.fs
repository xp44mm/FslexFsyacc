module FslexFsyacc.Fsyacc.Fsyacc2Compiler

open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc

open FSharp.Literals.Literal

let parser = Parser<Position<FsyaccToken2>>(
    Fsyacc2ParseTable.rules,
    Fsyacc2ParseTable.actions,
    Fsyacc2ParseTable.closures,

    FsyaccToken2.getTag,
    FsyaccToken2.getLexeme)

//let parse(tokens:seq<Position<FsyaccToken2>>) =
//    tokens
//    |> parser.parse
//    |> FsyaccParseTable.unboxRoot

/// 解析文本为结构化数据
let compile (input:string) =
    //let mutable tokens = []
    let mutable states = [0,null]

    input
    |> FsyaccToken2.tokenize 0
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

    //match parser.tryReduce(states) with
    //| Some x -> states <- x
    //| None -> ()

    match parser.accept states with
    | [1,lxm; 0,null] ->
        Fsyacc2ParseTable.unboxRoot lxm
    | _ ->
        failwith $"{stringify states}"

//bnf的语法检测代码


