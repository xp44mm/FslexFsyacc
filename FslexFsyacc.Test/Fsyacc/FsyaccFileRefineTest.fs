namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit

type FsyaccFileRefineTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``00 - refine production``() =
        let rules = [
            "AsyncArrowFunction",[
                ["async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
                ["CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""]]

        let oldProd = ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"]
        let newProd = ["AsyncArrowFunction";"async";"ArrowFormalParameters";"=>";"AsyncConciseBody"]

        let y = 
            rules 
            |> FsyaccFileRefine.refineRules oldProd newProd
        show y
        let z = ["AsyncArrowFunction",[
            ["async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["async";"ArrowFormalParameters";"=>";"AsyncConciseBody"],"",""]]
        Should.equal y z
