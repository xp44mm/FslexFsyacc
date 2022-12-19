namespace FSharpAnatomy

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.xUnit
open FSharp.Literals

type DirTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``01 - solutionPath test``() =
        show Dir.testPath
        show Dir.solutionPath
        show Dir.FSharpAnatomyPath

    [<Fact>]
    member _.``02 - typeArgument test``() =
        let path = Path.Combine(Dir.FSharpAnatomyPath,"typeArgument.fsyacc")
        show path
        let t = File.Exists(path)
        Should.equal true t
