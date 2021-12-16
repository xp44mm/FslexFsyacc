module FslexFsyacc.Fsyacc.SemanticGenerator

open System.Text.RegularExpressions
open System
open FSharp.Literals
open FSharp.Idioms.StringOps

let printProduction (symbols:string list) =
    let symbols =
        symbols
        |> List.map(fun sym ->
            if Regex.IsMatch(sym,@"^\w+$") then
                sym
            else Literal.stringify sym
            )
    sprintf "%s : %s" symbols.Head (symbols.Tail |> String.concat " ")

let decorateSemantic (typeAnnotations:Map<string,string>) (prodSymbols:string list) (semantic:string) =
    let bodySymbols =
        prodSymbols
        |> List.tail
        |> List.mapi(fun i s -> i,s)
        |> List.filter(fun(i,s)-> typeAnnotations.ContainsKey s)

    [
        "fun (ss:obj[]) ->"
        $"{indent 2}// {printProduction prodSymbols}"
        for (i,sym) in bodySymbols do
            $"{indent 2}let s{i} = unbox<{typeAnnotations.[sym]}> ss.[{i}]"
        $"{indent 2}let result:{typeAnnotations.[prodSymbols.Head]} ="
        semantic |> indentCodeBlock (3*4)
        $"{indent 2}box result"
    ] |> String.concat Environment.NewLine

