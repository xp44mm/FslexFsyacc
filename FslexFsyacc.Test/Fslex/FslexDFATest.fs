namespace FslexFsyacc.Fslex

open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FslexFsyacc.Lex
open FSharp.xUnit

type FslexDFATest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.Parent.FullName
    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fslex")
    let filePath = Path.Combine(sourcePath, @"fslex.fslex")
    let text = File.ReadAllText(filePath)
    let fslex = FslexFile.parse text

    [<Fact>]
    member this.``0 - compiler test``() =
        let hdr,dfs,rls = FslexCompiler.parseToStructuralData text
        show hdr
        show dfs
        show rls
        
    [<Fact>] // (Skip="once and for all!")
    member this.``1 - generate DFA``() =
        let name = "FslexDFA2"
        let moduleName = $"FslexFsyacc.Fslex.{name}"

        let y = fslex.toFslexDFA()
        let result = y.generate(moduleName)

        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir, result)
        output.WriteLine("output lex:" + outputDir)

    [<Fact>]
    member this.``2 - valid DFA``() =
        let y = fslex.toFslexDFA()

        Should.equal y.dfa.nextStates       FslexDFA2.nextStates
        Should.equal y.dfa.lexemesFromFinal FslexDFA2.lexemesFromFinal
        Should.equal y.dfa.universalFinals  FslexDFA2.universalFinals
        Should.equal y.dfa.indicesFromFinal FslexDFA2.indicesFromFinal
        Should.equal y.header               FslexDFA2.header
        Should.equal y.semantics            FslexDFA2.semantics

