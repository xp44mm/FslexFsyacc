namespace FslexFsyacc.Fsyacc
open FslexFsyacc

open System.IO

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms.Literal

type SourceTextTest(output:ITestOutputHelper) =
    [<Fact>]
    member _.``tryWord``() =
        let x = "xyz"
        let y = 
            SourceText.tryWord x 
            |> fun m -> m.Value.Value
        Should.equal y x

    [<Fact>]
    member _.``tryWS``() =
        let x = "  "
        let y = 
            SourceText.tryWS x 
            |> fun m -> m.Value.Value

        Should.equal y x

