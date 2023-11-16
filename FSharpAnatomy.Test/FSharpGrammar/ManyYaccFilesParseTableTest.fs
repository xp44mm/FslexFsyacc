namespace FSharpAnatomy.FSharpGrammar

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms.Literal
open FSharp.Idioms

open System.IO
open System.Text

type ManyYaccFilesParseTableTest(output:ITestOutputHelper) =

    let show res =
        res
        |> stringify
        |> output.WriteLine

    let sourcePath = Path.Combine(__SOURCE_DIRECTORY__, "fsyacc")

    let readYacc filename =
        let filePath = Path.Combine(sourcePath, $"{filename}.fsyacc")
        let text = File.ReadAllText(filePath,Encoding.UTF8)
        text
        |> RawFsyaccFileCrewUtils.parse
        |> FlatedFsyaccFileCrewUtils.fromRawFsyaccFileCrew
        |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

    let robust = set [
        "error";
        "recover";
        "coming_soon";"COMING_SOON";
        "_IS_HERE"
        ]

    [<Fact(
    Skip="no for verify"
    )>]
    member _.``001 - explicitMemberConstraint test``() =

        let s0 = "explicitMemberConstraint"
        let typeAnnotWhenConstraintsFsyacc = readYacc s0

        let parsFsyacc = readYacc "pars"

        //let mergedRules =
        //    [
        //        for KeyValue(k,v) in typeAnnotWhenConstraintsFsyacc.augmentRules do
        //            k,v
        //        for KeyValue(k,v) in parsFsyacc.augmentRules do
        //            k,v
        //    ]
        //    |> Map.ofList

        //let augmentRules = 
        //    mergedRules
        //    |> Map.filter(fun prod _ -> 
        //        prod 
        //        |> ProductionUtils.isWithoutError robust)
        //    |> RuleSetUtils.eliminateChomsky
        //    |> Map.map (fun prod (nm,ac) ->"","")

        //let sumFsyacc =
        //    {
        //        parsFsyacc with
        //            augmentRules = augmentRules
        //    }
        let rules = 
            typeAnnotWhenConstraintsFsyacc.rules @ parsFsyacc.rules
            |> List.filter(fun (prod,_,_) -> 
                prod 
                |> ProductionUtils.without robust)
            |> FlatRulesUtils.eliminateChomsky
            |> List.map (fun (prod,_,_) ->prod,"","")

        let sumFsyacc =
            {
                parsFsyacc with
                    rules = rules
            }

        //分解到关键字表达式（含）
        let terminals = set [
            "attributes"
            "topTypeWithTypeConstraints"
        ]
        let fsyacc = 
            {
                sumFsyacc with
                    rules = 
                    sumFsyacc.rules
                    |> FlatRulesUtils.removeHeads terminals
            }
            |> FlatFsyaccFileUtils.start s0

        let txt =
            fsyacc
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        let outputDir = Path.Combine(sourcePath, $"{s0}_result.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n" + outputDir)



