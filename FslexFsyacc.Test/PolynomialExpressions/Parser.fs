module PolynomialExpressions.Parser

open PolynomialExpressions.Tokenizer
open PolynomialExpressions

let parse (posTokens:seq<int*int*Token>) = 
    posTokens
    |> TermDFA2.analyze
    |> Seq.toList

