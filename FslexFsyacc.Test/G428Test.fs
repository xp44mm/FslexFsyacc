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

    // 与fsyacc文件完全相对应的结构树
    let rawFsyacc = 
        text
        |> RawFsyaccFileUtils.parse 

    let flatedFsyacc = 
        rawFsyacc 
        |> RawFsyaccFileUtils.toFlated

    let inputProductionList =
        flatedFsyacc.rules
        |> FsyaccFileRules.getMainProductions

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let fsyacc = 
            text
            |> FlatFsyaccFileUtils.parse
        // the start symbol of bnf 
        let s0 = 
            fsyacc.rules
            |> FlatFsyaccFileRule.getStartSymbol

        let src = 
            fsyacc.start(s0, Set.empty)
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        output.WriteLine(src)

    [<Fact>]
    member _.``02 - data printer``() =
        let ptbl =     
            let mainProductions = FsyaccFileRules.getMainProductions flatedFsyacc.rules
            let dummyTokens = FsyaccFileRules.getDummyTokens flatedFsyacc.rules
            EncodedParseTableCrewUtils.getEncodedParseTableCrew(
                mainProductions,
                dummyTokens,
                flatedFsyacc.precedences)

        output.WriteLine($"let encodedActions = {stringify ptbl.encodedActions}")
        output.WriteLine($"let encodedClosures = {stringify ptbl.encodedClosures}")


