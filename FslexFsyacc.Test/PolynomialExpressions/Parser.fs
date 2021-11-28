module PolynomialExpressions.Parser

open PolynomialExpressions.Tokenizer
open PolynomialExpressions

let parse (tokens:seq<Token>) = 
    tokens
    |> TermDFA.split
    |> Seq.toList

