namespace FslexFsyacc

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type FSharpSourceTextTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``tryWord``() =
        let x = "xyz"
        let y = FSharpSourceText.tryWord x

        Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``tryWS``() =
        let x = "  "
        let y = FSharpSourceText.tryWS x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``trySingleLineComment``() =
        let x = "// xdfasdf\r\n   "
        let y = FSharpSourceText.trySingleLineComment x
        Should.equal y <| Some("// xdfasdf\r","\n   ")

    [<Fact>]
    member _.``tryMultiLineComment``() =
        let x = "(* empty *) "
        let y = FSharpSourceText.tryMultiLineComment x
        Should.equal y <| Some("(* empty *)"," ")

        let x1 = "(* ) "
        let y1 = FSharpSourceText.tryMultiLineComment x1
        Should.equal y1 None

    [<Fact>]
    member _.``tryChar``() =
        let xs = [@"'\\'";@"'\''";@"'\u0000'";@"'{'"]
        for x in xs do
            let y = FSharpSourceText.tryChar x
            Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``tryDoubleTick``() =
        let xs = ["``xs ys``"]
        for x in xs do
            let y = FSharpSourceText.tryDoubleTick x
            Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``tryTypeParameter``() =
        let xs = ["'a";"'abc"]
        for x in xs do
            let y = FSharpSourceText.tryTypeParameter x
            Should.equal y <| Some(x,"")

        let ns = ["'a'"]
        for n in ns do
            let m = FSharpSourceText.tryTypeParameter n
            Should.equal m None

    [<Fact>]
    member _.``trySingleQuoteString``() =
        let xs = 
            [""" "" """;""" "\\" """;""" "\"" """;""" "\u1234" """;""" "{" """;]
            |> List.map (fun x -> x.Trim())

        for x in xs do
            let y = FSharpSourceText.trySingleQuoteString x
            Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``tryVerbatimString``() =
        let xs = 
            [""" @"" """;""" @"\""x" """;]
            |> List.map (fun x -> x.Trim())

        for x in xs do
            let y = FSharpSourceText.tryVerbatimString x
            Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``tryTripleQuoteString``() =
        let xs = 
            ["\"\"\"xyz\"\"\""]

        for x in xs do
            let y = FSharpSourceText.tryTripleQuoteString x
            Should.equal y <| Some(x,"")


    [<Fact>]
    member _.``getHeaderLength``() =
        let x = "%}"
        let y = FSharpSourceText.getHeaderLength x
        let z = x.[0..y-1]
        Should.equal x z

        let x1 = "(*%}*)%}"
        let y1 = FSharpSourceText.getHeaderLength x1
        let z1 = x1.[0..y1-1]
        Should.equal x1 z1

    [<Fact>]
    member _.``getNestedActionLength``() =
        let x = "}"
        let y = FSharpSourceText.getSemanticLength x
        let z = x.[0..y-1]
        Should.equal x z

        let x1 = "(*}*){}}"
        let y1 = FSharpSourceText.getSemanticLength x1
        let z1 = x1.[0..y1-1]
        Should.equal x1 z1

    [<Fact>]
    member _.``tryHeader``() =
        let x = "%{%}"
        let y = FSharpSourceText.tryHeader x
        //show y
        Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``trySemantic``() =
        let x = "{{}}"
        let y = FSharpSourceText.trySemantic x
        //show y
        Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``getColumnAndRest``() =
        let start = 19
        let inp = "0123456789\nabc"
        let pos = 20
        let col,nextStart,rest = 
            FSharpSourceText.getColumnAndRest (start, inp) pos
        //show (col,nextStart,rest)
        Should.equal col 1 //20-19,col base on 0
        Should.equal nextStart 30 //19+11
        Should.equal rest "abc" // rest after first \n

    [<Fact>]
    member _.``formatNestedCode``() =
        let col = 3
        let code = 
            [
                " let a = 0";
                "    let b = 1";
            ]
            |> String.concat System.Environment.NewLine
        let result = 
            FSharpSourceText.formatNestedCode col code
        //show result
        Should.equal result "let a = 0\r\nlet b = 1"



