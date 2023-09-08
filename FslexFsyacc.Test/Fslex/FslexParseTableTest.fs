﻿namespace FslexFsyacc.Fslex

open System
open System.Text
open System.Text.RegularExpressions
open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Idioms
open FSharp.Literals.Literal
open FSharp.xUnit

open FslexFsyacc.Yacc
open FslexFsyacc.Fsyacc
open FslexFsyacc.Runtime

type FslexParseTableTest(output: ITestOutputHelper) =
    let show res =
        res |> stringify |> output.WriteLine

    let solutionPath =
        DirectoryInfo(
            __SOURCE_DIRECTORY__
        )
            .Parent
            .Parent
            .FullName

    let parseTblName = "FslexParseTable"
    let parseTblModule = $"FslexFsyacc.Fslex.{parseTblName}"
    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fslex")
    let filePath = Path.Combine(sourcePath, @"fslex.fsyacc")
    let parseTblPath = Path.Combine(sourcePath, $"{parseTblName}.fs")

    //let text = File.ReadAllText(filePath)
    //let rawFsyacc = RawFsyaccFile.parse text
    //let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc


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
        //|> FlatFsyaccFileUtils.parse
        |> FlatFsyaccFileUtils.toFsyaccParseTableFile

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let fsyacc = 
            text
            |> FlatFsyaccFileUtils.parse

        let startSymbol = 
            fsyacc.rules
            |> FlatFsyaccFileRule.getStartSymbol

        let txt = 
            fsyacc.start(startSymbol, Set.empty)
            |> RawFsyaccFile2Utils.fromFlat
            |> RawFsyaccFile2Utils.render

        output.WriteLine(txt)

    [<Fact>]
    member _.``02 - list all tokens``() =
        let grammar = grammar flatedFsyacc

        let y = set [ 
            "%%"
            "&"
            "("
            ")"
            "*"
            "+"
            "/"
            "="
            "?"
            "CAP"
            "HEADER"
            "HOLE"
            "ID"
            "LITERAL"
            "SEMANTIC"
            "["
            "]"
            "|" 
            ]

        let tokens = grammar.symbols - grammar.nonterminals
        show tokens

    [<Fact>]
    member _.``03 - list all states``() =
        let collection = ambiguousCollection flatedFsyacc
        
        let text = collection.render()
        output.WriteLine(text)

    [<Fact>]
    member _.``04 - 汇总冲突的产生式``() =
        let collection = ambiguousCollection flatedFsyacc

        let productions = 
            collection.collectConflictedProductions()

        // production -> %prec
        let pprods =
            ProductionUtils.precedenceOfProductions collection.grammar.terminals productions

        //优先级应该据此结果给出，不能少，也不应该多。
        let y =
            [ [ "expr"; "expr"; "&"; "expr" ], "&"
              [ "expr"; "expr"; "*"         ], "*"
              [ "expr"; "expr"; "+"         ], "+"
              [ "expr"; "expr"; "?"         ], "?"
              [ "expr"; "expr"; "|"; "expr" ], "|" ]

        Should.equal y pprods

    [<Fact>]
    member _.``05 - list the type annotaitions``() =
        let grammar = grammar flatedFsyacc

        let terminals =
            grammar.terminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let nonterminals =
            grammar.nonterminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let src =
            [
                "// Do not list symbols whose return value is always `null`"
                ""
                "// terminals: ref to the returned type of `getLexeme`"
                "%type<> " + terminals
                ""
                "// nonterminals"
                "%type<> " + nonterminals
            ] 
            |> String.concat "\r\n"

        output.WriteLine(src)

    [<Fact()>] // Skip="once for all!"
    member _.``06 - generate ParseTable``() =
        let parseTbl = parseTbl flatedFsyacc

        let fsharpCode = parseTbl.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, fsharpCode, Encoding.UTF8)
        output.WriteLine("output yacc:" + parseTblPath)

    [<Fact>]
    member _.``07 - valid ParseTable``() =
        let src = parseTbl flatedFsyacc

        Should.equal src.actions FslexParseTable.actions
        Should.equal src.closures FslexParseTable.closures

        let prodsFsyacc = 
            List.map fst src.rules

        let prodsParseTable = 
            List.map fst FslexParseTable.rules

        Should.equal prodsFsyacc prodsParseTable

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFsyacc =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            File.ReadAllText(parseTblPath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

    [<Fact>]
    member _.``08 - regex first or last token test``() =
        let grammar = grammar flatedFsyacc

        let lastsOfExpr = grammar.lasts.["expr"]
        let firstsOfExpr = grammar.firsts.["expr"]

        show lastsOfExpr
        show firstsOfExpr

