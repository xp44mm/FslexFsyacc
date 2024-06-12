namespace FslexFsyacc.RawFsharp

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc
open FslexFsyacc.Dir
open FslexFsyacc.Fsyacc
open FslexFsyacc
open FslexFsyacc.YACCs

open System
open System.IO
open System.Text
open System.Text.RegularExpressions
open Xunit
open Xunit.Abstractions

type ParsTest(output:ITestOutputHelper) =
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "pars.fsyacc")
    let text = File.ReadAllText(filePath, Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler2.compile

    let fsyacc =
        rawFsyacc
        |> FlatFsyaccFile.from

    //let coder = FsyaccParseTableCoder.from fsyacc

    //let tbl =
    //    fsyacc.getYacc()

    //[<Fact>]
    member _.``00 - file test``() =
        let s0 = fsyacc
        output.WriteLine(sprintf "%A" s0)

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

    //[<Fact>]
    //member _.``02 - print conflict productions``() =
    //    let st = ConflictedProduction.from fsyacc.rules
    //    if st.IsEmpty then
    //        output.WriteLine("no conflict")
    //    for cp in st do
    //    output.WriteLine($"{stringify cp}")

    //[<Fact(
    //Skip="按需更新源代码"
    //)>]
    //member _.``06 - generate FsyaccParseTable``() =
    //    let outp = coder.generateModule(moduleName)        
    //    File.WriteAllText(modulePath, outp, Encoding.UTF8)
    //    output.WriteLine("output yacc:")
    //    output.WriteLine(modulePath)

    //[<Fact>]
    //member _.``10 - valid ParseTable``() =
    //    Should.equal coder.tokens FsyaccParseTable2.tokens
    //    Should.equal coder.kernels FsyaccParseTable2.kernels
    //    Should.equal coder.actions FsyaccParseTable2.actions

    //    //产生式比较
    //    let prodsFsyacc =
    //        fsyacc.rules
    //        |> Seq.map (fun rule -> rule.production)
    //        |> Seq.toList

    //    let prodsParseTable =
    //        FsyaccParseTable2.rules
    //        |> List.map fst

    //    Should.equal prodsFsyacc prodsParseTable

    //    //header,reducers代码比较
    //    let headerFromFsyacc =
    //        FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",fsyacc.header)

    //    let semansFsyacc =
    //        let mappers = coder.generateMappers()
    //        FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

    //    let header,semans =
    //        let text = File.ReadAllText(modulePath, Encoding.UTF8)
    //        FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 4 text

    //    Should.equal headerFromFsyacc header
    //    Should.equal semansFsyacc semans

