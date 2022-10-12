module FslexFsyacc.Program 

open System
open System.IO
open System.Collections.Generic

open FSharp.Literals.Literal

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime


let [<EntryPoint>] main _ = 
    let y = 1 |> (~-)
    Console.WriteLine(stringify y)
    0
