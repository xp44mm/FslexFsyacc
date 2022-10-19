module FslexFsyacc.Program 

open System
open System.IO
open System.Collections.Generic

open FSharp.Literals.Literal

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals.Literal
open FSharp.Idioms

open System.IO
open System.Text

type TupleBase<'head,'tail> = TupleBase of head:'head* tail:'tail
let x = TupleBase (1,("",(true,(4,()))))
 


let [<EntryPoint>] main _ = 
    let filePath = Path.Combine(Dir.TestPath, @"FSharpGrammar\implementationFile.fsyacc")
    let text = File.ReadAllText(filePath)
    let rawFsyacc = RawFsyaccFile.parse text
    let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    let productions = FsyaccFileShaking.getProductions fsyacc.rules

    let y = FsyaccFileShaking.deepFirstSort productions "implementationFile"

    //Console.WriteLine(stringify y)
    0
