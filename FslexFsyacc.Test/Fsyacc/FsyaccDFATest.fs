namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open System.IO

open FSharp.xUnit
open FSharp.Literals
open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Fslex

type FsyaccDFATest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.Parent.FullName
    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fsyacc")
    let filePath = Path.Combine(sourcePath, @"fsyacc.fslex")
    let text = File.ReadAllText(filePath)
    let fslex = FslexFile.parse text

    [<Fact(Skip="once and for all!")>] // 
    member this.``1 - generate DFA``() =
        let y = fslex.toFslexDFA()

        let name = "FsyaccDFA"
        let moduleName = $"FslexFsyacc.Fsyacc.{name}"

        let result = y.generate(moduleName)
        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir, result)
        output.WriteLine("output lex:" + outputDir)

    [<Fact>]
    member this.``2 - valid DFA``() =
        let y = fslex.toFslexDFA()

        Should.equal y.dfa.nextStates       FsyaccDFA.nextStates
        Should.equal y.dfa.lexemesFromFinal FsyaccDFA.lexemesFromFinal
        Should.equal y.dfa.universalFinals  FsyaccDFA.universalFinals
        Should.equal y.dfa.indicesFromFinal FsyaccDFA.indicesFromFinal
        Should.equal y.header               FsyaccDFA.header
        Should.equal y.semantics            FsyaccDFA.semantics



