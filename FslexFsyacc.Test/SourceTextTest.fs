namespace FslexFsyacc

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type SourceTextTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member this.``tryWhiteSpace``() =
        let x = " \r\n "
        let y = SourceText.tryWhiteSpace x
        Should.equal y <| Some(" \r","\n ")

    [<Fact>]
    member this.``tryLineTerminator``() =
        let x = "\r\n000"
        let y = SourceText.tryLineTerminator x
        Should.equal y <| Some("\r\n","000")

        let x1 = "\n000"
        let y1 = SourceText.tryLineTerminator x1
        Should.equal y1 <| Some("\n","000")

    [<Fact>]
    member this.``trySingleLineComment``() =
        let x = "// xdfasdf\r\n   "
        let y = SourceText.trySingleLineComment x
        Should.equal y <| Some("// xdfasdf\r","\n   ")

    [<Fact>]
    member this.``tryMultiLineComment``() =
        let x = "/* empty */ "
        let y = SourceText.tryMultiLineComment x
        Should.equal y <| Some("/* empty */"," ")

    [<Fact>]
    member this.``tryIdentifierName``() =
        let x = "xyz-"
        let y = SourceText.tryIdentifierName x
        Should.equal y <| Some("xyz","-")

    [<Fact>]
    member this.``tryOptionalChainingPunctuator``() =
        let x = "?.toString()"
        let y = SourceText.tryOptionalChainingPunctuator x
        Should.equal y <| Some("?.","toString()")

    [<Fact>]
    member this.``illegalNumberSep``() =
        for x in ["_0";"0_";"0__0";"0_.0";"0._0"] do
            let y = SourceText.illegalNumberSep x
            Should.equal y true

    [<Fact>]
    member this.``tryBinaryIntegerLiteral``() =
        let x = "0b111_111n"
        let y = SourceText.tryBinaryIntegerLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryOctalIntegerLiteral``() =
        let x = "0o765_432n"
        let y = SourceText.tryOctalIntegerLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryHexIntegerLiteral``() =
        let x = "0xabcdef_987n"
        let y = SourceText.tryHexIntegerLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryDecimalIntegerLiteral``() =
        let x = "987_654321n"
        let y = SourceText.tryDecimalIntegerLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryDecimalLiteral``() =
        let x = "987_654.321e-5"
        let y = SourceText.tryDecimalLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``trySingleStringLiteral``() =
        let x = @"'\r\n\\\'\\'"
        let y = SourceText.trySingleStringLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryDoubleStringLiteral``() =
        let x = """ "\p{L}\\\"\\" """.Trim()
        let y = SourceText.tryDoubleStringLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryTemplate``() =
        let x = @"`\\\`\\`"
        let y = SourceText.tryTemplate x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member this.``tryRegularExpressionLiteral``() =
        let x = @"/\\\/\\/"
        let y = SourceText.tryRegularExpressionLiteral x
        Should.equal y <| Some(x,"")
