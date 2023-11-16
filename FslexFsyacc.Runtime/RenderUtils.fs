module FslexFsyacc.Runtime.RenderUtils

open System
open System.Text.RegularExpressions
open FSharp.Idioms

let renderSymbol (sym:string) =
    if Regex.IsMatch(sym,@"^_*$") then
        $"\"{sym}\""
    elif Regex.IsMatch(sym,@"^\w+$") then
        sym
    elif sym.Length > 1 && sym.[0] = '{' && sym.[sym.Length-1] = '}' then
        sym.[1..sym.Length-2]
    else
        sym |> Literal.stringify

let renderProduction (symbols:string list) =
    let symbols =
        symbols
        |> List.map renderSymbol
    let body = symbols.Tail |> String.concat " "
    $"{symbols.Head} : {body}"

let renderItemCore (prod:string list) (dot:int) =
    let symbols =
        prod
        |> List.map(renderSymbol)

    let body =
        let ls1,ls2 =
            symbols.Tail
            |> List.splitAt dot
        [
            yield! ls1
            yield "@"
            yield! ls2
        ] |> String.concat " "

    sprintf "%s : %s" symbols.Head body

let renderEntry (prod:string list) (dot:int) (lookaheads:seq<string>) =
    if lookaheads |> Seq.isEmpty then
        renderItemCore prod dot
    else
        let lookaheadsPart =
            lookaheads
            |> Seq.map renderSymbol
            |> String.concat " "
        [
            renderItemCore prod dot
            lookaheadsPart
        ]
        |> String.concat " / "

let renderClosure (closure: (string list*int*string list)list) =
    [
        for (prod,dot,las) in closure ->
            renderEntry prod dot las
    ]
    |> String.concat Environment.NewLine
