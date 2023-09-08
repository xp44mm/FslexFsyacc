namespace FslexFsyacc.Fsyacc
open FslexFsyacc.Runtime

open System.IO

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Literals.Literal

type FsyaccTokenTest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    [<Fact>]
    member _.``tryWord``() =
        let x = "xyz"
        let y = 
            FsyaccTokenUtils.tokenize 0 x 
            |> Seq.toList
        let e = [{index= 0;length= 3;value= FsyaccToken.ID "xyz"}]
        show y
        Should.equal y e

    [<Fact>]
    member _.``tryWS``() =
        let x = "  "
        let y = 
            FsyaccTokenUtils.tokenize 0 x 
            |> Seq.toList

        Should.equal y []

    [<Fact>]
    member _.``trySingleLineComment``() =
        let x = "// xdfasdf\r\n   "
        let y = 
            FsyaccTokenUtils.tokenize 0 x 
            |> Seq.toList
        show y
        Should.equal y []

    [<Theory>]
    [<InlineData("(* empty *) ")>]
    member _.``tryMultiLineComment``(x) =
        let y = 
            FsyaccTokenUtils.tokenize 0 x 
            |> Seq.toList
        Should.equal y []

    [<Theory>]
    [<InlineData(""" "" """)>]
    [<InlineData(""" "\\" """)>]
    [<InlineData(""" "\"" """)>]
    [<InlineData(""" "\u1234" """)>]
    [<InlineData(""" "{" """)>]
    member _.``trySingleQuoteString``(x:string) =
        let x = x.Trim()

        let y = 
            FsyaccTokenUtils.tokenize 0 x 
            |> Seq.toList
        show y

    [<Theory>]
    [<InlineData("%{%}")>]
    [<InlineData("%{open System%}")>]
    member _.``tryHeader`` (x:string) =
        let y = 
            FsyaccTokenUtils.tokenize 0 x 
            |> Seq.toList
        show y

    [<Theory>]
    [<InlineData("{{}}")>]
    [<InlineData("{'}'}")>]
    [<InlineData("{ [lexbuf.Head] }")>]
    member _.``trySemantic`` (x:string) =
        let y = 
            FsyaccTokenUtils.tokenize 0 x 
            |> Seq.toList
        show y

    [<Theory>]
    [<InlineData("%type")>]
    [<InlineData("%type <int>")>]
    [<InlineData("%type <seq<float*string>> starts")>]
    member _.``type test`` (x:string) =
        let y = 
            FsyaccTokenUtils.tokenize 0 x 
            |> Seq.toList
        show y


