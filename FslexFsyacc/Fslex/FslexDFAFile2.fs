namespace FslexFsyacc.Fslex

open System
open FSharp.Literals

type FslexDFAFile2 = 
    {
        nextStates      : (uint32*(string*uint32)[])[]
        lexemesFromFinal: (uint32*uint32[])[]
        universalFinals : uint32[]
        indicesFromFinal: (uint32*int)[]

        header:string
        semantics: string[]
    }

    member this.generate(moduleName:string) =
        [
            $"module {moduleName}"
            "let nextStates = " + Literal.stringify this.nextStates
            "let lexemesFromFinal = " + Literal.stringify this.lexemesFromFinal
            "let universalFinals = " + Literal.stringify this.universalFinals
            "let indicesFromFinal = " + Literal.stringify this.indicesFromFinal
            $"let header = {Literal.stringify this.header}"
            $"let semantics = {Literal.stringify this.semantics}"
            this.header
            "let mappers = [|"
            for fn in this.semantics do
                LexSemanticGenerator.decorateSemantic fn
            "|]"
            "let finalMappers ="
            "    indicesFromFinal"
            "    |> Array.map(fun(fnl, i) -> fnl,mappers.[i])"
            "open FslexFsyacc.Runtime"
            "let analyzer = Analyzer2(nextStates, lexemesFromFinal, universalFinals, finalMappers)"
            "let analyze (tokens:seq<_>) = "
            "    analyzer.analyze(tokens,getTag)"
        ]
        |> String.concat Environment.NewLine
