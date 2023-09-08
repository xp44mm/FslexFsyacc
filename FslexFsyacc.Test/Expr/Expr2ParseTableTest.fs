﻿namespace FslexFsyacc.Expr

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals.Literal

open System.IO
open System.Text

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime

type Expr2ParseTableTest(output:ITestOutputHelper) =
    let parseTblName = "Expr2ParseTable"
    let parseTblModule = $"FslexFsyacc.Expr.{parseTblName}"
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"expr2.fsyacc")
    let parseTblPath = Path.Combine(__SOURCE_DIRECTORY__, $"{parseTblName}.fs")

    //let text = File.ReadAllText(filePath)

    //let grammar text =
    //    text
    //    |> FlatFsyaccFileUtils.parse
    //    |> FlatFsyaccFileUtils.toGrammar

    //let ambiguousCollection text =
    //    text
    //    |> FlatFsyaccFileUtils.parse
    //    |> FlatFsyaccFileUtils.toAmbiguousCollection

    //let parseTbl text = 
    //    text
    //    |> FlatFsyaccFileUtils.parse
    //    |> FlatFsyaccFileUtils.toFsyaccParseTableFile
    let text = File.ReadAllText(filePath,Encoding.UTF8)

    // 与fsyacc文件完全相对应的结构树
    let rawFsyacc = 
        text
        |> RawFsyaccFile2Utils.parse 

    let flatedFsyacc = 
        rawFsyacc 
        |> RawFsyaccFile2Utils.toFlated

    let grammar (flatedFsyacc) =
        flatedFsyacc
        |> FlatFsyaccFileUtils.toGrammar

    let ambiguousCollection (flatedFsyacc) =
        flatedFsyacc
        |> FlatFsyaccFileUtils.toAmbiguousCollection

    //解析表数据
    let parseTbl (flatedFsyacc) = 
        flatedFsyacc
        |> FlatFsyaccFileUtils.toFsyaccParseTableFile

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let fsyacc = 
            text
            |> FlatFsyaccFileUtils.parse

        let s0 = 
            fsyacc.rules
            |> FlatFsyaccFileRule.getStartSymbol

        let src = 
            fsyacc.start(s0, Set.empty)
            |> RawFsyaccFile2Utils.fromFlat
            |> RawFsyaccFile2Utils.render

        output.WriteLine(src)

    [<Fact()>] // Skip="once for all!"
    member _.``generate Parse Table``() =
        let parseTbl = parseTbl flatedFsyacc
        let src = parseTbl.generateModule(parseTblModule)

        File.WriteAllText(parseTblPath, src, Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{parseTblPath}")

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        let parseTbl = parseTbl flatedFsyacc

        Should.equal parseTbl.actions Expr2ParseTable.actions
        Should.equal parseTbl.closures Expr2ParseTable.closures

        //产生式比较
        let prodsFsyacc = 
            List.map fst parseTbl.rules

        let prodsParseTable = 
            List.map fst Expr2ParseTable.rules

        Should.equal prodsFsyacc prodsParseTable

        //header,semantic代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",parseTbl.header)

        let semansFsyacc =
            let mappers = parseTbl.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(parseTblPath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

