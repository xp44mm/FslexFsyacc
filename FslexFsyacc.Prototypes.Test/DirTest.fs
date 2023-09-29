namespace FslexFsyacc.Prototypes

open Xunit
open Xunit.Abstractions

open System.Reflection

open FSharp.Reflection
open FSharp.xUnit

open FSharp.Idioms
open FSharp.Literals
open FSharp.Literals.Literal

type DirTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``01 - testProjPath test``() =
        output.WriteLine(Dir.testProjPath)

    [<Fact>]
    member _.``02 - solutionPath test``() =
        output.WriteLine(Dir.solutionPath)

    [<Fact>]
    member _.``03 - crewProjPath test``() =
        output.WriteLine(Dir.crewProjPath)

    [<Fact>]
    member _.``04 - dllFilePath test``() =
        let path = @"D:\Application Data\GitHub\xp44mm\FslexFsyacc\FslexFsyacc.Prototypes\bin\Release\net6.0\FslexFsyacc.Prototypes.dll"
        Should.equal Dir.dllFilePath path

