namespace FslexFsyacc.Fsyacc
open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals.Literal
open FSharp.Idioms

open System.IO
open System.Text


type FsyaccFileShakingTest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

