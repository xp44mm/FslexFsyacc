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

type Expr2ParseTableTest(output:ITestOutputHelper) =
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"expr2.fsyacc")
    let text = File.ReadAllText(filePath)

    let parseTblName = "Expr2ParseTable"
    let parseTblPath = Path.Combine(__SOURCE_DIRECTORY__, $"{parseTblName}.fs")
    let parseTblModule = $"FslexFsyacc.Expr.{parseTblName}"

    let r:RawFsyaccFile2 = {header= "";rules= ["expr",[["expr";"+";"expr"],"","s0 + s2";["expr";"-";"expr"],"","s0 - s2";["expr";"*";"expr"],"","s0 * s2";["expr";"/";"expr"],"","s0 / s2";["(";"expr";")"],"","s1";["-";"expr"],"UMINUS","-s1";["NUMBER"],"","s0"]];precedences= ["left",["+";"-"];"left",["*";"/"];"right",["UMINUS"]];declarations= ["float",["NUMBER";"expr"]]}
    let f:FlatFsyaccFile = {rules= [["expr";"expr";"+";"expr"],"","s0 + s2";["expr";"expr";"-";"expr"],"","s0 - s2";["expr";"expr";"*";"expr"],"","s0 * s2";["expr";"expr";"/";"expr"],"","s0 / s2";["expr";"(";"expr";")"],"","s1";["expr";"-";"expr"],"UMINUS","-s1";["expr";"NUMBER"],"","s0"];precedences= Map ["*",199;"+",99;"-",99;"/",199;"UMINUS",301];header= "";declarations= ["NUMBER","float";"expr","float"]}

    [<Fact>]
    member this.``Fsyacc2Compiler``() =
        let rawFsyacc = Fsyacc2Compiler.compile text
        output.WriteLine(stringify rawFsyacc)
        Should.equal r rawFsyacc

    [<Fact>]
    member this.``RawFsyaccFile2Utils flat``() =
        let ffsyacc = RawFsyaccFile2Utils.flat r
        output.WriteLine(stringify ffsyacc)
        Should.equal f ffsyacc

    [<Fact>]
    member this.``RawFsyaccFile2Utils fromFlat``() =
        let rfsyacc = RawFsyaccFile2Utils.fromFlat f
        output.WriteLine(stringify rfsyacc)
        Should.equal r rfsyacc

    [<Fact>]
    member this.``RawFsyaccFile2Utils render``() =
        let rfsyacc = RawFsyaccFile2Utils.render r
        output.WriteLine(rfsyacc)

    [<Fact()>] // Skip="once for all!"
    member _.``generate Parse Table``() =
        let fsyacc = 
            text
            |> Fsyacc2Compiler.compile 
            |> RawFsyaccFile2Utils.flat

        //解析表数据
        let fsharpCode = 
            fsyacc
                .toFsyaccParseTableFile()
                .generateModule(parseTblModule)

        File.WriteAllText(parseTblPath,fsharpCode,Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{parseTblPath}")

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        let rawFsyacc = Fsyacc2Compiler.compile text
        let fsyacc = RawFsyaccFile2Utils.flat rawFsyacc

        let src = fsyacc.toFsyaccParseTableFile()

        Should.equal src.actions Expr2ParseTable.actions
        Should.equal src.closures Expr2ParseTable.closures

        //产生式比较
        let prodsFsyacc = 
            List.map fst src.rules

        let prodsParseTable = 
            List.map fst Expr2ParseTable.rules

        Should.equal prodsFsyacc prodsParseTable

        //header,semantic代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFsyacc =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(parseTblPath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

