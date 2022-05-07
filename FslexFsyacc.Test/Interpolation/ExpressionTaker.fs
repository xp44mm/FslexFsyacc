module Interpolation.ExpressionTaker

open Interpolation.PlaceholderUtils

//let takeFirst (tokens:seq<_>) =
//    let tryFinal states trees maybeToken =
//        let ai =
//            maybeToken
//            |> Option.map getTag
//            |> Option.defaultValue ""

//        match states,ai with
//        | [2;0],"}" -> true
//        | _ -> false

//    PlaceholderParseTable.parser.parse(tokens,tryFinal)

//let compile (inp:string) =
//    let states,trees,maybeToken =
//        inp
//        |> Tokenizer.tokenize 0
//        |> Seq.filter(PlaceholderUtils.tokenFilter)
//        |> takeFirst

//    let expr = trees.Head    
//    let rest =
//        inp.Substring(maybeToken.Value.index)
//    expr,rest

