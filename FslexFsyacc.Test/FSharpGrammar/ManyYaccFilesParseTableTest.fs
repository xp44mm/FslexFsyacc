namespace FslexFsyacc.FSharpGrammar

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
        let text = File.ReadAllText(filePath)
        let rawFsyacc = RawFsyaccFile.parse text
        FlatFsyaccFile.fromRaw rawFsyacc

    let removeErrorRules =
        let robust = set [
            "error";
            "recover";
            "coming_soon";"COMING_SOON";
            "_IS_HERE"
            ]
        FsyaccFileRules.removeErrorRules robust

    [<Fact>]
    member _.``001 - typeAnnot test``() =
        let parsFsyacc = readYacc "pars"
        let s0 = "typeAnnot"
        let typeAnnotWhenConstraintsFsyacc = readYacc s0

        let sumFsyacc = 
            {
                parsFsyacc with
                    rules = 
                        [ 
                            yield! parsFsyacc.rules; 
                            yield! typeAnnotWhenConstraintsFsyacc.rules
                        ]
                        |> removeErrorRules
                        |> FsyaccFileRules.eliminateChomsky
                        |> List.map (fun(prod,nm,ac)->prod,"","")

            }

        //分解到关键字表达式（含）
        let terminals = set [
            "attributes"    
        ]
        let fsyacc = sumFsyacc.start(s0,terminals)
        let txt = fsyacc.toRaw().render()
        let outputDir = Path.Combine(sourcePath, $"{s0}_result.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n" + outputDir)



