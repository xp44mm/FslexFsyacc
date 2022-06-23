module FslexFsyacc.Fsyacc.SemanticGenerator

open FslexFsyacc.Runtime.RenderUtils
open System
open FSharp.Literals
open FSharp.Idioms
open FSharp.Idioms.StringOps

let decorateSemantic (typeAnnotations:Map<string,string>) (prodSymbols:string list) (semantic:string) =
    let bodySymbols =
        prodSymbols
        |> List.tail
        |> List.mapi(fun i s -> i,s)
        |> List.filter(fun(i,s)-> typeAnnotations.ContainsKey s && s <> "unit")

    let mainLines =
        [
            for (i,sym) in bodySymbols do
                if typeAnnotations.ContainsKey sym then
                    $"let s{i} = unbox<{typeAnnotations.[sym]}> ss.[{i}]"
                else 
                    failwith $"type annot `{sym}` is required."

            if typeAnnotations.ContainsKey prodSymbols.Head && 
                typeAnnotations.[prodSymbols.Head] <> "unit" then

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
            if semantic = "" then
                $"{space4 2}null"
            else
                mainLines 
                |> Line.indentCodeBlock (4*2)

        ] |> String.concat Environment.NewLine
    funcDef
