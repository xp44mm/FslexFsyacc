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

    let fsyacc =
        rawFsyacc
        |> FslexFsyacc.Runtime.YACCs.FlatFsyaccFile.from

    let tbl =
        fsyacc.getYacc()

    let bnf = tbl.bnf
    let coder = FsyaccParseTableCoder.from fsyacc

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

    [<Fact>]
    member _.``00 - print rules``() =
        for r in rawFsyacc.ruleGroups do
        output.WriteLine($"{stringify r}")

    [<Fact>]
    member _.``01 - print resolvedClosures``() =
        output.WriteLine($"{stringify tbl.resolvedClosures}")

    [<Fact>]
    member _.``02 - print conflict``() =
        let bnf = tbl.bnf
        for acts in bnf.getProperConflictActions() do
        output.WriteLine($"{stringify acts}")

    [<Fact>]
    member _.``02 - print conflict productions``() =
        let bnf = tbl.bnf
        let productions = bnf.getConflictedProductions()
        for prod in productions do
        output.WriteLine($"{stringify prod}")

    [<Fact(
    Skip="按需更新源代码"
    )>]
    member _.``02 - generate Parse Table``() =
        let outp = coder.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, outp, Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{parseTblPath}")

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        Should.equal coder.tokens ExprParseTable.tokens
        Should.equal coder.kernels ExprParseTable.kernels
        Should.equal coder.actions ExprParseTable.actions

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
            let mappers = coder.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(parseTblPath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 4 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

