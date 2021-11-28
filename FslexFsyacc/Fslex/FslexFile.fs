namespace FslexFsyacc.Fslex
open System

open FslexFsyacc.Lex
open FSharp.Literals

type FslexFile = 
    {
        header:string
        definitions: (string*RegularExpression<string>)list
        rules: (RegularExpression<string>list*string)list

    }

    /// parse from file input to structural data
    static member parse(fslex:string) =
        let header,definitions, rules = FslexCompiler.parseToStructuralData fslex
        let uninameset = definitions |> List.map fst |> Set.ofList
        let usednames = FslexCompiler.getUsedNames(definitions, rules)
        let unused = uninameset - usednames
        let undeclared = usednames - uninameset

        if unused |> Set.isEmpty |> not then
            let unused = Set.toList unused
            failwith <| "unused definitions:" + Literal.stringify unused

        if undeclared |> Set.isEmpty |> not then
            let undeclared = Set.toList undeclared
            failwith <| "undeclared definitions:" + Literal.stringify undeclared

        {
            header = header
            definitions = definitions
            rules = rules
        }

    member this.eliminateHoles() =
        let patterns = this.rules |> List.map fst
        let patterns = LexFileNormalization.normRules this.definitions patterns
        let semantics = this.rules |> List.map snd
        let rules = List.zip patterns semantics
        {
            header = this.header
            definitions = []
            rules = rules
        }

    member this.toFslexDFA() =
        let this = this.eliminateHoles()
        let patterns = this.rules |> List.map fst
        let semantics = this.rules |> List.map snd
        let dfa = DFA.fromRgx patterns

        {
            header = this.header
            dfa = dfa
            semantics = semantics 
        }
