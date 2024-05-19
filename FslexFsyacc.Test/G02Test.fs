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

type G02Test(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    // ** input **
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"g02.fsyacc")
    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler.compile
        //|> fun f -> f.migrate()

    let fsyacc =
        rawFsyacc
        |> FslexFsyacc.Runtime.YACCs.FlatFsyaccFile.from

    let tbl =
        fsyacc.getYacc()

    //let fsyaccCrew =
    //    text
    //    |> RawFsyaccFileCrewUtils.parse
    //    |> FlatedFsyaccFileCrewUtils.fromRawFsyaccFileCrew

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


    //[<Fact>]
    //member _.``02 - data printer``() =
    //    let ptbl =     
    //        let mainProductions = 
    //            fsyaccCrew.flatedRules
    //            |> List.map Triple.first

    //        let dummyTokens = 
    //            fsyaccCrew.flatedRules
    //            |> List.filter(fun (prod,dummy,act) -> dummy > "")
    //            |> List.map(Triple.firstTwo)
    //            |> Map.ofList

    //        EncodedParseTableCrewUtils.getEncodedParseTableCrew(
    //            mainProductions,
    //            dummyTokens,
    //            fsyaccCrew.flatedPrecedences)

    //    output.WriteLine($"let encodedActions = {stringify ptbl.encodedActions}")
    //    output.WriteLine($"let encodedClosures = {stringify ptbl.encodedClosures}")



