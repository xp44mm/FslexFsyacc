namespace Expr

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text

open FSharp.xUnit
open FSharp.Literals.Literal
open FSharp.Idioms

open FslexFsyacc.Runtime
open Expr

type ExprCompilerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    let parser = 
        Parser<int*int*ExprToken>(
            ExprParseTable.rules,
            ExprParseTable.actions,
            ExprParseTable.closures,ExprToken.getTag,ExprToken.getLexeme)

    let parse(tokens:seq<int*int*ExprToken>) =
        tokens
        |> parser.parse
        |> ExprParseTable.unboxRoot

    let compile (inp:string) =
        inp
        |> ExprToken.tokenize
        |> parse

    [<Fact>]
    member _.``01 - output closures details``() =
        let theory = ExprParseTable.theoryParser
        let str = theory.collection()
        //let name = "expr"
        //let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.txt")
        //File.WriteAllText(outputDir,str,Encoding.UTF8)
        //output.WriteLine($"output:\r\n{outputDir}")
        output.WriteLine(str)

    [<Fact>]
    member _.``02 - basis test``() =
        let inp = "2 + 3"
        let y = compile inp
        //show result
        Should.equal y 5.0

    [<Fact>]
    member _.``03 - prec test``() =
        let inp = "2 + 3 * 5"
        let y = compile inp
        //show result
        Should.equal y 17.0

    [<Fact>]
    member _.``04 - named prod test``() =
        let inp = "2 + 3 * -5"
        let y = compile inp
        //show result
        Should.equal y (-13.0)

    [<Fact>]
    member _.``05 - exception test``() =
        let y = Assert.Throws<_>(fun()->
            let inp = "2* + 4*3"
            let y = compile inp
            show y
        )

        output.WriteLine(y.Message)

    [<Fact>]
    member _.``11 - state symbol pair test``() =
        let symbols = 
            ExprParseTable.theoryParser.getStateSymbolPairs()
            |> List.mapi Pair.ofApp

        //output.WriteLine(stringify symbols)
        let mp = [
            0,"";
            1,"expr";
            2,"(";
            3,"expr";
            4,")";
            5,"-";
            6,"expr";
            7,"NUMBER";
            8,"expr";
            9,"expr";
            10,"expr";
            11,"expr";
            12,"*";
            13,"+";
            14,"-";
            15,"/"]
        Should.equal mp symbols



