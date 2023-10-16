namespace FslexFsyacc.Brackets

open System
open System.Text
open System.Text.RegularExpressions
open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Idioms
open FSharp.Literals
open FSharp.xUnit

open FslexFsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Fsyacc
open FslexFsyacc.Runtime

type BoundedParseTableTest(output: ITestOutputHelper) =
    let show res =
        res |> Literal.stringify |> output.WriteLine

    let parseTblName = "BoundedParseTable"
    let parseTblModule = $"FslexFsyacc.Brackets.{parseTblName}"
    let sourcePath = Path.Combine(Dir.solutionPath, @"FslexFsyacc\Brackets")
    let filePath = Path.Combine(sourcePath, "bounded.fsyacc")
    let parseTblPath = Path.Combine(sourcePath, $"{parseTblName}.fs")

    let text = File.ReadAllText(filePath,Encoding.UTF8)

    // 与fsyacc文件完全相对应的结构树
    let rawFsyacc =
        text
        |> RawFsyaccFileCrewUtils.parse

    let flatedFsyacc =
        rawFsyacc
        |> FlatedFsyaccFileCrewUtils.getFlatedFsyaccFileCrew

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let tbl =
            flatedFsyacc
            |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

        let s0 = tbl.startSymbol
        let fsyacc = 
            flatedFsyacc
            |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile
        let src =
            fsyacc
            |> FlatFsyaccFileUtils.start s0
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        output.WriteLine(src)

    [<Fact>]
    member _.``02 - list all tokens``() =
        let grammar =
            flatedFsyacc
            |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew
        let e = set ["LEFT";"RIGHT";"TICK"]

        let y = grammar.symbols - grammar.nonterminals
        show y
        Should.equal e y

    [<Fact>]
    member _.``03 - list all states``() =
        let collection =
            flatedFsyacc
            |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

        let src =
            AmbiguousCollectionUtils.render
                collection.terminals
                collection.conflictedItemCores
                (collection.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq)

        output.WriteLine(src)

    [<Fact>]
    member _.``04 - 汇总冲突的产生式``() =
        let collection =
            flatedFsyacc
            |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

        let productions =
            AmbiguousCollectionUtils.collectConflictedProductions collection.conflictedItemCores

        // production -> %prec
        let pprods =
            ProductionListUtils.precedenceOfProductions collection.terminals productions

        //优先级应该据此结果给出，不能少，也不应该多。
        let y = []

        Should.equal y pprods

    [<Fact>]
    member _.``05 - list declarations``() =
        let grammar =
            flatedFsyacc
            |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew
        let terminals =
            grammar.terminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let nonterminals =
            grammar.nonterminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let src =
            [
                "// Do not list symbols whose return value is always `null`"
                ""
                "// terminals: ref to the returned type of `getLexeme`"
                "%type<> " + terminals
                ""
                "// nonterminals"
                "%type<> " + nonterminals
            ]
            |> String.concat "\r\n"

        output.WriteLine(src)

    [<Fact(
    Skip="once for all!"
    )>] //
    member _.``06 - generate ParseTable``() =
        let parseTbl =
            flatedFsyacc
            |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew
            |> FsyaccParseTableFileUtils.ofSemanticParseTableCrew

        //let parseTbl = id<FsyaccParseTableFile> {
        //    header = raw.header
        //    rules = raw.rules
        //    actions = raw.encodedActions
        //    closures = raw.encodedClosures
        //    declarations = raw.declarations
        //}

        let fsharpCode = parseTbl|> FsyaccParseTableFileUtils.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, fsharpCode, Encoding.UTF8)
        output.WriteLine("output yacc:" + parseTblPath)

    [<Fact>]
    member _.``07 - valid ParseTable``() =
        let raw =
            flatedFsyacc
            |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

        let parseTbl = id<FsyaccParseTableFile> {
            header = raw.header
            rules = raw.rules
            actions = raw.encodedActions
            closures = raw.encodedClosures
            declarations = raw.declarations
        }

        Should.equal parseTbl.actions BoundedParseTable.actions
        Should.equal parseTbl.closures BoundedParseTable.closures

        let prodsFsyacc =
            List.map fst parseTbl.rules

        let prodsParseTable =
            List.map fst BoundedParseTable.rules

        Should.equal prodsFsyacc prodsParseTable

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",parseTbl.header)

        let semansFsyacc =
            let mappers = parseTbl|> FsyaccParseTableFileUtils.generateMappers
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            File.ReadAllText(parseTblPath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

