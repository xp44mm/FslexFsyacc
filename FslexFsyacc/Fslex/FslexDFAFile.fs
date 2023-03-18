namespace FslexFsyacc.Fslex

open System
open FSharp.Literals
open FSharp.Idioms

type FslexDFAFile = 
    {
        header: string
        rules: (uint32 list*uint32 list*string)list
        nextStates: (uint32*(string*uint32)list)list
    }

    member this.generate(moduleName:string) =
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
    member this.generateMappers() =
        this.rules
        |> List.map(fun(_,_,semantic) ->
            LexSemanticGenerator.decorateSemantic semantic
            )
        |> String.concat Environment.NewLine
