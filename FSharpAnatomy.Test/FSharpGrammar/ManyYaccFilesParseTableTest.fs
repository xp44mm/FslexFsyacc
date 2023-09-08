namespace FSharpAnatomy.FSharpGrammar

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Literals.Literal
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

        //let rawFsyacc = RawFsyaccFile.parse text
        //FlatFsyaccFile.fromRaw rawFsyacc
        FlatFsyaccFileUtils.parse text

    let removeErrorRules =
        let robust = set [
            "error";
            "recover";
            "coming_soon";"COMING_SOON";
            "_IS_HERE"
            ]
        FsyaccFileRules.removeErrorRules robust

    [<Fact(Skip="no for verify")>]
    member _.``001 - explicitMemberConstraint test``() =

        let s0 = "explicitMemberConstraint"
        let typeAnnotWhenConstraintsFsyacc = readYacc s0

        let parsFsyacc = readYacc "pars"

        let sumFsyacc =
            {
                parsFsyacc with
                    rules =
                        [
                            yield! typeAnnotWhenConstraintsFsyacc.rules
                            yield! parsFsyacc.rules;
                        ]
                        |> removeErrorRules
                        |> FsyaccFileRules.eliminateChomsky
                        |> List.map (fun(prod,nm,ac)->prod,"","")

            }

        //分解到关键字表达式（含）
        let terminals = set [
            "attributes"
            "topTypeWithTypeConstraints"
        ]
        let fsyacc = sumFsyacc.start(s0,terminals)
        //let txt = fsyacc.toRaw().render()
        let txt =
            fsyacc
            |> RawFsyaccFile2Utils.fromFlat
            |> RawFsyaccFile2Utils.render

        let outputDir = Path.Combine(sourcePath, $"{s0}_result.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n" + outputDir)



