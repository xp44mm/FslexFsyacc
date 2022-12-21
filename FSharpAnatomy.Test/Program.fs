module FSharpAnatomy.Program 

open System
open System.IO

open FSharp.Literals.Literal
open FslexFsyacc.Yacc

let [<EntryPoint>] main _ = 
    Console.WriteLine(stringify 0)
    0
