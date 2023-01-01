module FslexFsyacc.Program 

open System
open System.IO

open FSharp.Literals.Literal
open FslexFsyacc.Yacc
open System.Collections.Generic

let x<'a when 'a : comparison > (z:'a) = ()

let [<EntryPoint>] main _ = 
    Console.WriteLine(stringify "")
    
    0
