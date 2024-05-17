namespace FslexFsyacc.Fsyacc

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc
open FslexFsyacc.Dir
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

type FsyaccParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    // ** input **
    let parseTblName = "FsyaccParseTable"
    let parseTblModule = $"FslexFsyacc.Fsyacc.{parseTblName}"
    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fsyacc")
    let filePath = Path.Combine(sourcePath, @"fsyacc.fsyacc")
    let parseTblPath = Path.Combine(sourcePath, $"{parseTblName}.fs")

    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler.compile
        |> fun f -> f.migrate()

    let fsyacc =
        rawFsyacc
        |> FslexFsyacc.Runtime.YACCs.FlatFsyaccFile.from

    let tbl =
        fsyacc.getParseTable()

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
    member _.``02 - list all tokens``() =
        let y = set ["%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"HEADER";"ID";"LITERAL";"SEMANTIC";"TYPE_ARGUMENT";"[";"]";"|"]
        Should.equal y tbl.bnf.grammar.terminals

    //[<Fact>]
    //member _.``03 - list all states``() =        
    //    let src = 
    //        AmbiguousCollectionUtils.render
    //            tblCrew.terminals
    //            tblCrew.conflictedItemCores
    //            (tblCrew.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq)
    //    output.WriteLine(src)

    //[<Fact>]
    //member _.``04 - precedence Of Productions`` () =
    //    let productions = 
    //        AmbiguousCollectionUtils.collectConflictedProductions tblCrew.conflictedItemCores

    //    // production -> %prec
    //    let pprods =
    //        ProductionSetUtils.precedenceOfProductions tblCrew.terminals productions

    //    //优先级应该据此结果给出，不能少，也不应该多。
    //    let y = []

    //    Should.equal y pprods

    //[<Fact>]
    //member _.``05 - list declarations``() =
    //    let terminals =
    //        tblCrew.terminals
    //        |> Seq.map RenderUtils.renderSymbol
    //        |> String.concat " "

    //    let nonterminals =
    //        tblCrew.nonterminals
    //        |> Seq.map RenderUtils.renderSymbol
    //        |> String.concat " "

    //    let sourceCode =
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

    //    output.WriteLine(sourceCode)


    [<Fact(
    Skip="按需更新源代码"
    )>]
    member _.``06 - generate FsyaccParseTable``() =
        let outp = moduleFile.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, outp, Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{parseTblPath}")

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        Should.equal tbl.encodeActions  FsyaccParseTable.actions
        Should.equal tbl.encodeClosures FsyaccParseTable.closures

        //产生式比较
        let prodsFsyacc =
            fsyacc.rules
            |> Seq.map (fun rule -> rule.production)
            |> Seq.toList

        let prodsParseTable =
            FsyaccParseTable.rules
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
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

