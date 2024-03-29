﻿namespace FslexFsyacc.Fslex
open FslexFsyacc.Lex

open System.IO
open System.Text

open Xunit
open Xunit.Abstractions

open FSharp.Idioms
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
    let fslex = FslexFileUtils.parse text

    let name = "FslexDFA"
    let moduleName = $"FslexFsyacc.Fslex.{name}"
    let modulePath = Path.Combine(sourcePath, $"{name}.fs")

    [<Fact>]
    member _.``01 - compiler test``() =
        let hdr,dfs,rls = FslexCompiler.compile text
        show hdr
        show dfs
        show rls
        
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
        show y

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


