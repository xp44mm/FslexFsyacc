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
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Expr

[<EntryPoint>]
let main _ =
    let inp = "2 + 3"
    let y = ExprCompiler.compile inp

    0
