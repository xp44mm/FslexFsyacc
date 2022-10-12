module FslexFsyacc.Runtime.RenderUtils

open System
open System.Text.RegularExpressions
open FSharp.Literals

let renderSymbol sym =
    if Regex.IsMatch(sym,@"^\w+$") then
        sym
    else Literal.stringify sym

let renderQuantifySymbol sym =
    let rgx = Regex(@"^(.+)\{\\([?+*])\}$")
    let mat = rgx.Match(sym)
    if mat.Success then
        let m = mat.Groups.[1].Value
        let q = mat.Groups.[2].Value
        $"{renderSymbol m}{q}"
    else
        renderSymbol sym

let renderProduction (symbols:string list) =
    let symbols =
        symbols
        |> List.map renderSymbol
    sprintf "%s -> %s" symbols.Head (symbols.Tail |> String.concat " ")

let renderItemCore (prod:string list) dot =
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

    sprintf "%s -> %s" symbols.Head body

let renderEntry (prod:string list) (dot:int) (lookaheads:string list) = 
    if lookaheads.Length = 0 then
        renderItemCore prod dot
    else
        [
            renderItemCore prod dot
            Literal.stringify lookaheads
        ]
        |> String.concat " "

let renderClosure (closure: (string list*int*string list)list) =
    [
        for (prod,dot,las) in closure ->
            renderEntry prod dot las
    ]
    |> String.concat Environment.NewLine
