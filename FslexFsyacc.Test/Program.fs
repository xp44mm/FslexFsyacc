module Program 

open System
open System.IO
open System.Collections.Generic

open FslexFsyacc.Fsyacc
open FSharp.Literals
open FslexFsyacc.Yacc
open Interpolation
open Interpolation.PlaceholderUtils
open FslexFsyacc.Runtime

let [<EntryPoint>] main _ = 
    let states = Stack<int>()
    let trees = Stack<obj>()

    //let expr,rest = ExpressionTaker.compile inp
    //Console.WriteLine($"{expr},{Literal.stringify rest}")
    0
