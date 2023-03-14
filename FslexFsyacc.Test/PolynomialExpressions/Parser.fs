module PolynomialExpressions.Parser

//open PolynomialExpressions.Tokenizer
open PolynomialExpressions
open FslexFsyacc.Runtime

let analyze (tokens:seq<_>) = 
    TermDFA.analyzer.analyze(tokens,Tokenizer.getTag)

let parse (posTokens:seq<Position<Tokenizer.Token>>) =
    posTokens
    |> analyze
    |> Seq.map(fun postok -> postok.value)
    |> Seq.toList

