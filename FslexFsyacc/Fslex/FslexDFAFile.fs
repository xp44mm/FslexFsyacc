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

    member this.generateModule(moduleName:string) =
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
            for m in this.semantics do
                LexSemanticGenerator.decorateSemantic m
            "|]"
            "open FslexFsyacc.Runtime"
            "let analyzer = LexicalAnalyzer(nextStates, lexemesFromFinal, universalFinals, indicesFromFinal, mappers)"
            "let split (tokens:seq<_>) = "
            "    analyzer.split(tokens,getTag)"
        ]
        |> String.concat Environment.NewLine

