module FslexFsyacc.Program 

open System
open System.IO
open System.Collections.Generic

open FslexFsyacc.Fsyacc
open FSharp.Literals
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime


let [<EntryPoint>] main _ = 
    // groupby ±£ÁôÔ­Ê¼Ë³Ðò
    let src = [1,1;2,1;3,1;2,2;3,2;4,1]

    let tgt = 
        src
        |> List.groupBy fst
        |> List.map (fun(a,b)->
            let b = b |> List.map snd
            a,b
            )

    Console.WriteLine(Literal.stringify tgt)
    0
