namespace FslexFsyacc.Brackets

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc
open FslexFsyacc.Fsyacc
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.YACCs
open FslexFsyacc.Yacc
open System
open System.IO
open System.Text
open System.Text.RegularExpressions
open Xunit
open Xunit.Abstractions

type BoundedParseTableTest(output: ITestOutputHelper) =
    let parseTblName = "BoundedParseTable"
    let parseTblModule = $"FslexFsyacc.Brackets.{parseTblName}"
    let sourcePath = Path.Combine(Dir.solutionPath, @"FslexFsyacc\Brackets")
    let filePath = Path.Combine(sourcePath, "bounded.fsyacc")
    let parseTblPath = Path.Combine(sourcePath, $"{parseTblName}.fs")

    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler.compile

    let fsyacc =
        rawFsyacc
        |> FslexFsyacc.Runtime.YACCs.FlatFsyaccFile.from

    //let tbl =
    //    fsyacc.getYacc()

    let coder = FsyaccParseTableCoder.from fsyacc

    //[<Fact>]
    //member _.``01 - norm fsyacc file``() =
    //    let tbl =
    //        fsyacc
    //        |> fsyaccFileCrewUtils.getSemanticParseTableCrew

    //    let s0 = tbl.startSymbol
    //    let fsyacc = 
    //        fsyacc
    //        |> fsyaccFileCrewUtils.toFlatFsyaccFile
    //    let src =
    //        fsyacc
    //        |> FlatFsyaccFileUtils.start s0
    //        |> RawFsyaccFileUtils.fromFlat
    //        |> RawFsyaccFileUtils.render

    //    output.WriteLine(src)

    //[<Fact>]
    //member _.``03 - list all states``() =
    //    //let collection =
    //    //    fsyacc
    //    //    |> fsyaccFileCrewUtils.getSemanticParseTableCrew
    //    let grammar = tbl.bnf.grammar
    //    let src =
    //        AmbiguousCollectionUtils.render
    //            grammar.terminals
    //            tbl.bnf.conflictedItemCores
    //            (collection.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq)

    //    output.WriteLine(src)

    //[<Fact>]
    //member _.``05 - list declarations``() =
    //    let grammar =
    //        fsyacc
    //        |> fsyaccFileCrewUtils.getSemanticParseTableCrew
    //    let terminals =
    //        grammar.terminals
    //        |> Seq.map RenderUtils.renderSymbol
    //        |> String.concat " "

    //    let nonterminals =
    //        grammar.nonterminals
    //        |> Seq.map RenderUtils.renderSymbol
    //        |> String.concat " "

    //    let src =
    //        [
    //            "// Do not list symbols whose return value is always `null`"
    //            ""
    //            "// terminals: ref to the returned type of `getLexeme`"
    //            "%type<> " + terminals
    //            ""
    //            "// nonterminals"
    //            "%type<> " + nonterminals
    //        ]
    //        |> String.concat "\r\n"

    //    output.WriteLine(src)

    [<Fact>]
    member _.``02 - print conflict productions``() =
        let st = ConflictedProduction.from fsyacc.rules
        if st.IsEmpty then
            output.WriteLine("no conflict")
        for cp in st do
        output.WriteLine($"{stringify cp}")

    [<Fact(
    Skip="once for all!"
    )>]
    member _.``06 - generate ParseTable``() =
        let outp = coder.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, outp, Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{parseTblPath}")

    [<Fact>]
    member _.``07 - valid ParseTable``() =
        Should.equal coder.tokens BoundedParseTable.tokens
        Should.equal coder.kernels BoundedParseTable.kernels
        Should.equal coder.actions BoundedParseTable.actions

        //产生式比较
        let prodsFsyacc =
            fsyacc.rules
            |> Seq.map (fun rule -> rule.production)
            |> Seq.toList

        let prodsParseTable =
            BoundedParseTable.rules
            |> List.map fst

        Should.equal prodsFsyacc prodsParseTable

        //header,reducers代码比较
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

