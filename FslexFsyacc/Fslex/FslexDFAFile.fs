namespace FslexFsyacc.Fslex

open System
open FSharp.Literals
open FSharp.Idioms

type FslexDFAFile = 
    {
        header: string
        nextStates: (uint32*(string*uint32)[])[]
        rules:(uint32[]*uint32[]*string)[]

    }

    member this.generate(moduleName:string) =
        let rules = 
            this.rules
            |> Array.map(fun (f,l,g) -> 
                let fn = LexSemanticGenerator.decorateSemantic g
                LexSemanticGenerator.renderRule f l fn
                )
            |> String.concat Environment.NewLine

        [
            $"module {moduleName}"
            $"let header = {Literal.stringify this.header}"
            $"let nextStates = {Literal.stringify this.nextStates}"
            $"let rules:(uint32[]*uint32[]*string)[] = {Literal.stringify this.rules}"
            this.header
            "let fRules:(uint32[]*uint32[]*_)[] = [|"
            rules |> Line.indentCodeBlock 4
            "|]"

            "open FslexFsyacc.Runtime"
            "let analyzer = Analyzer(nextStates, fRules)"
            "let analyze (tokens:seq<_>) = "
            "    analyzer.analyze(tokens,getTag)"
        ]
        |> String.concat Environment.NewLine
