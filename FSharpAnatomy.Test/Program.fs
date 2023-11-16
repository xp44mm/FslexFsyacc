module FSharpAnatomy.Program 

open System
open System.IO

open FSharp.Idioms

open FSharp.Idioms.Literal
open FslexFsyacc.Yacc


let [<EntryPoint>] main _ = 
    let x = "<'T,'U when 'T : equality and 'U : equality>"
    let y = 
        PostfixTyparDeclsUtils.tokenize 0 x
        |> Seq.map(fun tok ->
            Console.WriteLine(stringify tok)
        )
        |> Seq.toList

    //show (x,y)
    //Console.WriteLine(stringify 0)
    0
