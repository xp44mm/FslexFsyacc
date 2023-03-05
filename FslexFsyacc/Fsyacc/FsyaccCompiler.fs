module FslexFsyacc.Fsyacc.FsyaccCompiler

open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc

open FSharp.Literals.Literal

let parser = Parser<Position<FsyaccToken>>(
    FsyaccParseTable.rules,
    FsyaccParseTable.actions,
    FsyaccParseTable.closures,
    FsyaccTokenUtils.getTag,
    FsyaccTokenUtils.getLexeme)

let parse(tokens:seq<Position<FsyaccToken>>) =
    tokens
    |> parser.parse
    |> FsyaccParseTable.unboxRoot

/// 解析文本为结构化数据
let compile (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]
    //let mutable result = defaultValue<_>
    txt
    |> FsyaccTokenUtils.tokenize 0
    |> Seq.map(fun tok ->
        tokens <- tok::tokens
        tok
    )
    |> Seq.iter(fun lookahead ->
        //match parser.tryReduce(states,lookahead) with
        //| Some x -> states <- x
        //| None -> ()

        states <- parser.shift(states,lookahead)
    )

    //match parser.tryReduce(states) with
    //| Some x -> states <- x
    //| None -> ()

    match parser.accept states with
    | [1,lxm; 0,null] ->
        FsyaccParseTable.unboxRoot lxm
    | _ ->
        failwith $"{stringify states}"

//bnf的语法检测代码


