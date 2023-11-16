namespace PolynomialExpressions
open FslexFsyacc.Fslex
open FslexFsyacc.Lex

open System
open System.IO
open System.Text

open Xunit
open Xunit.Abstractions

open FSharp.Idioms.Literal
open FSharp.xUnit

type TermDFATest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"term.fslex")
    let text = File.ReadAllText(filePath)
    let fslex = FslexFileUtils.parse text

    let name = "TermDFA"
    let moduleName = $"PolynomialExpressions.{name}"
    let modulePath = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.fs")

    [<Fact>]
    member _.``00 = tokenize test``() =
        let tokens = 
            FslexTokenUtils.tokenize 0 text
            //|> Seq.map(fun postok -> postok.value)
            |> Seq.map(stringify )
            |> String.concat "\r\n"
        output.WriteLine(tokens)

    [<Fact>]
    member _.``00 = compiler test``() =
        let hdr,dfs,rls = FslexCompiler.compile text
        show hdr
        show dfs
        show rls
        
    [<Fact>]
    member _.``01 = verify``() =
        let y = fslex|>FslexFileUtils.verify

        Assert.True(y.undeclared.IsEmpty)
        Assert.True(y.unused.IsEmpty)

    [<Fact>]
    member _.``02 = universal characters``() =
        let res = fslex|>FslexFileUtils.getRegularExpressions

        let y = 
            res
            |> List.collect(fun re -> re|>RegularExpressionUtils.getCharacters)
            |> Set.ofList
        show y

    [<Fact(
    Skip="once and for all!"
    )>] // 
    member _.``03 = generate DFA``() =

        let dfafile = fslex|>FslexFileUtils.toFslexDFAFile
        let result = dfafile|>FslexDFAFileUtils.generate(moduleName)

        File.WriteAllText(modulePath, result, Encoding.UTF8)
        output.WriteLine("output lex:" + modulePath)

    [<Fact>]
    member _.``10 - valid DFA``() =
        let src = fslex|>FslexFileUtils.toFslexDFAFile
        Should.equal src.nextStates TermDFA.nextStates

        let headerFslex =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFslex =
            let mappers = src|>FslexDFAFileUtils.generateMappers
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(modulePath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 1 text

        Should.equal headerFslex header
        Should.equal semansFslex semans



