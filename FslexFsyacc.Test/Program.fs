module Program 

open System.IO
open FslexFsyacc.Fsyacc
open System
open FSharp.Literals
open FslexFsyacc.Yacc
open Interpolation
open Interpolation.PlaceholderUtils
open FslexFsyacc.Runtime

let takeFirst (tokens:seq<_>) =
    let tryFinal states trees maybeToken =
        let ai =
            maybeToken
            |> Option.map getTag
            |> Option.defaultValue ""

        match states,ai with
        | [2;0],"}" -> true
        | _ -> false

    PlaceholderParseTable.parser.parse(tokens,tryFinal)

let compile (inp:string) =
    let states,trees,maybeToken =
        inp
        |> Tokenizer.tokenize 0
        |> Seq.filter(PlaceholderUtils.tokenFilter)
        |> takeFirst

    let expr = trees.Head    
    let rest =
        inp.Substring(maybeToken.Value.index)
    expr,rest

let [<EntryPoint>] main _ = 
    let inp = "2 + 3}1"
    let expr,rest = compile inp
    Console.WriteLine($"{expr},{Literal.stringify rest}")
    0
