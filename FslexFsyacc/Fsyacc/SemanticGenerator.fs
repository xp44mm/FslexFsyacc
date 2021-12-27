module FslexFsyacc.Fsyacc.SemanticGenerator

open System.Text.RegularExpressions
open System
open FSharp.Literals
open FSharp.Idioms.StringOps
open FSharp.Idioms

let printProduction (symbols:string list) =
    let symbols =
        symbols
        |> List.map(fun sym ->
            if Regex.IsMatch(sym,@"^\w+$") then
                sym
            else Literal.stringify sym
            )
    sprintf "%s -> %s" symbols.Head (symbols.Tail |> String.concat " ")

let decorateSemantic (typeAnnotations:Map<string,string>) (prodSymbols:string list) (semantic:string) =
    let bodySymbols =
        prodSymbols
        |> List.tail
        |> List.mapi(fun i s -> i,s)
        |> List.filter(fun(i,s)-> typeAnnotations.ContainsKey s && s <> "unit")

    let mainLines =
        [
            for (i,sym) in bodySymbols do
                $"let s{i} = unbox<{typeAnnotations.[sym]}> ss.[{i}]"
            if typeAnnotations.ContainsKey prodSymbols.Head && typeAnnotations.[prodSymbols.Head] <> "unit" then
                $"let result:{typeAnnotations.[prodSymbols.Head]} ="
                semantic |> Line.indentCodeBlock (4)
                $"box result"
            else
                semantic
                "null"
        ]|> String.concat Environment.NewLine

    let funcDef =
        [
            "fun (ss:obj[]) ->"
            $"{space4 2}// {printProduction prodSymbols}"
            if semantic = "" then
                $"{space4 2}null"
            else
                mainLines |> Line.indentCodeBlock (4*2)
        ] |> String.concat Environment.NewLine
    funcDef
