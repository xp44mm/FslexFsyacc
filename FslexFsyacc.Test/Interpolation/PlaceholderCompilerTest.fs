namespace Interpolation

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals

type PlaceholderCompilerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    //[<Fact>]
    //member _.``0 - basis test``() =
    //    let inp = "2 + 3}"
    //    let y = compile inp
    //    show y
    //    let e = BinaryExpression(Number 2.0,"+",Number 3.0)
    //    Should.equal y e

