namespace FslexFsyacc.Fsyacc

open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FslexFsyacc.Fsyacc.FsyaccToken
open FSharp.xUnit

type FsyaccTokenAnalyzeTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let analyze tokens = 
        tokens
        |> FsyaccDFA.analyze
        |> Seq.concat
        |> Seq.toList

    [<Fact>]
    member this.``clear bof sep test``() =
        let tokens = [BOF; SEMICOLON; PERCENT; SEMICOLON]
        let y = analyze tokens
        //show y

        let e = []
        Should.equal e y

    [<Fact>]
    member this.``clear EOF test``() =
        let tokens = [SEMICOLON; PERCENT; SEMICOLON; EOF]
        let y = analyze tokens
        //show y

        let e = []
        Should.equal e y

    [<Fact>]
    member this.``clear SEMICOLON before percent test``() =
        let tokens = [ SEMICOLON; PERCENT]
        let y = analyze tokens
        //show y

        let e = [PERCENT]
        Should.equal e y

    [<Fact>]
    member this.``collapse many SEMICOLON test``() =
        let tokens = [ SEMICOLON; SEMICOLON]
        let y = analyze tokens
        //show y

        let e = [SEMICOLON]
        Should.equal e y
