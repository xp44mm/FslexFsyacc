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

type FsyaccParseTable1Test(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    // ** input **
    let sourceFile = "fsyacc1.fsyacc"
    let parseTblName = "FsyaccParseTable1"

    let parseTblModule = $"FslexFsyacc.Fsyacc.{parseTblName}"
    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fsyacc")
    let filePath = Path.Combine(sourcePath, sourceFile)
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


    [<Fact(
    //Skip="按需更新源代码"
    )>]
    member _.``06 - generate FsyaccParseTable``() =
        let outp = moduleFile.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, outp, Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{parseTblPath}")

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        Should.equal tbl.encodeActions  FsyaccParseTable1.actions
        Should.equal tbl.encodeClosures FsyaccParseTable1.closures

        //产生式比较
        let prodsFsyacc =
            fsyacc.rules
            |> Seq.map (fun rule -> rule.production)
            |> Seq.toList

        let prodsParseTable =
            FsyaccParseTable1.rules
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

