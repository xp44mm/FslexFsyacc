namespace PolynomialExpressions

open System
open System.IO
open System.Text

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
    member _.``0 = compiler test``() =
        let hdr,dfs,rls = FslexCompiler.compile text
        show hdr
        show dfs
        show rls
        
    [<Fact>]
    member _.``1 = verify``() =
        let y = fslex.verify()

        Assert.True(y.undeclared.IsEmpty)
        Assert.True(y.unused.IsEmpty)

    [<Fact>]
    member _.``2 = universal characters``() =
        let res = fslex.getRegularExpressions()

        let y = 
            res
            |> List.collect(fun re -> re.getCharacters())
            |> Set.ofList
        show y

    [<Fact(Skip="once and for all!")>] // 
    member _.``3 = generate DFA``() =
        let name = "TermDFA"
        let moduleName = $"PolynomialExpressions.{name}"

        let dfafile = fslex.toFslexDFAFile()
        let result = dfafile.generate(moduleName)
        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.fs")

        File.WriteAllText(outputDir, result,System.Text.Encoding.UTF8)
        output.WriteLine("output lex:" + outputDir)

    [<Fact>]
    member _.``10 - valid DFA``() =
        let src = fslex.toFslexDFAFile()
        Should.equal src.nextStates TermDFA.nextStates

        let headerFslex =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFslex =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let filePath = Path.Combine(__SOURCE_DIRECTORY__, "TermDFA.fs")
            let text = File.ReadAllText(filePath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 1 text

        Should.equal headerFslex header
        Should.equal semansFslex semans



