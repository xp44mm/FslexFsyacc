namespace FslexFsyacc.Expr

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc
open FslexFsyacc.Fsyacc
open FslexFsyacc
open FslexFsyacc.Precedences
open FslexFsyacc.YACCs

open System.IO
open System.Text
open Xunit
open Xunit.Abstractions

type ExprParseTableTest(output:ITestOutputHelper) =
    let parseTblName = "ExprParseTable"
    let parseTblModule = $"FslexFsyacc.Expr.{parseTblName}"
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"expr.fsyacc")
    let parseTblPath = Path.Combine(__SOURCE_DIRECTORY__, $"{parseTblName}.fs")

    let text = File.ReadAllText(filePath, Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler2.compile

    let fsyacc =
        rawFsyacc
        |> FlatFsyaccFile.from

    let coder = FsyaccParseTableCoder.from fsyacc

    let tbl =
        fsyacc.getYacc()

    let bnf = tbl.bnf

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let s0 = rawFsyacc.ruleGroups.Head.lhs
        Should.equal s0 "expr"
        let rules =
            fsyacc.rules
            //|> RuleSet.removeSymbols robust
            //|> RuleSet.removeHeads heads
            |> RuleSet.crawl s0
            //|> List.map(fun rule -> { rule with reducer = "" })

        let raw = fsyacc.toRaw(rules)
        let src = raw.toCode()
        output.WriteLine(src)

    [<Fact>]
    member _.``02 - print conflict``() =
        for acts in bnf.getProperConflictActions() do
        output.WriteLine($"{stringify acts}")

    [<Fact>]
    member _.``03 - print conflict productions``() =
        let st = ConflictedProduction.from fsyacc.rules
        for cp in st do
        output.WriteLine($"{stringify cp}")

    [<Fact(
    Skip="按需更新源代码"
    )>]
    member _.``04 - generate Parse Table``() =
        let outp = coder.generateModule(parseTblModule)
        //output.WriteLine(outp)
        File.WriteAllText(parseTblPath, outp, Encoding.UTF8)
        output.WriteLine("output yacc:")
        output.WriteLine(parseTblPath)
    [<Fact>]
    member _.``05 - valid ParseTable``() =
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

