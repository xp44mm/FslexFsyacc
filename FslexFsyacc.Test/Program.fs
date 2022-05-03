module Program 

open System.IO
open FslexFsyacc.Fsyacc
open System
open FSharp.Literals
open FslexFsyacc.Yacc
open Interpolation
open Interpolation.PlaceholderUtils
open FslexFsyacc.Runtime

let [<EntryPoint>] main _ = 
    let inp = "2 + 3}1"
    let expr,rest = ExpressionTaker.compile inp
    Console.WriteLine($"{expr},{Literal.stringify rest}")
    0
