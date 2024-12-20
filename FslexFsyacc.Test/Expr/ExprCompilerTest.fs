﻿namespace FslexFsyacc.Expr

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text

open FSharp.xUnit
open FSharp.Idioms.Literal
open FSharp.Idioms

open FslexFsyacc

type ExprCompilerTest(output:ITestOutputHelper) =

    //[<Fact>]
    //member _.``01 - output closures details``() =
    //    let theory = ExprParseTable.parser
    //    let str = theory.collection()
    //    output.WriteLine(str)

    [<Fact>]
    member _.``02 - basis test``() =
        let inp = "2 + 3"
        let y = ExprCompiler.compile inp
        //show result
        Should.equal y 5.0

    [<Fact>]
    member _.``03 - prec test``() =
        let inp = "2 + 3 * 5"
        let y = ExprCompiler.compile inp
        //show result
        Should.equal y 17.0

    [<Fact>]
    member _.``04 - named prod test``() =
        let inp = "2 + 3 * -5"
        let y = ExprCompiler.compile inp
        //show result
        Should.equal y (-13.0)

    [<Fact>]
    member _.``05 - exception test``() =
        let y = Assert.Throws<_>(fun()->
            let inp = "2* + 4*3"
            let y = ExprCompiler.compile inp
            output.WriteLine(stringify y)
        )

        output.WriteLine(y.Message)

    [<Fact>]
    member _.``11 - state symbol pair test``() =
        let symbols = 
            ExprCompiler.table.kernelSymbols
            |> Map.toList

        //output.WriteLine(stringify symbols)
        //[
        //    0,"";
        //    1,"expr";
        //    2,"(";
        //    3,"expr";
        //    4,")";
        //    5,"-";
        //    6,"expr";
        //    7,"NUMBER";
        //    8,"expr";
        //    9,"expr";
        //    10,"expr";
        //    11,"expr";
        //    12,"*";
        //    13,"+";
        //    14,"-";
        //    15,"/"]
        let mp = 
            [0,"";1,"expr";2,"(";3,"expr";4,")";5,"NUMBER";6,"expr";7,"expr";8,"expr";9,"expr";10,"expr";11,"*";12,"+";13,"-";14,"/";15,"unaryExpr";16,"-"]
        Should.equal mp symbols




