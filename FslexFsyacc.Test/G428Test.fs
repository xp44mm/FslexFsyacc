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
open FSharp.Literals
open FSharp.Literals.Literal

type G428Test(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    // ** input **
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"428.fsyacc")
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
            |> FlatFsyaccFileUtils.start s0
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        output.WriteLine(src)


    [<Fact>]
    member _.``02 - data printer``() =
        let ptbl =     
            let mainProductions = 
                fsyaccCrew.flatedRules
                |> List.map Triple.first

            let dummyTokens = 
                fsyaccCrew.flatedRules
                |> RuleListUtils.getDummyTokens

            EncodedParseTableCrewUtils.getEncodedParseTableCrew(
                mainProductions,
                dummyTokens,
                fsyaccCrew.flatedPrecedences)

        output.WriteLine($"let encodedActions = {stringify ptbl.encodedActions}")
        output.WriteLine($"let encodedClosures = {stringify ptbl.encodedClosures}")



