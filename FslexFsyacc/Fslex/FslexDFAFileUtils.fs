﻿module FslexFsyacc.Fslex.FslexDFAFileUtils

open System
open FSharp.Idioms

let generate(moduleName:string) (this:FslexDFAFile) =
    let fxRules = 
        this.rules
        |> List.map(fun (f,l,g) -> 
            let fn = LexSemanticGenerator.decorateSemantic g
            LexSemanticGenerator.renderRule f l fn
            )
        |> String.concat Environment.NewLine

    [
        $"module {moduleName}"
        $"let nextStates = {Literal.stringify this.nextStates}"
        this.header
        "let rules:list<uint32 list*uint32 list*(list<token>->_)> = ["
        fxRules |> Line.indentCodeBlock 4
        "]"
        "let analyzer = Analyzer<_,_>(nextStates, rules)"
    ]
    |> String.concat Environment.NewLine

// print action code list
let generateMappers(this:FslexDFAFile) =
    this.rules
    |> List.map(fun(_,_,semantic) ->
        LexSemanticGenerator.decorateSemantic semantic
        )
    |> String.concat Environment.NewLine
