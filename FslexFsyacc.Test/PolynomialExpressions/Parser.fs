module PolynomialExpressions.Parser

open PolynomialExpressions.Tokenizer
open PolynomialExpressions
open FslexFsyacc.Runtime

let parse (posTokens:seq<Position<Token>>) = 
    posTokens
    |> TermDFA.analyze
    |> Seq.map(fun postok -> postok.value)
    |> Seq.toList

