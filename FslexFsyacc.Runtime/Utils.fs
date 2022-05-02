module FslexFsyacc.Runtime.RenderUtils

open System.Text.RegularExpressions
open FSharp.Literals

let renderSymbol sym =
    if Regex.IsMatch(sym,@"^\w+$") then
        sym
    else Literal.stringify sym

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
        [|
            yield! ls1
            yield "@"
            yield! ls2
        |] |> String.concat " "

    sprintf "%s -> %s" symbols.Head body

let renderEntry (prod:string list) (dot:int) (lookaheads:string[]) = 
    if lookaheads.Length = 0 then
        renderItemCore prod dot
    else
        [
            renderProduction prod 
            Literal.stringify (List.ofArray lookaheads)
        ]
        |> String.concat " "

let renderClosure (closure: (string list*int*string[])[]) =
    seq {
        for (prod,dot,las) in closure ->
            renderEntry prod dot las
    }
    |> String.concat System.Environment.NewLine