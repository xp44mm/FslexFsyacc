namespace FSharpAnatomy

open System
open System.IO
open System.Text

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open FslexFsyacc.Fslex

type TypeArgumentDFATest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let filePath = Path.Combine(Dir.FSharpAnatomyPath, @"typeArgument.fslex")
    let text = File.ReadAllText(filePath)
    let fslex = FslexFile.parse text

    [<Fact>]
    member _.``00 - compiler test``() =
        let hdr,dfs,rls = FslexCompiler.parseToStructuralData text
        show hdr
        show dfs
        show rls
        
    [<Fact>]
    member _.``01 - verify``() =
        let y = fslex.verify()

        Assert.True(y.undeclared.IsEmpty)
        Assert.True(y.unused.IsEmpty)

    [<Fact>]
    member _.``02 - universal characters``() =
        let res = fslex.getRegularExpressions()

        let y = 
            res
            |> List.collect(fun re -> re.getCharacters())
            |> Set.ofList
        show y

    [<Fact(Skip="once and for all!") >] //
    member _.``03 - generate DFA``() =
        let name = "TypeArgumentDFA"
        let moduleName = $"FSharpAnatomy.{name}"

        let dfafile = fslex.toFslexDFAFile()
        let result = dfafile.generate(moduleName)

        let outputDir = Path.Combine(Dir.FSharpAnatomyPath, $"{name}.fs")
        File.WriteAllText(outputDir,result,Encoding.UTF8)
        output.WriteLine("output lex:" + outputDir)

    [<Fact>]
    member _.``10 - valid DFA``() =
        let src = fslex.toFslexDFAFile()
        Should.equal src.nextStates TypeArgumentDFA.nextStates

        let headerFslex =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFslex =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let filePath = Path.Combine(Dir.FSharpAnatomyPath, "TypeArgumentDFA.fs")
            let text = File.ReadAllText(filePath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 1 text

        Should.equal headerFslex header
        Should.equal semansFslex semans



