namespace FslexFsyacc.Fslex
open FslexFsyacc.Runtime

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
        |> FslexDFA.analyze
        |> Seq.concat
        |> List.ofSeq

    [<Fact>]
    member _.``explicit amp test``() =
        let tokens = 
            [LPAREN;ID "";RPAREN;LBRACK;RBRACK;STAR;LITERAL ""]
            |> List.map(fun t -> Position<_>.from(0,0,t))
        let y = analyze tokens |> List.map(fun x -> x.value)
        //show y

        let e = [LPAREN;ID "";RPAREN;AMP;LBRACK;RBRACK;STAR;AMP;LITERAL ""]
        Should.equal e y

