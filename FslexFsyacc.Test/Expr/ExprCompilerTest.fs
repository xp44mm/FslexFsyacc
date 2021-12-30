namespace Expr

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals

type ExprCompilerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let compile (inp:string) =
        inp
        |> ExprToken.tokenize
        |> ExprParseTable.parse

    [<Fact>]
    member _.``0 - basis test``() =
        let inp = "2 + 3"
        let y = compile inp
        //show result
        Should.equal y 5.0

    [<Fact>]
    member _.``1 - prec test``() =
        let inp = "2 + 3 * 5"
        let y = compile inp
        //show result
        Should.equal y 17.0

    [<Fact>]
    member _.``2 - named prod test``() =
        let inp = "2 + 3 * -5"
        let y = compile inp
        //show result
        Should.equal y (-13.0)

