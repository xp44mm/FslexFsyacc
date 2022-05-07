namespace Expr
open Expr.ExprToken
open Expr.ExprParseTable


open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals
open FslexFsyacc.Runtime
open System.Reactive
open System.Reactive.Linq
open System.Collections.Generic


type ReactiveParserTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    //let compile (inp:string) =
    //    let states = Stack<int>([0])
        
    //    let trees =Stack<obj>()
    //    let parser = 
    //        ReactiveParser<int*int*ExprToken>(
    //            fxRules,actions,closures,getTag,getLexeme,states,trees)
        
        //let tokens =
            //tokens
        //let x = 
        //    inp
        //    |> ExprToken.tokenize
        //    |> Seq.map(fun((i,l,tok)as inp) ->                
        //        parser.next(inp)
        //        output.WriteLine($"{Literal.stringify tok}{Literal.stringify(List.ofSeq states) }") 
        //        inp
        //    )
        //x

    //[<Fact>]
    //member _.``0 - basis test``() =
    //    let inp = "2 + 3"
    //    compile inp
    //    |> List.ofSeq
    //    |> show
        //show result
        //Should.equal y 5.0

    //[<Fact>]
    //member _.``1 - prec test``() =
    //    let inp = "2 + 3 * 5"
    //    let y = compile inp
    //    //show result
    //    Should.equal y 17.0

    //[<Fact>]
    //member _.``2 - named prod test``() =
    //    let inp = "2 + 3 * -5"
    //    let y = compile inp
    //    //show result
    //    Should.equal y (-13.0)

    //[<Fact>]
    //member _.``2 - exception test``() =
    //    let y = Assert.Throws<_>(fun()->
    //        let inp = "2* + 4*3"
    //        let y = compile inp
    //        show y
    //    )

    //    output.WriteLine(y.Message)