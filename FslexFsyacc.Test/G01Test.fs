namespace FslexFsyacc

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime
open FslexFsyacc.Dir

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.xUnit
open FSharp.Idioms
open FSharp.Idioms
open FSharp.Idioms.Literal

type G01Test(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    // ** input **
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"g01.fsyacc")
    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler.compile
        |> fun f -> f.migrate()

    let fsyacc =
        rawFsyacc
        |> FslexFsyacc.Runtime.YACCs.FlatFsyaccFile.from

    let tbl =
        fsyacc.getParseTable()

    //let fsyaccCrew =
    //    text
    //    |> RawFsyaccFileCrewUtils.parse
    //    |> FlatedFsyaccFileCrewUtils.fromRawFsyaccFileCrew

    //let inputProductionList =
    //    fsyaccCrew.flatedRules
    //    |> List.map Triple.first

    //let collectionCrew = 
    //    fsyaccCrew
    //    |> FlatedFsyaccFileCrewUtils.getAmbiguousCollectionCrew

    //let tblCrew =
    //    fsyaccCrew
    //    |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

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

    //[<Fact(
    //Skip="按需生成文件"
    //)>]
    //member _.``11 - data file generator``() =
    //    let filename = "G01Data"
    //    let itemCoreCrewFlats = 
    //        collectionCrew.itemCoreCrews
    //        |> Map.values
    //        |> Seq.map(fun crew -> ItemCoreCrewFlat.from crew)
    //        |> List.ofSeq

    //    let lines =
    //        [
    //            $"module FslexFsyacc.{filename}"
    //            "open FslexFsyacc.Runtime"
    //            $"let inputProductionList = {stringify inputProductionList}"
    //            $"let mainProductions = {stringify collectionCrew.mainProductions}"
    //            $"let augmentedProductions = {stringify collectionCrew.augmentedProductions}"
    //            $"let symbols = {stringify collectionCrew.symbols}"
    //            $"let nonterminals = {stringify collectionCrew.nonterminals}"
    //            $"let terminals = {stringify collectionCrew.terminals}"
    //            $"let nullables = {stringify collectionCrew.nullables}"
    //            $"let firsts = {stringify collectionCrew.firsts}"
    //            $"let lasts = {stringify collectionCrew.lasts}"
    //            $"let follows = {stringify collectionCrew.follows}"
    //            $"let precedes = {stringify collectionCrew.precedes}"
    //            $"let itemCoreCrewFlats = {stringify itemCoreCrewFlats}"

    //            $"let kernels = {stringify collectionCrew.kernels}"
    //            $"let closures = {stringify collectionCrew.closures}"
    //            $"let GOTOs = {stringify collectionCrew.GOTOs}"
    //            $"let conflictedItemCores = {stringify collectionCrew.conflictedItemCores}"

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



