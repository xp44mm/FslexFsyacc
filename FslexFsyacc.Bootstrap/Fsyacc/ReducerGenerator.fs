﻿module FslexFsyacc.Fsyacc.ReducerGenerator

open System
open FSharp.Idioms
open FslexFsyacc.TypeArguments

let reducerBody (typeAnnotations:Map<string,TypeArgument>) (production:string list) (reducer:string) =
    let bodySymbols =
        production
        |> List.tail
        |> List.mapi(fun i s -> i,s)
        |> List.filter(fun(i,s)-> 
            typeAnnotations.ContainsKey s && 
            typeAnnotations.[s] <> Ctor(["unit"],[]))

    let mainLines =
        [
            for (i,sym) in bodySymbols do
                if typeAnnotations.ContainsKey sym then
                    $"let s{i} = unbox<{typeAnnotations.[sym].toCode()}> ss.[{i}]"
                else 
                    failwith $"type annot `{sym}` is required."

            if typeAnnotations.ContainsKey production.Head && 
                typeAnnotations.[production.Head] <> Ctor(["unit"],[]) then

                $"let result:{typeAnnotations.[production.Head].toCode()} ="
                reducer |> Line.indentCodeBlock (4)
                $"box result"
            else
                reducer
                "null"
        ]
        |> String.concat Environment.NewLine

    if production.Head = "" then
        "ss.[0]"
    elif reducer = "" then
        "null"
    else
        mainLines

// 生成semantic函数的定义
let decorateReducer (typeAnnotations:Map<string,TypeArgument>) (production: string list) (reducer:string) =
    let body = reducerBody typeAnnotations production reducer
    let funcDef =
        [
            "fun(ss:obj list)->"
            body |> Line.indentCodeBlock 4
        ] 
        |> String.concat Environment.NewLine
    funcDef

