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

    // 与fsyacc文件完全相对应的结构树
    let rawFsyacc = 
        text
        |> RawFsyaccFileUtils.parse 

    let flatedFsyacc = 
        rawFsyacc 
        |> RawFsyaccFileUtils.toFlated

    let grammar (flatedFsyacc) =
        flatedFsyacc
        |> FlatFsyaccFileUtils.toGrammar

    let ambiguousCollection (flatedFsyacc) =
        flatedFsyacc
        |> FlatFsyaccFileUtils.toAmbiguousCollection

    //解析表数据
    let parseTbl (flatedFsyacc) = 
        flatedFsyacc
        |> FlatFsyaccFileUtils.toFsyaccParseTableFile

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let fsyacc = 
            text
            |> FlatFsyaccFileUtils.parse

        let s0 = 
            fsyacc.rules
            |> FlatFsyaccFileRule.getStartSymbol

        let src = 
            fsyacc.start(s0, Set.empty)
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        output.WriteLine(src)

    [<Fact()>] // Skip="once for all!"
    member _.``generate Parse Table``() =
        let parseTbl = parseTbl flatedFsyacc
        let src = parseTbl.generateModule(parseTblModule)

        File.WriteAllText(parseTblPath, src, Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{parseTblPath}")

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        let parseTbl = parseTbl flatedFsyacc

        Should.equal parseTbl.actions ExprParseTable.actions
        Should.equal parseTbl.closures ExprParseTable.closures

        //产生式比较
        let prodsFsyacc = 
            List.map fst parseTbl.rules

        let prodsParseTable = 
            List.map fst ExprParseTable.rules

        Should.equal prodsFsyacc prodsParseTable

        //header,semantic代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",parseTbl.header)

        let semansFsyacc =
            let mappers = parseTbl.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(parseTblPath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

