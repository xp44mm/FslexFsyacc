﻿namespace FslexFsyacc.Fslex

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals
open FslexFsyacc.Runtime
open FslexFsyacc
open System.IO
type FslexTokenUtilsTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``tryWord``() =
        let x = "xyz"
        let y = 
            FslexTokenUtils.tokenize 0 x 
            |> Seq.toList
        let e = [{index= 0;length= 3;value= ID "xyz"}]
        show y
        Should.equal y e

    [<Fact>]
    member _.``tryWS``() =
        let x = "  "
        let y = 
            FslexTokenUtils.tokenize 0 x 
            |> Seq.toList

        Should.equal y []

    [<Fact>]
    member _.``trySingleLineComment``() =
        let x = "// xdfasdf\r\n   "
        let y = 
            FslexTokenUtils.tokenize 0 x 
            |> Seq.toList
        show y
        Should.equal y []

    [<Theory>]
    [<InlineData("(* empty *) ")>]
    member _.``tryMultiLineComment``(x) =
        let y = 
            FslexTokenUtils.tokenize 0 x 
            |> Seq.toList
        Should.equal y []

    [<Theory>]
    [<InlineData(""" "" """)>]
    [<InlineData(""" "\\" """)>]
    [<InlineData(""" "\"" """)>]
    [<InlineData(""" "\u1234" """)>]
    [<InlineData(""" "{" """)>]
    member _.``trySingleQuoteString``(x:string) =
        let x = x.Trim()

        let y = 
            FslexTokenUtils.tokenize 0 x 
            |> Seq.toList
        show y

    [<Theory>]
    [<InlineData("%{%}")>]
    [<InlineData("%{open System%}")>]
    member _.``tryHeader`` (x:string) =
        let y = 
            FslexTokenUtils.tokenize 0 x 
            |> Seq.toList
        show y

    [<Theory>]
    [<InlineData("{{}}")>]
    [<InlineData("{'}'}")>]
    [<InlineData("{ [lexbuf.Head] }")>]
    member _.``trySemantic`` (x:string) =
        let y = 
            FslexTokenUtils.tokenize 0 x 
            |> Seq.toList
        show y

    [<Fact>]
    member _.``fslex file test``() =
        let sourcePath = Path.Combine(Dir.solutionPath, @"FslexFsyacc\Fslex")
        let filePath = Path.Combine(sourcePath, @"fslex.fslex")
        let text = File.ReadAllText(filePath)
        let y = 
            FslexTokenUtils.tokenize 0 text 
            |> Seq.toList
        show y



