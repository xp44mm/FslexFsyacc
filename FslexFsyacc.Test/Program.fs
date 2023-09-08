module FslexFsyacc.Program
open FslexFsyacc.VanillaFSharp

open System
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals.Literal
open FSharp.Idioms

open FslexFsyacc.Fsyacc

let x =id<RawFsyaccFile>{header= "";rules= ["expr",[["expr";"+";"expr"],"","s0 + s2";["expr";"-";"expr"],"","s0 - s2";["expr";"*";"expr"],"","s0 * s2";["expr";"/";"expr"],"","s0 / s2";["(";"expr";")"],"","s1";["-";"expr"],"UMINUS","-s1";["NUMBER"],"","s0"]];precedences= ["left",["+";"-"];"left",["*";"/"];"right",["UMINUS"]];declarations= ["float",["NUMBER";"expr"]]}


[<EntryPoint>]
let main _ =
    let b =
        "<>"
        |> TypeArgumentAngleCompiler.compile 0
    Console.WriteLine(stringify b)
    0
