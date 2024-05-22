module FslexFsyacc.Runtime.Grammars.ProductionUtils

open FSharp.Idioms.Literal

//从文法生成增广文法
let augment (input:list<list<string>>) =
    let repl =
        input
        |> List.groupBy id
        |> List.map snd
        |> List.filter(fun ps -> ps.Length > 1 )

    if repl.IsEmpty then
        let s0 = input.[0].[0]
        let p0 = ["";s0]
        (p0::input) 
        |> Set.ofList
    else
        let repl =
            repl
            |> List.map List.head
        failwith $"重复的产生式:{stringify repl}"
