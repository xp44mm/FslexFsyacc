namespace FslexFsyacc.Fslex
open FslexFsyacc

open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Idioms.Literal
open FSharp.xUnit
open FSharp.Idioms

type AnalyzeTest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    let analyze tokens =
        tokens
        |> FslexCompiler.analyze
        |> Seq.concat
        |> List.ofSeq

    [<Fact>]
    member _.``input tokens is empty test``() =
        let tokens = []
        let y = 
            tokens
            |> analyze
            |> List.map(fun x -> x.value)

        output.WriteLine(stringify y)

        let e = []
        Should.equal e y

    [<Fact>]
    member _.``explicit amp test``() =
        let tokens = 
            [LPAREN;ID "";RPAREN;LBRACK;RBRACK;STAR;LITERAL ""]
            |> List.map(fun t -> Position.from(0,0,t))
        let y = analyze tokens |> List.map(fun x -> x.value)
        //show y

        let e = [LPAREN;ID "";RPAREN;AMP;LBRACK;RBRACK;STAR;AMP;LITERAL ""]
        Should.equal e y
