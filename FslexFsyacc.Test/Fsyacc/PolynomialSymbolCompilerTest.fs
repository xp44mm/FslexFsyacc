namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals.Literal
open FSharp.Idioms

open System.IO
open System.Text

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

type PolynomialSymbolCompilerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    let compile text =
        text
        |> PolynomialSymbolUtils.tokenize
        |> PolynomialSymbolParseTable.parse

    [<Fact>]
    member _.``01 - rec ? test``() =
        let text = """rec ?"""
        let y = compile text
        let e = Repetition(Atomic "rec","?")
        Should.equal e y
        ()

    [<Fact>]
    member _.``02 - oneof test``() =
        let text = """[sig begin]"""
        let y = compile text
        let e = Oneof [Atomic "sig";Atomic "begin"]
        Should.equal e y
        //show y
        ()

    [<Fact>]
    member _.``03 - chain test``() =
        let text = """(sig begin)"""
        let y = compile text
        let e = Chain [Atomic "sig";Atomic "begin"]
        Should.equal e y
        //show y
        ()

    [<Fact>]
    member _.``04 - rec ? test``() =
        let x = Repetition(Atomic "rec","?")
        let y = PolynomialSymbol.render x
        let e = "rec?"
        Should.equal e y
        show y
        ()

    [<Fact>]
    member _.``05 - oneof test``() =
        let x = Oneof [Atomic "sig";Atomic "begin"]
        let y = PolynomialSymbol.render x
        let e = """[sig begin]"""
        Should.equal e y
        show y
        ()

    [<Fact>]
    member _.``06 - chain test``() =
        let x = Chain [Atomic "sig";Atomic "begin"]
        let y = PolynomialSymbol.render x
        let e = """(sig begin)"""
        Should.equal e y
        show y
        ()




