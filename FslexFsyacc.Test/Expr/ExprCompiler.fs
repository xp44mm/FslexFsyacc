module Expr.ExprCompiler

open System.IO
open System.Text
open System.Reactive
open System.Reactive.Linq

open FSharp.Literals.Literal
open FSharp.Idioms

open FslexFsyacc.Runtime
open Expr

let parser = 
    Parser<Position<ExprToken>>(
        ExprParseTable.rules,
        ExprParseTable.actions,
        ExprParseTable.closures,ExprToken.getTag,ExprToken.getLexeme)

let parse(tokens:seq<Position<ExprToken>>) =
    tokens
    |> parser.parse
    |> ExprParseTable.unboxRoot

let compile (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]
    let mutable result = defaultValue<float>

    let sq =
        txt
        |> ExprToken.tokenize 0
        |> Seq.map(fun tok ->
            tokens <- tok::tokens
            tok
        )
        |> Seq.map(fun tok ->
            match parser.tryReduce(states,tok) with
            | Some x -> states <- x
            | None -> ()
            tok
        )
    use _ =
        sq.Subscribe(Observer.Create(
            (fun lookahead ->
                states <- parser.shift(states,lookahead)
             ),(fun () ->
                match parser.tryReduce(states) with
                | Some x -> states <- x
                | None -> ()

                match states with
                |[1,lxm; 0,null] ->
                    result <- ExprParseTable.unboxRoot lxm
                | _ ->
                    failwith $"{stringify states}"
             )
        ))
    result
