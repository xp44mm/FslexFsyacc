module FslexFsyacc.Fsyacc.SemanticGenerator

open System
open FSharp.Idioms

let semanticBody (typeAnnotations:Map<string,string>) (prodSymbols:string list) (semantic:string) =
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

    if semantic = "" then
        "null"
    else
        mainLines 

// 生成semantic函数的定义
let decorateSemantic (typeAnnotations:Map<string,string>) (prodSymbols:string list) (semantic:string) =
    let body = semanticBody typeAnnotations prodSymbols semantic
    let funcDef =
        [
            "fun(ss:obj list)->"
            body 
            |> Line.indentCodeBlock 4
        ] 
        |> String.concat Environment.NewLine
    funcDef
