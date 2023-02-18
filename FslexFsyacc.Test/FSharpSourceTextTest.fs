namespace FslexFsyacc

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals
open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals
open FslexFsyacc.Runtime
open FslexFsyacc
open System.IO

type FSharpSourceTextTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``tryWord``() =
        let x = "xyz"
        let y = 
            FSharpSourceText.tryWord x 
            |> Option.get

        Should.equal x y.Value

    [<Fact>]
    member _.``tryWS``() =
        let x = "  "
        let y = 
            FSharpSourceText.tryWS x
            |> Option.get

        Should.equal x y.Value

    [<Fact>]
    member _.``trySingleLineComment``() =
        let x = "// xdfasdf\r\n   "
        let y = 
            FSharpSourceText.trySingleLineComment x
            |> Option.get
        let e = "// xdfasdf\r"
        Should.equal e y.Value 

    [<Theory>]
    [<InlineData("(* empty *) ","(* empty *)")>]
    [<InlineData("(* ) ",null)>]
    member _.``tryMultiLineComment``(x,e) =
        let e = if e = null then None else Some e
        let y = 
            FSharpSourceText.tryMultiLineComment x
            |> Option.map(fun m -> m.Value)
        Should.equal y e

    [<Theory>]
    [<InlineData(@"'\\'")>]
    [<InlineData(@"'\''")>]
    [<InlineData(@"'\u0000'")>]
    [<InlineData(@"'{'")>]
    member _.``tryChar`` x =
        let y = 
            FSharpSourceText.tryChar x
            |> Option.get
        Should.equal x y.Value

    [<Theory>]
    [<InlineData("``xs ys``")>]
    member _.``tryDoubleTick`` x =
        let y = 
            FSharpSourceText.tryDoubleTick x
            |> Option.get

        Should.equal x y.Value

    [<Theory>]
    [<InlineData("'a",true)>]
    [<InlineData("'abc",true)>]
    [<InlineData("'a'",false)>]
    member _.``tryTypeParameter``(x,e) =
        let y = 
            FSharpSourceText.tryTypeParameter x
            |> Option.map(fun m -> m.Value)
        let e = if e then Some x else None
        Should.equal y e

    [<Theory>]
    [<InlineData(""" "" """)>]
    [<InlineData(""" "\\" """)>]
    [<InlineData(""" "\"" """)>]
    [<InlineData(""" "\u1234" """)>]
    [<InlineData(""" "{" """)>]
    member _.``trySingleQuoteString``(x:string) =
        let x = x.Trim()

        let y = 
            FSharpSourceText.trySingleQuoteString x
            |> Option.map(fun m -> m.Value)
            |> Option.get

        Should.equal y x

    [<Theory>]
    [<InlineData(""" @"" """)>]
    [<InlineData(""" @"\""x" """)>]
    member _.``tryVerbatimString``(x:string) =
        let x = x.Trim()

        let y = 
            FSharpSourceText.tryVerbatimString x
            |> Option.map(fun m -> m.Value)
            |> Option.get

        Should.equal y x

    [<Theory>]
    [<InlineData("\"\"\"xyz\"\"\"")>]
    member _.``tryTripleQuoteString`` (x:string) =
        let x = x.Trim()
        let y = 
            FSharpSourceText.tryTripleQuoteString x
            |> Option.map(fun m -> m.Value)
            |> Option.get

        Should.equal y x


    [<Theory>]
    [<InlineData("%}")>]
    [<InlineData("(*%}*)%}")>]
    member _.``getHeaderLength`` (x:string) =
        //
        let y = FSharpSourceText.getHeaderLength x
        let z = x.[0..y-1]
        show z
        Should.equal x z


    [<Theory>]
    [<InlineData("}")>]
    [<InlineData("(*}*){}}")>]
    member _.``getNestedActionLength`` (x:string) =
        
        let y = FSharpSourceText.getSemanticLength x
        let z = x.[0..y-1]
        show z
        Should.equal x z

    [<Theory>]
    [<InlineData("%{%}")>]
    [<InlineData("%{open System%}")>]
    member _.``tryHeader`` (x:string) =
        let y = 
            FSharpSourceText.tryHeader x
            |> Option.get
        show y
        Should.equal y x

    [<Theory>]
    [<InlineData("{{}}")>]
    [<InlineData("{'}'}")>]
    member _.``trySemantic`` (x:string) =
        let y = 
            FSharpSourceText.trySemantic x
            |> Option.get

        show y
        Should.equal y x

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

