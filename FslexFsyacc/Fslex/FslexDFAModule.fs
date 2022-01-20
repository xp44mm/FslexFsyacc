namespace FslexFsyacc.Fslex

open System
open FSharp.Literals
open FSharp.Idioms

type FslexDFAModule = 
    {
        nextStates: (uint32*(string*uint32)[])[]
        finalLexemes:(uint32[]*uint32[])[]
        header: string
        semantics: string[] // mappers
    }

    member this.generate(moduleName:string) =
        let mappers = 
            this.semantics
            |> Array.map(fun sem -> LexSemanticGenerator.decorateSemantic sem)

        [
            $"module {moduleName}"
            "let nextStates = " + Literal.stringify this.nextStates
            $"let finalLexemes:(uint32[]*uint32[])[] = {Literal.stringify this.finalLexemes}"
            $"let header = {Literal.stringify this.header}"
            $"let semantics = {Literal.stringify this.semantics}"
            this.header
            "let mappers = [|"
            yield! mappers |> Array.map(Line.indentCodeBlock 4)
            "|]"
            "open FslexFsyacc.Runtime"
            "let analyzer = Analyzer(nextStates, finalLexemes, mappers)"
            "let analyze (tokens:seq<_>) = "
            "    analyzer.analyze(tokens,getTag)"
        ]
        |> String.concat Environment.NewLine
