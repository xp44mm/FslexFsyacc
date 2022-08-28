namespace FslexFsyacc.Fslex

open FslexFsyacc.Lex
open FSharp.Literals

type FslexFile = 
    {
        header:string
        definitions: (string*RegularExpression<string>)list
        rules: (RegularExpression<string>list*string)list
    }

    member this.getRegularExpressions() =
        [
            for (_,re) in this.definitions do
                yield re
            for (res,_) in this.rules do
                yield! res
        ]

    /// parse from file input to structural data
    static member parse(fslex:string) =
        let header,definitions,rules = FslexCompiler.parseToStructuralData fslex

        {
            header = header
            definitions = definitions
            rules = rules
        }

    member this.verify() =
        let uninameset = this.definitions |> List.map fst |> Set.ofList
        let usednames = FslexCompiler.getUsedNames(this.definitions,this.rules)
        let unused = uninameset - usednames
        let undeclared = usednames - uninameset

        if unused |> Set.isEmpty |> not then
            let unused = Set.toList unused
            failwith <| "unused definitions:" + Literal.stringify unused

        if undeclared |> Set.isEmpty |> not then
            let undeclared = Set.toList undeclared
            failwith <| "undeclared definitions:" + Literal.stringify undeclared

        {|
            unused = unused
            undeclared = undeclared
        |}

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

    member this.toFslexDFAFile() =
        let this = this.eliminateHoles()
        let patterns = this.rules |> List.map fst
        let dfa = DFA.fromRgx patterns
        let nextStates : (uint32*(string*uint32)list)list =
            dfa.nextStates
            |> Map.toList
            |> List.map(fun(i,mp)->
                let arr = mp |> Map.toList
                i,arr
            )

        let rules =
            dfa.finalLexemes
            |> List.mapi(fun i (fnls,lxms)->
                let _,sem = this.rules.[i]
                Set.toList fnls,Set.toList lxms,sem)

        {
            header = this.header
            nextStates = nextStates 
            rules = rules 
        }
