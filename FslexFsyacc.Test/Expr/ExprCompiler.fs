module FslexFsyacc.Expr.ExprCompiler

open System.IO
open System.Text

open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime

let parser = 
    PrimeParser<Position<ExprToken>>(
        ExprParseTable.rules,
        ExprParseTable.actions,
        ExprParseTable.closures,

        ExprToken.getTag,
        ExprToken.getLexeme)

let parse(tokens:seq<Position<ExprToken>>) =
    tokens
    |> parser.parse
    |> ExprParseTable.unboxRoot

let compile (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]

    txt
    |> ExprToken.tokenize 0
    |> Seq.map(fun tok ->
        tokens <- tok::tokens
        tok
    )
    |> Seq.iter(fun tok ->
        match parser.tryReduce(states,tok) with
        | Some x -> states <- x
        | None -> ()
        states <- parser.shift(states,tok)
    )
    match parser.tryReduce(states) with
    | Some x -> states <- x
    | None -> ()

    match states with
    |[1,lxm; 0,null] ->
        ExprParseTable.unboxRoot lxm
    | _ ->
        failwith $"{stringify states}"
