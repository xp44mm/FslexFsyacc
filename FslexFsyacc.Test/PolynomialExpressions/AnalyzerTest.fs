﻿namespace PolynomialExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals
open PolynomialExpressions
open PolynomialExpressions.Tokenizer
open FSharp.Idioms
open FslexFsyacc.Runtime

type AnalyzerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``basis``() =
        let x = "2x**2+3x-5"
        let y = x |> Tokenizer.tokenize |> TermDFA.analyze |> Seq.map(fun postok -> postok.value) |> List.ofSeq
        //show y
        Should.equal y [Term(2,"x",2);Term(3,"x",1);Const -5]

    [<Fact>]
    member _.``tokens``() =
        let x = "2x**2+3x-5"
        let tokens = x |> Tokenizer.tokenize |> Seq.map(fun postok -> postok.value) |> Seq.toList
        show tokens
        Should.equal tokens [INT 2;ID "x";HAT;INT 2;PLUS;INT 3;ID "x";MINUS;INT 5]

    [<Fact>]
    member _.``analyze test``() =
        let tokens = [INT 2;ID "x";HAT;INT 2;PLUS;INT 3;ID "x";MINUS;INT 5]
        let y = tokens |> Seq.map(fun tok -> Position<_>.from(0,0,tok)) |> TermDFA.analyze |> Seq.map(fun postok -> postok.value) |> List.ofSeq
        show y
        Should.equal y [Term(2,"x",2);Term(3,"x",1);Const -5]

