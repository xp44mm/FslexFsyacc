namespace FslexFsyacc.Fslex

open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FslexFsyacc.Lex
open FSharp.xUnit

type FslexDFAAnalyzerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.Parent.FullName
    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fslex")
    let filePath = Path.Combine(sourcePath, @"fslex.fslex")
    let text = File.ReadAllText(filePath)
    let tokens = 
        text
        |> FslexToken.tokenize

    [<Fact>]
    member this.``0 - show tokens``() =
        show (tokens |> Seq.toList)

    [<Fact>]
    member this.``0 - split result``() =
        let y =
            tokens
            |> FslexDFA.analyze
            |> Seq.toList
        show y

    [<Fact>]
    member this.``0 - analyze result``() =
        let y =
            tokens
            |> FslexDFA.analyze
            |> Seq.toList
        show y

    [<Fact>]
    member this.``0 - percent``() =
        let y =
            tokens
            |> FslexDFA.analyze
            |> Seq.toList
        show y





