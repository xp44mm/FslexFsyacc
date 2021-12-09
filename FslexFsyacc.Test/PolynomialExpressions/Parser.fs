﻿module PolynomialExpressions.Parser

open PolynomialExpressions.Tokenizer
open PolynomialExpressions

let parse (posTokens:seq<int*Token>) = 
    posTokens
    |> TermDFA.analyze
    |> Seq.toList

