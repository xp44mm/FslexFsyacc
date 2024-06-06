module FslexFsyacc.Program
open FslexFsyacc.VanillaFSharp

open System
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Idioms.Literal
open FSharp.Idioms

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open System.Reflection
open FslexFsyacc
open FslexFsyacc.ItemCores
open FslexFsyacc.Expr

[<EntryPoint>]
let main _ =
    let ls = [
        "%type"
        "%type<_>"
        "%type <int>"
        "%type <seq<float*string>> starts"
    ]

    let y = 
        FsyaccToken2Utils.tokenize 0 ls.[1]
        |> Seq.map(fun tok ->
            Console.WriteLine(stringify tok)
        )
        |> Seq.toList
    Console.WriteLine(stringify y)

    0
