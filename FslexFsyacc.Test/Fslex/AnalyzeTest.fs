namespace FslexFsyacc.Fslex

open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open FSharp.Idioms

type AnalyzeTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let analyze tokens =
        tokens
        |> FslexDFA2.analyze
        |> Seq.concat
        |> List.ofSeq

    [<Fact>]
    member _.``explicit amp test``() =
        let tokens = [LPAREN;ID "";RPAREN;LBRACK;RBRACK;STAR;QUOTE ""] |> List.map(fun t -> 0,0,t)
        let y = analyze tokens |> List.map Triple.last
        //show y

        let e = [LPAREN;ID "";RPAREN;AMP;LBRACK;RBRACK;STAR;AMP;QUOTE ""]
        Should.equal e y

