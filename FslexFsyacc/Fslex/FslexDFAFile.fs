namespace FslexFsyacc.Fslex

open FslexFsyacc.Lex
open FSharp.Literals
open System

type FslexDFAFile = 
    {
        dfa:DFA<string>
        header:string
        semantics: string list
    }

    member this.generate(moduleName:string) =
        let dfa = this.dfa
        [
            $"module {moduleName}"
            "let nextStates = " + Literal.stringify dfa.nextStates
            "let lexemesFromFinal = " + Literal.stringify dfa.lexemesFromFinal
            "let universalFinals = " + Literal.stringify dfa.universalFinals
            "let indicesFromFinal = " + Literal.stringify dfa.indicesFromFinal
            $"let header = {Literal.stringify this.header}"
            $"let semantics = {Literal.stringify this.semantics}"
            this.header
            "let mappers = [|"
            for fn in this.semantics do
                LexSemanticGenerator.decorateSemantic fn
            "|]"
            "let finalMappers ="
            "    indicesFromFinal"
            "    |> Map.map(fun _ i -> mappers.[i])"
            "open FslexFsyacc.Runtime"
            "let analyzer = Analyzer(nextStates, lexemesFromFinal, universalFinals, finalMappers)"
            "let analyze (tokens:seq<_>) = "
            "    analyzer.analyze(tokens,getTag)"
        ]
        |> String.concat Environment.NewLine
