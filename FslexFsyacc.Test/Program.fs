module Program 

open System.IO
open FslexFsyacc.Fsyacc
open System
open FSharp.Literals

let tokenizeTest() =
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"Expr\expr.fsyacc")
    let text = File.ReadAllText(filePath)
    let y = 
        FsyaccTokenUtils.tokenize text
        |> Seq.filter(fun (pos,len,token)->
            match token with
            | HEADER _ 
            | SEMANTIC _
                -> true
            | _ -> false
        )
        |> Seq.toArray
    Console.WriteLine(Literal.stringify y)

let [<EntryPoint>] main _ = 
    tokenizeTest()
    0
