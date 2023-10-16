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
    member _.``00 - rawToFlatRules``() =
        let rawRules = [
            "AsyncArrowFunction",[
                ["async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
                ["CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""]]
        let y = RuleListUtils.ofRaw rawRules
        let e = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""
            ]
        //show y
        Should.equal y e

    [<Fact>]
    member _.``11 - flatToRawRules``() =
        let x = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""
            ]
        let y = RuleListUtils.toRaw x

        let z = [
            "AsyncArrowFunction",[
                ["async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
                ["CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""]]
        //show y
        Should.equal y z

    [<Fact>]
    member _.``12 - addRule``() =
        let x = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""
            ]
        let y = ["AsyncArrowFunction";"async";"ArrowFormalParameters";"=>";"AsyncConciseBody"]
        let z = x |> RuleListUtils.addRule(y,"","")

        let e = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""
            ["AsyncArrowFunction";"async";"ArrowFormalParameters";"=>";"AsyncConciseBody"],"","";
            ]

        //show z

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
            |> RuleListUtils.removeRule oldProd

        //show y
        let x = [
            ["AsyncArrowFunction";"async";"ArrowFormalParameters";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"",""
            ]

        Should.equal y x

    [<Fact>]
    member _.``14 - replaceRule``() =
        let rules = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"",""
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"",""
            ]

        let oldProd = ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"]
        let newProd = ["AsyncArrowFunction";"async";"ArrowFormalParameters";"=>";"AsyncConciseBody"]

        let y = 
            rules 
            |> RuleListUtils.replaceRule oldProd (newProd,"","")
        //show y
        let e = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"",""
            ["AsyncArrowFunction";"async";"ArrowFormalParameters";"=>";"AsyncConciseBody"],"",""
            ]
        Should.equal e y

    [<Fact>]
    member _.``16 - findRuleByName``() =
        let rules = [
            ["AsyncArrowFunction";"async";"AsyncArrowBindingIdentifier";"=>";"AsyncConciseBody"],"","";
            ["AsyncArrowFunction";"CoverCallExpressionAndAsyncArrowHead";"=>";"AsyncConciseBody"],"cover",""
            ]

        let y = 
            rules
            |> RuleListUtils.findRuleByDummyToken "cover"

        Should.equal y rules.[1]


