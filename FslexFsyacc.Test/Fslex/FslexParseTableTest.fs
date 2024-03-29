﻿namespace FslexFsyacc.Fslex

open System
open System.Text
open System.Text.RegularExpressions
open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Idioms
open FSharp.Idioms.Literal
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

    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let fsyaccCrew =
        text
        |> RawFsyaccFileCrewUtils.parse
        |> FlatedFsyaccFileCrewUtils.fromRawFsyaccFileCrew

    let tblCrew =
        fsyaccCrew
        |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let startSymbol = tblCrew.startSymbol
        let flatedFsyacc =
            fsyaccCrew
            |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

        let txt = 
            flatedFsyacc 
            |> FlatFsyaccFileUtils.start startSymbol
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        output.WriteLine(txt)

    [<Fact>]
    member _.``02 - list all tokens``() =
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

        let tokens = tblCrew.symbols - tblCrew.nonterminals
        show tokens

    [<Fact>]
    member _.``03 - list all states``() =       
        let text = 
            AmbiguousCollectionUtils.render 
                tblCrew.terminals
                tblCrew.conflictedItemCores
                (tblCrew.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq)

        output.WriteLine(text)

    [<Fact>]
    member _.``04 - 汇总冲突的产生式``() =
        let productions = 
            AmbiguousCollectionUtils.collectConflictedProductions tblCrew.conflictedItemCores

        // production -> %prec
        let pprods =
            ProductionSetUtils.precedenceOfProductions tblCrew.terminals productions

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
        let terminals =
            tblCrew.terminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let nonterminals =
            tblCrew.nonterminals
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

    [<Fact(
    Skip="按需更新源代码"
    )>] // 
    member _.``06 - generate ParseTable``() =
        let fsharpCode = 
            tblCrew
            |> FsyaccParseTableFileUtils.ofSemanticParseTableCrew
            |> FsyaccParseTableFileUtils.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, fsharpCode, Encoding.UTF8)
        output.WriteLine("output yacc:" + parseTblPath)

    [<Fact>]
    member _.``07 - valid ParseTable``() =
        let src = 
            tblCrew
            |> FsyaccParseTableFileUtils.ofSemanticParseTableCrew
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
            let mappers = src|> FsyaccParseTableFileUtils.generateMappers
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            File.ReadAllText(parseTblPath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

    [<Fact>]
    member _.``08 - regex first or last token test``() =
        let lastsOfExpr = tblCrew.lasts.["expr"]
        let firstsOfExpr = tblCrew.firsts.["expr"]

        show lastsOfExpr
        show firstsOfExpr

