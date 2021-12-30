namespace PolynomialExpressions

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit
open PolynomialExpressions.Tokenizer
open FSharp.Idioms

type TokenizerTest(output:ITestOutputHelper) =
    let show res = 
        res 
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``test the tokenizer on some sample inputs``() =
        let input = "x**5 - 2x**3 + 20"
        let tokens = Tokenizer.tokenize input |> Seq.map Triple.last |> Seq.toList
        let y = [ID "x";HAT;INT 5;MINUS;INT 2;ID "x";HAT;INT 3;PLUS;INT 20]
        
        //show tokens
        Should.equal y tokens
