module FSharpAnatomy.TypeArgumentCompiler

open FslexFsyacc.Runtime
open FSharpAnatomy.FSharpTokenUtils

let analyze (posTokens:seq<Position<FSharpToken>>) = 
    posTokens
    |> TypeArgumentDFA.analyze

let parser = Parser<Position<FSharpToken>> (
    TypeArgumentParseTable.rules,
    TypeArgumentParseTable.actions,
    TypeArgumentParseTable.closures,getTag,getLexeme)

let parse (tokens:seq<Position<FSharpToken>>) =
    tokens
    |> parser.parse
    |> TypeArgumentParseTable.unboxRoot

let compile (srctxt:string) =
    srctxt
    |> FSharpTokenUtils.tokenize
    |> TypeArgumentDFA.analyze
    |> parse
