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
    member this.``tryWord``() =
        let x = "xyz"
        let y = FSharpSourceText.tryWord x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryWhiteSpace``() =
        let x = "  "
        let y = FSharpSourceText.tryWhiteSpace x
        Should.equal y <| Some(x,"")


    [<Fact>]
    member this.``trySingleLineComment``() =
        let x = "// xdfasdf\r\n   "
        let y = FSharpSourceText.trySingleLineComment x
        Should.equal y <| Some("// xdfasdf\r","\n   ")

    [<Fact>]
    member this.``tryMultiLineComment``() =
        let x = "(* empty *) "
        let y = FSharpSourceText.tryMultiLineComment x
        Should.equal y <| Some("(* empty *)"," ")

        let x1 = "(* ) "
        let y1 = FSharpSourceText.tryMultiLineComment x1
        Should.equal y1 None

    [<Fact>]
    member this.``tryChar``() =
        let xs = [@"'\\'";@"'\''";@"'\u0000'";@"'{'"]
        for x in xs do
            let y = FSharpSourceText.tryChar x
            Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryDoubleTick``() =
        let xs = ["``xs ys``"]
        for x in xs do
            let y = FSharpSourceText.tryDoubleTick x
            Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryTypeParameter``() =
        let xs = ["'a";"'abc"]
        for x in xs do
            let y = FSharpSourceText.tryTypeParameter x
            Should.equal y <| Some(x,"")

        let ns = ["'a'"]
        for n in ns do
            let m = FSharpSourceText.tryTypeParameter n
            Should.equal m None

    [<Fact>]
    member this.``trySingleQuoteString``() =
        let xs = 
            [""" "" """;""" "\\" """;""" "\"" """;""" "\u1234" """;""" "{" """;]
            |> List.map (fun x -> x.Trim())

        for x in xs do
            let y = FSharpSourceText.trySingleQuoteString x
            Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryVerbatimString``() =
        let xs = 
            [""" @"" """;""" @"\""x" """;]
            |> List.map (fun x -> x.Trim())

        for x in xs do
            let y = FSharpSourceText.tryVerbatimString x
            Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryTripleQuoteString``() =
        let xs = 
            ["\"\"\"xyz\"\"\""]

        for x in xs do
            let y = FSharpSourceText.tryTripleQuoteString x
            Should.equal y <| Some(x,"")


    [<Fact>]
    member this.``getHeaderLength``() =
        let x = "%}"
        let y = FSharpSourceText.getHeaderLength x
        let z = x.[0..y-1]
        Should.equal x z

        let x1 = "(*%}*)%}"
        let y1 = FSharpSourceText.getHeaderLength x1
        let z1 = x1.[0..y1-1]
        Should.equal x1 z1

    [<Fact>]
    member this.``getNestedActionLength``() =
        let x = "}"
        let y = FSharpSourceText.getNestedActionLength x
        let z = x.[0..y-1]
        Should.equal x z

        let x1 = "(*}*){}}"
        let y1 = FSharpSourceText.getNestedActionLength x1
        let z1 = x1.[0..y1-1]
        Should.equal x1 z1

    [<Fact>]
    member this.``tryHeader``() =
        let x = "%{%}"
        let y = FSharpSourceText.tryHeader x
        //show y
        Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryAction``() =
        let x = "{{}}"
        let y = FSharpSourceText.tryAction x
        //show y
        Should.equal y <| Some(x,"")
