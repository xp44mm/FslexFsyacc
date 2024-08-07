﻿module PolynomialExpressions.Parser

//open PolynomialExpressions.Tokenizer
open PolynomialExpressions
open FslexFsyacc

let analyze (tokens:seq<_>) = 
    TermDFA.analyzer.analyze(tokens,Tokenizer.getTag)

let parse (posTokens:seq<PositionWith<Tokenizer.Token>>) =
    posTokens
    |> analyze
    |> Seq.map(fun postok -> postok.value)
    |> Seq.toList

