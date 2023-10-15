namespace FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type ProductionUtilsTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine
    static let splitData = [
        {|body=[];symbol="x";res=[]|}
        {|body=["y"];symbol="x";res=["y"]|}
        {|body=["y"];symbol="x";res=["y"]|}

    ]

    [<Fact>]
    member _.``empty test`` () =
        let y = ProductionUtils.split [] "x"
        Should.equal [] y

    [<Fact>]
    member _.``split noexists test`` () =
        let y = ProductionUtils.split ["y"] "x"
        Should.equal [["y"]] y

    [<Fact>]
    member _.``split noexists 2 test`` () =
        let y = ProductionUtils.split ["a";"y"] "x"
        Should.equal [["a";"y"]] y

    [<Fact>]
    member _.``split exists 1 test`` () =
        let y = ProductionUtils.split ["x"] "x"
        Should.equal [["x"]] y

    [<Fact>]
    member _.``split exists abc a test`` () =
        let y = ProductionUtils.split ["a";"b";"c"] "a"
        Should.equal [["a"];["b";"c"]] y

    [<Fact>]
    member _.``split exists abc b test`` () =
        let y = ProductionUtils.split ["a";"b";"c"] "b"
        Should.equal [["a"];["b"];["c"]] y

    [<Fact>]
    member _.``split exists abc c test`` () =
        let y = ProductionUtils.split ["a";"b";"c"] "c"
        Should.equal [["a";"b"];["c"]] y

    [<Fact>]
    member _.``split exists abccb b test`` () =
        let y = ProductionUtils.split ["a";"b";"c";"c";"b"] "b"
        Should.equal [["a"];["b"];["c";"c"];["b"]] y

    [<Fact>]
    member _.``split exists abccb c test`` () =
        let y = ProductionUtils.split ["a";"b";"c";"c";"b"] "c"
        Should.equal [["a";"b"];["c"];["c"];["b"]] y

    [<Fact>]
    member _.``crosspower abcd test`` () =
        let a = [["a";"b"];["c";"d"];]

        let y1 = ProductionUtils.crosspower 1 a
        let e1 = [
            [["a";"b"]];
            [["c";"d"]]]

        Should.equal e1 y1

        let y2 = ProductionUtils.crosspower 2 a
        let e2 = [
            [["a";"b"];["a";"b"]];
            [["a";"b"];["c";"d"]];
            [["c";"d"];["a";"b"]];
            [["c";"d"];["c";"d"]]]

        let y3 = ProductionUtils.crosspower 3 a
        let e3 =[
            [["a";"b"];["a";"b"];["a";"b"]];
            [["a";"b"];["a";"b"];["c";"d"]];
            [["a";"b"];["c";"d"];["a";"b"]];
            [["a";"b"];["c";"d"];["c";"d"]];
            [["c";"d"];["a";"b"];["a";"b"]];
            [["c";"d"];["a";"b"];["c";"d"]];
            [["c";"d"];["c";"d"];["a";"b"]];
            [["c";"d"];["c";"d"];["c";"d"]]]

        show y3
        Should.equal e3 y3

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
