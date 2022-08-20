namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit

type FsyaccFileRulesTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``00 - rawToNormRules``() =
        let rawRules = [
            "AsyncArrowFunction",[
                ["async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
                ["CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""]]
        let y = FsyaccFileRules.rawToNormRules rawRules
        let e = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""
            ]
        show y
        Should.equal y e

    [<Fact>]
    member _.``11 - normToRawRules``() =
        let x = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""
            ]
        let y = FsyaccFileRules.normToRawRules x

        let z = [
            "AsyncArrowFunction",[
                ["async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
                ["CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""]]
        show y
        Should.equal y z

    [<Fact>]
    member _.``12 - addRule``() =
        let x = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""
            ]
        let y = ["AsyncArrowFunction";"async";"ArrowFormalParameters";"=>";"AsyncConciseBody"]
        let z = x |> FsyaccFileRules.addRule(y,"","")

        let e = [
            ["AsyncArrowFunction";"async";"ArrowFormalParameters";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""
            ]

        show z

        Should.equal z e

    [<Fact>]
    member _.``13 - removeRule``() =
        let e = [
            ["AsyncArrowFunction";"async";"ArrowFormalParameters";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""
            ]

        let oldProd = ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"]

        let y = 
            e 
            |> FsyaccFileRules.removeRule oldProd

        show y
        let x = [
            ["AsyncArrowFunction";"async";"ArrowFormalParameters";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"",""
            ]

        Should.equal y x
    [<Fact>]
    member _.``14 - replaceRule``() =
        let rules = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""
            ]

        let oldProd = ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"]
        let newProd = ["AsyncArrowFunction";"async";"ArrowFormalParameters";"=>";"AsyncConciseBody"]

        let y = 
            rules 
            |> FsyaccFileRules.replaceRule oldProd (newProd,"","")
        show y
        let e = [
            ["AsyncArrowFunction";"async";"ArrowFormalParameters";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"",""
            ]
        Should.equal e y
    [<Fact>]
    member _.``15 - findRule``() =
        let rules = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""
            ]

        let oldProd = ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"]
        let y = 
            rules
            |> FsyaccFileRules.findRule oldProd

        Should.equal y rules.[1]

    [<Fact>]
    member _.``16 - findRuleByName``() =
        let rules = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"cover",""
            ]

        let y = 
            rules
            |> FsyaccFileRules.findRuleByName "cover"

        Should.equal y rules.[1]


