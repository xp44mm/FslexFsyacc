module FslexFsyacc.Fsyacc.FsyaccCompiler

open System

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.Grammars

open FSharp.Idioms.Literal
open FslexFsyacc.Fsyacc

let parser = FsyaccParseTable1.getParser<Position<FsyaccToken>>
                FsyaccTokenUtils.getTag
                FsyaccTokenUtils.getLexeme

let grammar = 
    FsyaccParseTable1.rules
    |> List.map fst
    |> Set.ofList
    |> Grammar.just

/// 解析文本为结构化数据
let compile (input:string) =
    let mutable tokens = []
    let mutable states = [0,null]

    input
    |> FsyaccTokenUtils.tokenize 0

    |> Seq.map(fun tok ->
        tokens <- tok::tokens
        let tag = FsyaccTokenUtils.getTag tok
        if grammar.terminals.Contains tag = false then
            failwith $"{tag} not in {grammar.terminals}"
        tok
    )
    |> Seq.iter(fun postok ->
        //match parser.tryReduce(states,lookahead) with
        //| Some x -> states <- x
        //| None -> ()

        states <- parser.shift(states, postok)
    )

    match parser.accept states with
    | [1,lxm; 0,null] ->
        FsyaccParseTable1.unboxRoot lxm
    | _ ->
        failwith $"{stringify states}"
