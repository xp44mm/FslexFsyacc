module FslexFsyacc.Expr.ExprCompiler

open System.IO
open System.Text

open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc

open FslexFsyacc.Expr.ExprParseTable

let parser = app.getParser< PositionWith<ExprToken> >(ExprToken.getTag,ExprToken.getLexeme)
let table = app.getTable parser

let parse(tokens:seq<PositionWith<ExprToken>>) =
    tokens
    |> parser.parse
    |> ExprParseTable.unboxRoot

//let compile (txt:string) =
//    txt
//    |> ExprToken.tokenize 0
//    |> parse

let compile (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]

    SourceText.just(0, txt)
    |> ExprToken.tokenize
    |> Seq.map(fun tok ->
        tokens <- tok::tokens
        tok
    )
    |> Seq.iter(fun tok ->
        // 若干步reduce
        match parser.tryReduce(states,tok) with
        | None -> ()
        | Some x -> states <- x

        //正常流程一定是shift
        states <- parser.shift(states,tok)
    )

    match parser.tryReduce(states) with
    | None -> ()
    | Some x -> states <- x

    match states with
    |[1,lxm; 0,null] ->
        ExprParseTable.unboxRoot lxm
    | _ ->
        failwith $"{stringify states}"
