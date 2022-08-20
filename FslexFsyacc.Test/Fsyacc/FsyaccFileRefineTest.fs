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

