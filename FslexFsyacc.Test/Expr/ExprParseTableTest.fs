namespace FslexFsyacc.Expr

open Xunit
open Xunit.Abstractions

open FSharp.Idioms

open FSharp.xUnit
open FSharp.Idioms.Literal

open System.IO
open System.Text

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime
open FslexFsyacc
type ExprParseTableTest(output:ITestOutputHelper) =
    let parseTblName = "ExprParseTable"
    let parseTblModule = $"FslexFsyacc.Expr.{parseTblName}"
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"expr.fsyacc")
    let parseTblPath = Path.Combine(__SOURCE_DIRECTORY__, $"{parseTblName}.fs")

    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let fsyaccCrew =
        text
        |> RawFsyaccFileCrewUtils.parse
        |> FlatedFsyaccFileCrewUtils.fromRawFsyaccFileCrew

    let inputProductionList =
        fsyaccCrew.flatedRules
        |> List.map Triple.first

    let collectionCrew = 
        inputProductionList
        |> AmbiguousCollectionCrewUtils.newAmbiguousCollectionCrew

    let tblCrew =
        fsyaccCrew
        |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

    [<Fact>]
    member _.``00 - print rules``() =
        for r in fsyaccCrew.inputRuleList do
        output.WriteLine($"{stringify r}")

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let s0 = tblCrew.startSymbol
        let flatedFsyacc =
            fsyaccCrew
            |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

        let src = 
            flatedFsyacc 
            |> FlatFsyaccFileUtils.start s0
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        output.WriteLine(src)

    [<Fact(
    Skip="按需更新源代码"
    )>] // 
    member _.``02 - generate Parse Table``() =
        let src =
            tblCrew
            |> FsyaccParseTableFileUtils.ofSemanticParseTableCrew
            |> FsyaccParseTableFileUtils.generateModule(parseTblModule)

        File.WriteAllText(parseTblPath, src, Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{parseTblPath}")

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        Should.equal tblCrew.encodedActions  ExprParseTable.actions
        Should.equal tblCrew.encodedClosures ExprParseTable.closures

        //产生式比较
        let prodsFsyacc = 
            List.map fst tblCrew.rules

        let prodsParseTable = 
            List.map fst ExprParseTable.rules

        Should.equal prodsFsyacc prodsParseTable

        //header,semantic代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",tblCrew.header)

        let semansFsyacc =
            let mappers = 
                tblCrew
                |> FsyaccParseTableFileUtils.ofSemanticParseTableCrew
                |> FsyaccParseTableFileUtils.generateMappers
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(parseTblPath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

    [<Fact(
    Skip="按需生成文件"
    )>]
    member _.``11 - data printer``() =
        let itemCoreCrews = 
            collectionCrew.itemCoreCrews
            |> Map.values
            //|> Seq.map(fun crew -> ItemCoreCrewUtils.recurItemCoreCrew crew)
            |> List.ofSeq

        let filename = "ExprData"
        let lines =
            [
                $"module FslexFsyacc.Expr.{filename}"
                "open FslexFsyacc.Runtime"
                $"let inputProductionList = {stringify inputProductionList}"
                $"let mainProductions = {stringify collectionCrew.mainProductions}"
                $"let augmentedProductions = {stringify collectionCrew.augmentedProductions}"
                $"let symbols = {stringify collectionCrew.symbols}"
                $"let nonterminals = {stringify collectionCrew.nonterminals}"
                $"let terminals = {stringify collectionCrew.terminals}"
                $"let nullables = {stringify collectionCrew.nullables}"
                $"let firsts = {stringify collectionCrew.firsts}"
                $"let lasts = {stringify collectionCrew.lasts}"
                $"let follows = {stringify collectionCrew.follows}"
                $"let precedes = {stringify collectionCrew.precedes}"
                $"let itemCoreCrews = {stringify itemCoreCrews}"

                $"let kernels = {stringify collectionCrew.kernels}"
                $"let closures = {stringify collectionCrew.closures}"
                $"let GOTOs = {stringify collectionCrew.GOTOs}"
                $"let conflictedItemCores = {stringify collectionCrew.conflictedItemCores}"

                $"let dummyTokens = {stringify tblCrew.dummyTokens}"
                $"let precedences = {stringify tblCrew.precedences}"


                $"let unambiguousItemCores = {stringify tblCrew.unambiguousItemCores}"

                $"let actions = {stringify tblCrew.actions}"
                $"let resolvedClosures = {stringify tblCrew.resolvedClosures}"
                $"let encodedActions = {stringify tblCrew.encodedActions}"
                $"let encodedClosures = {stringify tblCrew.encodedClosures}"

            ] 
            |> String.concat "\r\n"

        let datapath = Path.Combine(__SOURCE_DIRECTORY__,$"{filename}.fs")
        File.WriteAllText(datapath, lines, Encoding.UTF8)
        output.WriteLine($"文件输出完成:\r\n{datapath}")


