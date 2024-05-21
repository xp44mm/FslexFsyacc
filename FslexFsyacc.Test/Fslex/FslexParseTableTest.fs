namespace FslexFsyacc.Fslex

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

type FslexParseTableTest(output: ITestOutputHelper) =
    let solutionPath =
        DirectoryInfo(
            __SOURCE_DIRECTORY__
        )
            .Parent
            .Parent
            .FullName

    // module name
    let parseTblName = "FslexParseTable"
    let parseTblModule = $"FslexFsyacc.Fslex.{parseTblName}"
    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fslex")
    let filePath = Path.Combine(sourcePath, @"fslex.fsyacc")
    let parseTblPath = Path.Combine(sourcePath, $"{parseTblName}.fs")

    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler.compile
        //|> fun f -> f.migrate()

    let fsyacc =
        rawFsyacc
        |> FslexFsyacc.Runtime.YACCs.FlatFsyaccFile.from

    let tbl =
        fsyacc.getYacc()

    let moduleFile = FsyaccParseTableFile.from fsyacc

    //let fsyaccCrew =
    //    text
    //    |> RawFsyaccFileCrewUtils.parse
    //    |> FlatedFsyaccFileCrewUtils.fromRawFsyaccFileCrew

    //let tblCrew =
    //    fsyaccCrew
    //    |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

    //[<Fact>]
    //member _.``01 - norm fsyacc file``() =
    //    let startSymbol = tblCrew.startSymbol
    //    let flatedFsyacc =
    //        fsyaccCrew
    //        |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

    //    let txt = 
    //        flatedFsyacc 
    //        |> FlatFsyaccFileUtils.start startSymbol
    //        |> RawFsyaccFileUtils.fromFlat
    //        |> RawFsyaccFileUtils.render

    //    output.WriteLine(txt)

    [<Fact>]
    member _.``02 - list all tokens``() =
        let y = set [ 
            "%%"
            "&"
            "("
            ")"
            "*"
            "+"
            "/"
            "="
            "?"
            "CAP"
            "HEADER"
            "HOLE"
            "ID"
            "LITERAL"
            "REDUCER"
            "["
            "]"
            "|" 
            ]

        //let tokens = tblCrew.symbols - tblCrew.nonterminals
        Should.equal y tbl.bnf.grammar.terminals

    //[<Fact>]
    //member _.``03 - list all states``() =       
    //    let text = 
    //        AmbiguousCollectionUtils.render 
    //            tblCrew.terminals
    //            tblCrew.conflictedItemCores
    //            (tblCrew.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq)

    //    output.WriteLine(text)

    //[<Fact>]
    //member _.``04 - 汇总冲突的产生式``() =
    //    let productions = 
    //        AmbiguousCollectionUtils.collectConflictedProductions tblCrew.conflictedItemCores

    //    // production -> %prec
    //    let pprods =
    //        ProductionSetUtils.precedenceOfProductions tblCrew.terminals productions

    //    //优先级应该据此结果给出，不能少，也不应该多。
    //    let y =
    //        [ [ "expr"; "expr"; "&"; "expr" ], "&"
    //          [ "expr"; "expr"; "*"         ], "*"
    //          [ "expr"; "expr"; "+"         ], "+"
    //          [ "expr"; "expr"; "?"         ], "?"
    //          [ "expr"; "expr"; "|"; "expr" ], "|" ]

    //    Should.equal y pprods

    //[<Fact>]
    //member _.``05 - list the type annotaitions``() =
    //    let terminals =
    //        tblCrew.terminals
    //        |> Seq.map RenderUtils.renderSymbol
    //        |> String.concat " "

    //    let nonterminals =
    //        tblCrew.nonterminals
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

    [<Fact(
    Skip="按需更新源代码"
    )>]
    member _.``06 - generate ParseTable``() =
        let outp = moduleFile.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, outp, Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{parseTblPath}")

    [<Fact>]
    member _.``07 - valid ParseTable``() =
        Should.equal tbl.bnf.terminals  FslexParseTable.tokens
        Should.equal tbl.encodeActions  FslexParseTable.actions
        Should.equal tbl.encodeClosures FslexParseTable.closures

        //产生式比较
        let prodsFsyacc =
            fsyacc.rules
            |> Seq.map (fun rule -> rule.production)
            |> Seq.toList

        let prodsParseTable =
            FslexParseTable.rules
            |> List.map fst

        Should.equal prodsFsyacc prodsParseTable

        //header,reducers代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",fsyacc.header)

        let semansFsyacc =
            let mappers = moduleFile.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(parseTblPath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 3 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

