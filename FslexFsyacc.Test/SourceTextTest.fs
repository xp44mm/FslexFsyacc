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
    member _.``tryWhiteSpace``() =
        let x = " \r\n "
        let y = SourceText.tryWhiteSpace x
        Should.equal y <| Some(" \r","\n ")

    [<Fact>]
    member _.``tryLineTerminator``() =
        let x = "\r\n000"
        let y = SourceText.tryLineTerminator x
        Should.equal y <| Some("\r\n","000")

        let x1 = "\n000"
        let y1 = SourceText.tryLineTerminator x1
        Should.equal y1 <| Some("\n","000")

    [<Fact>]
    member _.``trySingleLineComment``() =
        let x = "// xdfasdf\r\n   "
        let y = SourceText.trySingleLineComment x
        Should.equal y <| Some("// xdfasdf\r","\n   ")

    [<Fact>]
    member _.``tryMultiLineComment``() =
        let x = "/* empty */ "
        let y = SourceText.tryMultiLineComment x
        Should.equal y <| Some("/* empty */"," ")

    [<Fact>]
    member _.``tryIdentifierName``() =
        let x = "xyz-"
        let y = SourceText.tryIdentifierName x
        Should.equal y <| Some("xyz","-")

    [<Fact>]
    member _.``tryOptionalChainingPunctuator``() =
        let x = "?.toString()"
        let y = SourceText.tryOptionalChainingPunctuator x
        Should.equal y <| Some("?.","toString()")

    [<Fact>]
    member _.``illegalNumberSep``() =
        for x in ["_0";"0_";"0__0";"0_.0";"0._0"] do
            let y = SourceText.illegalNumberSep x
            Should.equal y true

    [<Fact>]
    member _.``tryBinaryIntegerLiteral``() =
        let x = "0b111_111n"
        let y = SourceText.tryBinaryIntegerLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``tryOctalIntegerLiteral``() =
        let x = "0o765_432n"
        let y = SourceText.tryOctalIntegerLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``tryHexIntegerLiteral``() =
        let x = "0xabcdef_987n"
        let y = SourceText.tryHexIntegerLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``tryDecimalIntegerLiteral``() =
        let x = "987_654321n"
        let y = SourceText.tryDecimalIntegerLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``tryDecimalLiteral``() =
        let x = "987_654.321e-5"
        let y = SourceText.tryDecimalLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``trySingleStringLiteral``() =
        let x = @"'\r\n\\\'\\'"
        let y = SourceText.trySingleStringLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``tryDoubleStringLiteral``() =
        let x = """ "\p{L}\\\"\\" """.Trim()
        let y = SourceText.tryDoubleStringLiteral x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``tryTemplate``() =
        let x = @"`\\\`\\`"
        let y = SourceText.tryTemplate x
        Should.equal y <| Some(x,"")

    [<Fact>]
    member _.``tryRegularExpressionLiteral``() =
        let x = @"/\\\/\\/"
        let y = SourceText.tryRegularExpressionLiteral x
        Should.equal y <| Some(x,"")
