namespace PolynomialExpressions

open System
open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open FslexFsyacc.Fslex

type TermDFATest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"term.fslex")
    let text = File.ReadAllText(filePath)
    let fslex = FslexFile.parse text

    [<Fact>]
    member this.``0 - compiler test``() =
        let tokens = 
            text
            |> FslexTokenUtils.tokenize
            |> FslexDFA.analyze
            |> Seq.concat
            |> List.ofSeq
        show tokens

    [<Fact (Skip="once and for all!")>] //
    member this.``1 - generate DFA``() =
        let name = "TermDFA"
        let moduleName = $"PolynomialExpressions.{name}"

        let dfafile = fslex.toFslexDFA()
        let result = dfafile.generate(moduleName)
        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.fs")

        File.WriteAllText(outputDir, result)
        output.WriteLine("output lex:" + outputDir)

    [<Fact>]
    member this.``2 - valid DFA``() =
        let y = fslex.toFslexDFA()

        Should.equal y.dfa.nextStates       TermDFA.nextStates
        Should.equal y.dfa.lexemesFromFinal TermDFA.lexemesFromFinal
        Should.equal y.dfa.universalFinals  TermDFA.universalFinals
        Should.equal y.dfa.indicesFromFinal TermDFA.indicesFromFinal
        Should.equal y.header               TermDFA.header
        Should.equal y.semantics            TermDFA.semantics

    [<Fact>]
    member this.``3 - tokenize``() =
        let y = 
            text
            |> FslexTokenUtils.tokenize 
            |> Seq.filter(fun (pos,len,token)->
                match token with
                | HEADER _ 
                | SEMANTIC _
                    -> true
                | _ -> false
            )
            |> Seq.toList
        show y

