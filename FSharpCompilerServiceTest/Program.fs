module FSharpCompilerServiceTest.Program 

module Result =

  let get =
    function
    | Ok x -> x
    | Error e -> failwithf "%A" e

open FSharp.Compiler.Syntax
open FSharp.Compiler.Text
open FSharp.Compiler.CodeAnalysis
open System
open FSharp.Literals.Literal
open System.IO

let sourcePath = Path.Combine(Dir.projPath, "Fsyacc")
let filePath = Path.Combine(sourcePath, "FsyaccParseTableFile.fs")
let text = File.ReadAllText(filePath)

let [<EntryPoint>] main _ = 
    let root:SynModuleOrNamespace =
        text
        |> Tests.tryParseExpression
        |> Async.RunSynchronously
        |> Result.get

    match root with
    | SynModuleOrNamespace(a,b,c,d,e,f,g,h) ->
        d
        |> List.iter(fun x ->
            Console.WriteLine(stringify x)
        )


    0
