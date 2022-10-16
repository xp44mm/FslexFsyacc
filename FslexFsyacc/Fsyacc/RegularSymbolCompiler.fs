module FslexFsyacc.Fsyacc.RegularSymbolCompiler
open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc
//open FslexFsyacc.Fsyacc.RegularSymbolToken

let parser = 
    Parser<int*int*RegularSymbolToken>(
        RegularSymbolParseTable.rules,
        RegularSymbolParseTable.actions,
        RegularSymbolParseTable.closures,RegularSymbolToken.getTag,RegularSymbolToken.getLexeme)

let parse(tokens:seq<int*int*RegularSymbolToken>) =
    tokens
    |> parser.parse
    |> RegularSymbolParseTable.unboxRoot

let compile (inp:string) =
    inp
    |> RegularSymbolToken.tokenize
    |> parse
