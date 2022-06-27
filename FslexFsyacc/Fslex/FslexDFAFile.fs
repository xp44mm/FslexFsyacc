namespace FslexFsyacc.Fslex

open System
open FSharp.Literals
open FSharp.Idioms

type FslexDFAFile = 
    {
        header: string
        nextStates: (uint32*(string*uint32)[])[]
        rules: (uint32[]*uint32[]*string)[]
    }

    member this.generate(moduleName:string) =
        let fxRules = 
            this.rules
            |> Array.map(fun (f,l,g) -> 
                let fn = LexSemanticGenerator.decorateSemantic g
                LexSemanticGenerator.renderRule f l fn
                )
            |> String.concat Environment.NewLine

        [
            $"module {moduleName}"
            //$"let header = {Literal.stringify this.header}"
            $"let nextStates = {Literal.stringify this.nextStates}"
            //$"let rules:(uint32[]*uint32[]*string)[] = {Literal.stringify this.rules}"
            this.header
            "let rules:(uint32[]*uint32[]*_)[] = [|"
            fxRules |> Line.indentCodeBlock 4
            "|]"
            //"open FslexFsyacc.Runtime"
            "let analyzer = Analyzer(nextStates, rules)"
            "let analyze (tokens:seq<_>) = "
            "    analyzer.analyze(tokens,getTag)"
        ]
        |> String.concat Environment.NewLine

    member this.generateMappers() =
        this.rules
        |> Array.map(fun(_,_,semantic) ->
            LexSemanticGenerator.decorateSemantic semantic
            )
        |> String.concat Environment.NewLine
