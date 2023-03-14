﻿namespace FslexFsyacc.Brackets

open System
open System.Text
open System.Text.RegularExpressions
open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Idioms
open FSharp.Literals
open FSharp.xUnit

open FslexFsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Fsyacc
open FslexFsyacc.Runtime

type BoundedParseTableTest(output: ITestOutputHelper) =
    let show res =
        res |> Literal.stringify |> output.WriteLine

    let sourcePath = Path.Combine(Dir.solutionPath, @"FslexFsyacc\Brackets")
    let filePath = Path.Combine(sourcePath, "bounded.fsyacc")
    let text = File.ReadAllText(filePath)
    //let rawFsyacc = RawFsyaccFile.parse text
    //let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    let parseTblName = "BoundedParseTable"
    let parseTblModule = $"FslexFsyacc.Brackets.{parseTblName}"
    let parseTblPath = Path.Combine(sourcePath, $"{parseTblName}.fs")

    let grammar text =
        text
        |> FlatFsyaccFileUtils.parse
        |> FlatFsyaccFileUtils.toGrammar

    let ambiguousCollection text =
        text
        |> FlatFsyaccFileUtils.parse
        |> FlatFsyaccFileUtils.toAmbiguousCollection

    let parseTbl text = 
        text
        |> FlatFsyaccFileUtils.parse
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

    [<Fact>]
    member _.``02 - list all tokens``() =
        let grammar = grammar text
        let e = set ["LEFT";"RIGHT";"TICK"]

        let y = grammar.symbols - grammar.nonterminals
        show y
        Should.equal e y

    [<Fact>]
    member _.``03 - list all states``() =
        let collection = ambiguousCollection text
        let src = collection.render()
        output.WriteLine(src)

    [<Fact>]
    member _.``04 - 汇总冲突的产生式``() =
        let collection = ambiguousCollection text

        let productions = 
            collection.collectConflictedProductions()

        // production -> %prec
        let pprods =
            ProductionUtils.precedenceOfProductions collection.grammar.terminals productions

        //优先级应该据此结果给出，不能少，也不应该多。
        let y = []

        Should.equal y pprods

    [<Fact>]
    member _.``05 - list declarations``() =
        let grammar = grammar text

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

    [<Fact(Skip="once for all!")>] // 
    member _.``06 - generate ParseTable``() =
        let parseTbl = parseTbl text

        let fsharpCode = parseTbl.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, fsharpCode, Encoding.UTF8)
        output.WriteLine("output yacc:" + parseTblPath)

    [<Fact>]
    member _.``07 - valid ParseTable``() =
        let parseTbl = parseTbl text

        Should.equal parseTbl.actions BoundedParseTable.actions
        Should.equal parseTbl.closures BoundedParseTable.closures

        let prodsFsyacc = 
            List.map fst parseTbl.rules

        let prodsParseTable = 
            List.map fst BoundedParseTable.rules

        Should.equal prodsFsyacc prodsParseTable

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",parseTbl.header)

        let semansFsyacc =
            let mappers = parseTbl.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            File.ReadAllText(parseTblPath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans
