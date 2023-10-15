namespace FslexFsyacc.Fsyacc

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

    let fsyaccCrew =
        text
        |> RawFsyaccFileCrewUtils.parse
        |> FlatedFsyaccFileCrewUtils.getFlatedFsyaccFileCrew

    let tblCrew =
        fsyaccCrew
        |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let s0 = tblCrew.startSymbol
        let flatedFsyacc =
            fsyaccCrew
            |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile
        
        let src = 
            flatedFsyacc 
            |> FlatFsyaccFileUtils.start s0
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        output.WriteLine(src)

    [<Fact>]
    member _.``02 - list all tokens``() =
        let y = set ["%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"HEADER";"ID";"LITERAL";"SEMANTIC";"TYPE_ARGUMENT";"[";"]";"|"]
        show tblCrew.terminals
        Should.equal y tblCrew.terminals

    [<Fact>]
    member _.``03 - list all states``() =        
        let src = 
            AmbiguousCollectionUtils.render 
                tblCrew.terminals
                tblCrew.conflictedItemCores
                (tblCrew.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq)
        output.WriteLine(src)

    [<Fact>]
    member _.``04 - precedence Of Productions`` () =
        let productions = 
            AmbiguousCollectionUtils.collectConflictedProductions tblCrew.conflictedItemCores

        // production -> %prec
        let pprods =
            ProductionUtils.precedenceOfProductions tblCrew.terminals productions

        //优先级应该据此结果给出，不能少，也不应该多。
        let y = []

        Should.equal y pprods

    [<Fact>]
    member _.``05 - list declarations``() =
        let terminals =
            tblCrew.terminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let nonterminals =
            tblCrew.nonterminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let sourceCode =
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

        output.WriteLine(sourceCode)


    [<Fact(
    Skip="按需更新源代码"
    )>]
    member _.``06 - generate FsyaccParseTable``() =
        let fsharpCode = 
            tblCrew
            |> FsyaccParseTableFileUtils.ofSemanticParseTableCrew
            |> FsyaccParseTableFileUtils.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath,fsharpCode,Encoding.UTF8)
        output.WriteLine("output fsyacc:"+parseTblPath)

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        Should.equal tblCrew.encodedActions FsyaccParseTable.actions
        Should.equal tblCrew.encodedClosures FsyaccParseTable.closures

        let prodsFsyacc =
            List.map fst tblCrew.rules

        let prodsParseTable =
            List.map fst FsyaccParseTable.rules
        Should.equal prodsFsyacc prodsParseTable

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",tblCrew.header)

        let semansFsyacc =
            let mappers = 
                tblCrew
                |> FsyaccParseTableFileUtils.ofSemanticParseTableCrew
                |> FsyaccParseTableFileUtils.generateMappers
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            File.ReadAllText(parseTblPath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

