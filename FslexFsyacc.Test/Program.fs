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

let x = set [
    ["expr";"-";"expr"];
    ["expr";"expr";"*";"expr"];
    ["expr";"expr";"+";"expr"];
    ["expr";"expr";"-";"expr"];
    ["expr";"expr";"/";"expr"]]

[<EntryPoint>]
let main _ =
    0
