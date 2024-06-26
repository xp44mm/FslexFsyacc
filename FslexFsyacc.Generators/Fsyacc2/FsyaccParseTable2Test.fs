﻿namespace FslexFsyacc.Fsyacc

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc
open FslexFsyacc.Dir
open FslexFsyacc.Fsyacc
open FslexFsyacc
open FslexFsyacc.YACCs

open System
open System.IO
open System.Text
open System.Text.RegularExpressions
open Xunit
open Xunit.Abstractions

type FsyaccParseTable2Test(output:ITestOutputHelper) =
    let name = "FsyaccParseTable2"
    let moduleName = $"FslexFsyacc.Fsyacc.{name}"
    let modulePath = Path.Combine(Dir.bootstrap, "Fsyacc2", $"{name}.fs")

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "fsyacc2.fsyacc")
    let text = File.ReadAllText(filePath, Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler2.compile

    let fsyacc =
        rawFsyacc
        |> FlatFsyaccFile.from

    let coder = FsyaccParseTableCoder.from fsyacc

    let tbl =
        fsyacc.getYacc()

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let s0 = rawFsyacc.ruleGroups.Head.lhs
        let rules =
            fsyacc.rules
            //|> RuleSet.removeSymbols robust
            //|> RuleSet.removeHeads heads
            |> RuleSet.crawl s0
            //|> List.map(fun rule -> { rule with reducer = "" })

        let raw = fsyacc.toRaw(rules)
        let src = raw.toCode()

        output.WriteLine(src)

    [<Fact>]
    member _.``02 - print conflict productions``() =
        let st = ConflictedProduction.from fsyacc.rules
        if st.IsEmpty then
            output.WriteLine("no conflict")
        for cp in st do
        output.WriteLine($"{stringify cp}")

    [<Fact(
    Skip="按需更新源代码"
    )>]
    member _.``06 - generate FsyaccParseTable``() =
        let outp = coder.generateModule(moduleName)        
        File.WriteAllText(modulePath, outp, Encoding.UTF8)
        output.WriteLine("output yacc:")
        output.WriteLine(modulePath)

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        Should.equal coder.tokens FsyaccParseTable2.tokens
        Should.equal coder.kernels FsyaccParseTable2.kernels
        Should.equal coder.actions FsyaccParseTable2.actions

        //产生式比较
        let prodsFsyacc =
            fsyacc.rules
            |> Seq.map (fun rule -> rule.production)
            |> Seq.toList

        let prodsParseTable =
            FsyaccParseTable2.rules
            |> List.map fst

        Should.equal prodsFsyacc prodsParseTable

        //header,reducers代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",fsyacc.header)

        let semansFsyacc =
            let mappers = coder.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(modulePath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 4 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

