module FslexFsyacc.Expr.Expr2Compiler

open System.IO
open System.Text

open FSharp.Idioms
open FSharp.Literals.Literal

open FslexFsyacc.Runtime
open Expr

let parser = 
    Parser<Position<ExprToken>>(
        Expr2ParseTable.rules,
        Expr2ParseTable.actions,
        Expr2ParseTable.closures,

        ExprToken.getTag,
        ExprToken.getLexeme)

let parse(tokens:seq<Position<ExprToken>>) =
    tokens
    |> parser.parse
    |> Expr2ParseTable.unboxRoot

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
        Expr2ParseTable.unboxRoot lxm
    | _ ->
        failwith $"{stringify states}"
