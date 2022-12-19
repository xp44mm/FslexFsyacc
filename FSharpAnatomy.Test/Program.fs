module FSharpAnatomy.Program 

open System
open System.IO

open FSharp.Literals.Literal
open FslexFsyacc.Yacc

let [<EntryPoint>] main _ = 
    let x = "int[,]list"
    let y = 
        x 
        |> FSharpTokenUtils.tokenize
        
        //|> TypeArgumentParser.parse

    for t in y do
        Console.WriteLine(stringify t)
    
    0
