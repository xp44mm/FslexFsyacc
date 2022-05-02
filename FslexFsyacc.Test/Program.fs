module Program 

open System.IO
open FslexFsyacc.Fsyacc
open System
open FSharp.Literals
open FslexFsyacc.Yacc
open Interpolation
open Interpolation.PlaceholderUtils

let takeFirst (tokens:seq<_>) =
    let tryFinal trees states ai =
        match states,ai with
        | [2;0],"}" ->
            Some trees
        | _ -> None

    PlaceholderParseTable.parser.head(tokens,getTag,getLexeme,tryFinal)

let compile (inp:string) =
    let expr,token =
        inp
        |> Tokenizer.tokenize 0
        |> Seq.filter(PlaceholderUtils.tokenFilter)
        |> takeFirst
        //|> PlaceholderParseTable.parse
    let rest =
        inp.Substring(token.index)
    expr,rest

let [<EntryPoint>] main _ = 
    let inp = "2 + 3}1"
    let expr,rest = compile inp
    Console.WriteLine($"{expr},{Literal.stringify rest}")
    0
