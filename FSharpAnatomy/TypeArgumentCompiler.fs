﻿module FSharpAnatomy.TypeArgumentCompiler

open FslexFsyacc.Runtime
open FSharp.Literals.Literal
open System.Reactive
open System.Reactive.Linq

let analyze (posTokens:seq<Position<FSharpToken>>) = 
    posTokens
    |> ArrayTypeSuffixDFA.analyze

let parser = Parser<Position<FSharpToken>> (
    TypeArgumentParseTable.rules,
    TypeArgumentParseTable.actions,
    TypeArgumentParseTable.closures,
    
    TypeArgumentUtils.getTag,
    TypeArgumentUtils.getLexeme)

let parse (tokens:seq<Position<FSharpToken>>) =
    tokens
    |> parser.parse
    |> TypeArgumentParseTable.unboxRoot

let compile (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]
    let mutable result = defaultValue<_>
    let sq =
        txt
        |> TypeArgumentUtils.tokenize 0
        |> ArrayTypeSuffixDFA.analyze
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
                    result <- TypeArgumentParseTable.unboxRoot lxm
                | _ ->
                    failwith $"{stringify states}"
             )
        ))
    result

