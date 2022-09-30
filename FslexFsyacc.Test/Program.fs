module FslexFsyacc.Program 

open System
open System.IO
open System.Collections.Generic

open FslexFsyacc.Fsyacc
open FSharp.Literals
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime

let hehe =
    let mutable x = 1
    // The subsequent token occurs on the same line. 
    x <- 
        printfn "hello"; 
        2 + 2   


let [<EntryPoint>] main _ = 

    0
