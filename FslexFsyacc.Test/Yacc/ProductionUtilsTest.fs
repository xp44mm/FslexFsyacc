namespace FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.Idioms
open FSharp.xUnit

type ProductionUtilsTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``eliminate test`` () =
        let body = ["let";"pattern";"="; "expr"]
        let symbol = "pattern"
        let bodies = [
            [ "{";"}"]
            [ "(";")"]
            [ "[";"]"]
            ]
        let y = ProductionUtils.eliminateSymbol(symbol, bodies) body

        let e = [
            ["let";"{";"}";"=";"expr"];
            ["let";"(";")";"=";"expr"];
            ["let";"[";"]";"=";"expr"]]

        show y
        Should.equal e y
