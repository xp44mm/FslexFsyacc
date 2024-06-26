﻿namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms
open FslexFsyacc.TypeArguments

type SemanticGeneratorTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``semantic actions``() =
        let typeAnnotations = Map [
            "expr", Ctor(["float"],[])
        ]
        let prodSymbols = [ "expr" ; "expr" ; "+"; "expr"]
        let semantic = "s0 + s3"

        let y = ReducerGenerator.decorateReducer typeAnnotations prodSymbols semantic
        //output.WriteLine(y)
        let e = 
            """
fun(ss:obj list)->
    let s0 = unbox<float> ss.[0]
    let s2 = unbox<float> ss.[2]
    let result:float =
        s0 + s3
    box result
            """.Trim()

        Should.equal e y
