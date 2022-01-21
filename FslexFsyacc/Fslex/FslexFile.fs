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

    //member this.toFslexDFA() =
    //    let this = this.eliminateHoles()
    //    let patterns = this.rules |> List.map fst
    //    let dfa = DFA.fromRgx patterns
    //    let nextStates : (uint32*(string*uint32)[])[] =
    //        dfa.nextStates
    //        |> Map.toArray
    //        |> Array.map(fun(i,mp)->
    //            let arr = mp |> Map.toArray
    //            i,arr
    //        )
    //    let finalLexemes =
    //        dfa.finalLexemes
    //        |> Array.map(fun(fnls,lxms)->Set.toArray fnls,Set.toArray lxms)

    //    let semantics = this.rules |> List.map snd |> List.toArray

    //    {
    //        nextStates = nextStates 
    //        finalLexemes = finalLexemes
    //        header = this.header
    //        semantics = semantics 
    //    }

    member this.toFslexDFAFile() =
        let this = this.eliminateHoles()
        let patterns = this.rules |> List.map fst
        let dfa = DFA.fromRgx patterns
        let nextStates : (uint32*(string*uint32)[])[] =
            dfa.nextStates
            |> Map.toArray
            |> Array.map(fun(i,mp)->
                let arr = mp |> Map.toArray
                i,arr
            )

        let rules =
            dfa.finalLexemes
            |> Array.mapi(fun i (fnls,lxms)->
                let _,sem = this.rules.[i]
                Set.toArray fnls,Set.toArray lxms,sem)

        {
            header = this.header
            nextStates = nextStates 
            rules = rules 
        }
