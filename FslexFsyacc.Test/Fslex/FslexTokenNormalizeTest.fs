namespace FslexFsyacc.Fslex

open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FslexFsyacc.Fslex.FslexToken
open FSharp.xUnit

type FslexTokenNormalizeTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let norm tokens =
        tokens
        |> FslexDFA.analyze
        |> Seq.concat
        |> List.ofSeq

    [<Fact>]
    member this.``explicit amp test``() =
        let tokens = [LPAREN;ID "";RPAREN;LBRACK;RBRACK;STAR;QUOTE ""]
        let y = norm tokens
        //show y

        let e = [LPAREN;ID "";RPAREN;AMP;LBRACK;RBRACK;STAR;AMP;QUOTE ""]
        Should.equal e y

    [<Fact>]
    member this.``clear bof test``() =
        let tokens = [BOF; LF; PERCENT; LF; ID ""]
        let y = norm tokens
        //show y

        let e = [ID ""]
        Should.equal e y

    [<Fact>]
    member this.``clear EOF test``() =
        let tokens = [LF; PERCENT; LF; EOF]
        let y = norm tokens
        //show y

        let e = []
        Should.equal e y

    [<Fact>]
    member this.``clear lf after percent test``() =
        let tokens = [ PERCENT; LF]
        let y = norm tokens
        //show y

        let e = [PERCENT; ]
        Should.equal e y

    [<Fact>]
    member this.``collapse many LF test``() =
        let tokens = [ LF; LF]
        let y = norm tokens
        //show y

        let e = [LF; ]
        Should.equal e y
