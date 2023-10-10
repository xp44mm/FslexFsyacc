namespace FslexFsyacc.Expr

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals.Literal

open System.IO
open System.Text

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime

type ExprParseTableTest(output:ITestOutputHelper) =
    let parseTblName = "ExprParseTable"
    let parseTblModule = $"FslexFsyacc.Expr.{parseTblName}"
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"expr.fsyacc")
    let parseTblPath = Path.Combine(__SOURCE_DIRECTORY__, $"{parseTblName}.fs")

    let text = File.ReadAllText(filePath,Encoding.UTF8)
    let fsyaccCrew =
        text
        |> RawFsyaccFileCrewUtils.parse
        |> FlatedFsyaccFileCrewUtils.getFlatedFsyaccFileCrew

    let tblCrew =
        fsyaccCrew
        |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let s0 = tblCrew.startSymbol
        let flatedFsyacc =
            fsyaccCrew
            |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

        let src = 
            flatedFsyacc 
            |> FlatFsyaccFileUtils.start(s0, Set.empty)
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

    //[<Fact(
    //Skip="按需生成文件"
    //)>]
    //member _.``11 - data printer``() =
    //    let inputProductionList =
    //        flatedFsyacc.rules
    //        |> FsyaccFileRules.getMainProductions

    //    let grammar = Grammar.from inputProductionList

    //    let itemCores = ItemCoreFactory.make grammar.productions

    //    let itemCoreAttributes =
    //        ItemCoreAttributeFactory.make grammar.nonterminals grammar.nullables grammar.firsts itemCores

    //    let lalrCollection = LALRCollection.create(inputProductionList)
    //    let kernels = 
    //        lalrCollection.kernels
    //        |> FSharp.Idioms.Map.keys

    //    let closures =
    //        lalrCollection.closures
    //        |> Map.values
    //        |> Seq.toList

    //    let gotos =
    //        lalrCollection.getGOTOs()
    //        |> Map.values
    //        |> Seq.toList

    //    let ambCollection = AmbiguousCollection.create inputProductionList

    //    let conflicts =
    //        ambCollection.conflicts
    //        |> Map.values
    //        |> Seq.toList
    //    let productionNames = FsyaccFileRules.getProductionNames flatedFsyacc.rules
    //    let precedences = flatedFsyacc.precedences

    //    let parsingTable = 
    //        ParsingTable.create(inputProductionList,productionNames,precedences)

    //    let actions = 
    //        parsingTable.actions
    //        |> Map.values
    //        |> Seq.toList

    //    let resolvedClosures = 
    //        parsingTable.closures
    //        |> Map.values
    //        |> Seq.toList

    //    let filename = "ExprData"
    //    let lines =
    //        [
    //            $"module FslexFsyacc.Expr.{filename}"
    //            "open FslexFsyacc.Runtime"
    //            $"let inputProductionList = {stringify inputProductionList}"
    //            $"let mainProductions = {stringify grammar.mainProductions}"
    //            $"let augmentedProductions = {stringify grammar.productions}"
    //            $"let symbols = {stringify grammar.symbols}"
    //            $"let nonterminals = {stringify grammar.nonterminals}"
    //            $"let terminals = {stringify grammar.terminals}"
    //            $"let nullables = {stringify grammar.nullables}"
    //            $"let firsts = {stringify grammar.firsts}"
    //            $"let lasts = {stringify grammar.lasts}"
    //            $"let follows = {stringify grammar.follows}"
    //            $"let precedes = {stringify grammar.precedes}"
    //            $"let itemCores = {stringify itemCores}"
    //            $"let itemCoreAttributes = {stringify itemCoreAttributes}"
    //            $"let kernels = {stringify kernels}"
    //            $"let closures = {stringify closures}"
    //            $"let gotos = {stringify gotos}"
    //            $"let conflicts = {stringify conflicts}"

    //            $"let productionNames = {stringify productionNames}"
    //            $"let precedences = {stringify precedences}"

    //            $"let actions = {stringify actions}"
    //            $"let resolvedClosures = {stringify resolvedClosures}"
    //        ] 
    //        |> String.concat "\r\n"
    //    let datapath = Path.Combine(__SOURCE_DIRECTORY__,$"{filename}.fs")
    //    File.WriteAllText(datapath, lines, Encoding.UTF8)
    //    output.WriteLine($"文件输出完成:\r\n{datapath}")


