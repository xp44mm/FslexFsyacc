module FslexFsyacc.Program
open FslexFsyacc.VanillaFSharp

open System
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals.Literal
open FSharp.Idioms

[<EntryPoint>]
let main _ =
    let b =
        "<>"
        |> TypeArgumentAngleCompiler.compile 0
    Console.WriteLine(stringify b)
    0
