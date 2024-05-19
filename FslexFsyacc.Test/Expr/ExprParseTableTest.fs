namespace FslexFsyacc.Expr

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc
open FslexFsyacc.Fsyacc
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.YACCs
open FslexFsyacc.Yacc
open System.IO
open System.Text
open Xunit
open Xunit.Abstractions

type ExprParseTableTest(output:ITestOutputHelper) =
    let parseTblName = "ExprParseTable"
    let parseTblModule = $"FslexFsyacc.Expr.{parseTblName}"
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"expr.fsyacc")
    let parseTblPath = Path.Combine(__SOURCE_DIRECTORY__, $"{parseTblName}.fs")

    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler.compile
        |> fun f -> f.migrate()

    let fsyacc =
        rawFsyacc
        |> FslexFsyacc.Runtime.YACCs.FlatFsyaccFile.from

    let tbl =
        fsyacc.getYacc()

    let moduleFile = FsyaccParseTableFile.from fsyacc

    //let fsyaccCrew =
    //    text
    //    |> RawFsyaccFileCrewUtils.parse
    //    |> FlatedFsyaccFileCrewUtils.fromRawFsyaccFileCrew

    //let inputProductionList =
    //    fsyaccCrew.flatedRules
    //    |> List.map Triple.first

    //let tblCrew =
    //    fsyaccCrew
    //    |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

    [<Fact>]
    member _.``00 - print rules``() =
        for r in rawFsyacc.ruleGroups do
        output.WriteLine($"{stringify r}")

    [<Fact>]
    member _.``01 - print resolvedClosures``() =
        output.WriteLine($"{stringify tbl.resolvedClosures}")

    //[<Fact>]
    //member _.``01 - norm fsyacc file``() =
    //    let s0 = tblCrew.startSymbol
    //    let flatedFsyacc =
    //        fsyaccCrew
    //        |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

    //    let src =
    //        flatedFsyacc
    //        |> FlatFsyaccFileUtils.start s0
    //        |> RawFsyaccFileUtils.fromFlat
    //        |> RawFsyaccFileUtils.render

    //    output.WriteLine(src)

    [<Fact(
    //Skip="按需更新源代码"
    )>]
    member _.``02 - generate Parse Table``() =
        let outp = moduleFile.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, outp, Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{parseTblPath}")

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        Should.equal tbl.encodeActions  ExprParseTable.actions
        Should.equal tbl.encodeClosures ExprParseTable.closures

        //产生式比较
        let prodsFsyacc =
            fsyacc.rules
            |> Seq.map (fun rule -> rule.production)
            |> Seq.toList

        let prodsParseTable =
            ExprParseTable.rules
            |> List.map fst

        Should.equal prodsFsyacc prodsParseTable

        //header,semantic代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",fsyacc.header)

        let semansFsyacc =
            let mappers = moduleFile.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(parseTblPath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

    //[<Fact(
    //Skip="按需生成文件"
    //)>]
    //member _.``11 - data printer``() =
    //    let itemCoreCrews =
    //        tblCrew.itemCoreCrews
    //        |> Map.values
    //        //|> Seq.map(fun crew -> ItemCoreCrewUtils.recurItemCoreCrew crew)
    //        |> List.ofSeq

    //    let filename = "ExprData"
    //    let lines =
    //        [
    //            $"module FslexFsyacc.Expr.{filename}"
    //            "open FslexFsyacc.Runtime"
    //            $"let inputProductionList = {stringify inputProductionList}"
    //            $"let mainProductions = {stringify tblCrew.mainProductions}"
    //            $"let augmentedProductions = {stringify tblCrew.augmentedProductions}"
    //            $"let symbols = {stringify tblCrew.symbols}"
    //            $"let nonterminals = {stringify tblCrew.nonterminals}"
    //            $"let terminals = {stringify tblCrew.terminals}"
    //            $"let nullables = {stringify tblCrew.nullables}"
    //            $"let firsts = {stringify tblCrew.firsts}"
    //            $"let lasts = {stringify tblCrew.lasts}"
    //            $"let follows = {stringify tblCrew.follows}"
    //            $"let precedes = {stringify tblCrew.precedes}"
    //            $"let itemCoreCrews = {stringify itemCoreCrews}"

    //            $"let kernels = {stringify tblCrew.kernels}"
    //            $"let closures = {stringify tblCrew.closures}"
    //            $"let GOTOs = {stringify tblCrew.GOTOs}"
    //            $"let conflictedItemCores = {stringify tblCrew.conflictedItemCores}"

    //            $"let dummyTokens = {stringify tblCrew.dummyTokens}"
    //            $"let precedences = {stringify tblCrew.precedences}"


    //            $"let unambiguousItemCores = {stringify tblCrew.unambiguousItemCores}"

    //            $"let actions = {stringify tblCrew.actions}"
    //            $"let resolvedClosures = {stringify tblCrew.resolvedClosures}"
    //            $"let encodedActions = {stringify tblCrew.encodedActions}"
    //            $"let encodedClosures = {stringify tblCrew.encodedClosures}"

    //        ]
    //        |> String.concat "\r\n"

    //    let datapath = Path.Combine(__SOURCE_DIRECTORY__,$"{filename}.fs")
    //    File.WriteAllText(datapath, lines, Encoding.UTF8)
    //    output.WriteLine($"文件输出完成:\r\n{datapath}")


