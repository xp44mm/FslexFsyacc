namespace FslexFsyacc.Fslex
open FslexFsyacc.Runtime.Lex
open FslexFsyacc
open System.IO
open System.Text

open Xunit
open Xunit.Abstractions

open FSharp.Idioms.Literal
open FSharp.xUnit

type FslexDFATest(output:ITestOutputHelper) =
    let name = "FslexDFA"
    let moduleName = $"FslexFsyacc.Fslex.{name}"
    let modulePath = Path.Combine(Dir.bootstrap, "Fslex", $"{name}.fs")

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "fslex.fslex")
    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let fslex = FslexFileUtils.parse text

    [<Fact>]
    member _.``01 - compiler test``() =
        let hdr,dfs,rls = FslexCompiler.compile text
        output.WriteLine(stringify hdr )
        output.WriteLine(stringify dfs )
        output.WriteLine(stringify rls )
        
    [<Fact>]
    member _.``02 - verify``() =
        let y = fslex|>FslexFileUtils.verify

        Assert.True(y.undeclared.IsEmpty)
        Assert.True(y.unused.IsEmpty)

    [<Fact>]
    member _.``03 - universal characters``() =
        let res = fslex|>FslexFileUtils.getRegularExpressions

        let y = 
            res
            |> List.collect(fun re -> re|>RegularExpressionUtils.getCharacters)
            |> Set.ofList
        output.WriteLine(stringify y)

    [<Fact(
    Skip="once and for all!"
    )>]
    member _.``04 - generate DFA``() =
        let y = fslex |> FslexFileUtils.toFslexDFAFile
        let result = y |> FslexDFAFileUtils.generate(moduleName)

        File.WriteAllText(modulePath, result, Encoding.UTF8)
        output.WriteLine("output lex:" + modulePath)

    [<Fact>]
    member _.``10 - valid DFA``() =
        let src = fslex|>FslexFileUtils.toFslexDFAFile
        Should.equal src.nextStates FslexDFA.nextStates

        let headerFslex =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFslex =
            let mappers = src|>FslexDFAFileUtils.generateMappers
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let src = File.ReadAllText(modulePath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 1 src

        Should.equal headerFslex header
        Should.equal semansFslex semans


