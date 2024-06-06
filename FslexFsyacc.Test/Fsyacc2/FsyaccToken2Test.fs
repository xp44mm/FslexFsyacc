namespace FslexFsyacc.Fsyacc
open FslexFsyacc

open System.IO

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms.Literal

type FsyaccToken2Test (output:ITestOutputHelper) =

    [<Fact>]
    member _.``tryWord``() =
        let x = "xyz"
        let y = 
            FsyaccToken2Utils.tokenize 0 x 
            |> Seq.toList
        let e = [{index= 0;length= 3;value= FsyaccToken2.ID "xyz"}]
        Should.equal y e

    [<Fact>]
    member _.``tryWS``() =
        let x = "  "
        let y = 
            FsyaccToken2Utils.tokenize 0 x 
            |> Seq.toList

        Should.equal y []

    [<Fact>]
    member _.``trySingleLineComment``() =
        let x = "// xdfasdf\r\n   "
        let y = 
            FsyaccToken2Utils.tokenize 0 x 
            |> Seq.toList
        Should.equal y []

    [<Theory>]
    [<InlineData("(* empty *) ")>]
    member _.``tryMultiLineComment``(x) =
        let y = 
            FsyaccToken2Utils.tokenize 0 x 
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
            FsyaccToken2Utils.tokenize 0 x 
            |> Seq.toList
        output.WriteLine(stringify y)

    [<Theory>]
    [<InlineData("%{%}")>]
    [<InlineData("%{open System%}")>]
    member _.``tryHeader`` (x:string) =
        let y = 
            FsyaccToken2Utils.tokenize 0 x 
            |> Seq.toList
        output.WriteLine(stringify y)

    [<Theory>]
    [<InlineData("{{}}")>]
    [<InlineData("{'}'}")>]
    [<InlineData("{ [lexbuf.Head] }")>]
    member _.``trySemantic`` (x:string) =
        let y = 
            FsyaccToken2Utils.tokenize 0 x 
            |> Seq.toList
        output.WriteLine(stringify y)

    [<Theory>]
    [<Natural(4)>]
    member _.``type test`` (i:int) =
        let ls = [
            "%type"
            "%type<_>"
            "%type <int>"
            "%type <seq<float*string>> starts"
        ]

        let y = 
            FsyaccToken2Utils.tokenize 0 ls.[i]
            |> Seq.map(fun tok ->
                output.WriteLine(stringify tok)
            )
            |> Seq.toList
        output.WriteLine(stringify y)

