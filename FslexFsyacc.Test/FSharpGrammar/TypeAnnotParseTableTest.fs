﻿namespace FslexFsyacc.FSharpGrammar

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.xUnit
open FSharp.Literals

type TypeAnnotParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let sourcePath = __SOURCE_DIRECTORY__
    let fsyaccPath = Path.Combine(sourcePath,"typeAnnot.fsyacc")

    let text = File.ReadAllText(fsyaccPath)
    let rawFsyacc = RawFsyaccFile.parse text
    let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    [<Fact>]
    member _.``001 - rawFsyacc fsyacc test``() =
        let txt = rawFsyacc.render()
        //output.WriteLine(txt)
        let fff = fsyacc.toRaw().render()
        output.WriteLine(fff)

    [<Fact>]
    member _.``002 - list all tokens``() =
        let grammar =
            fsyacc.getMainProductions()
            |> Grammar.from

        let tokens = grammar.terminals
        let res = set ["#";"'";"(";")";"*";",";"->";".";":";":>";";";"<";">";
        "IDENT";"^";"_";"struct";"{\";\"?}";"{(\"[\" \",\"* \"]\")}";"{|";"|}"]

        show tokens

    [<Fact>]
    member _.``003 - 显示冲突状态的冲突项目``() =
        let collection =
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create
        let text = collection.render()
        output.WriteLine(text)

        //Should.equal conflictedClosures m

    [<Fact>]
    member _.``04 - print the template of type annotaitions``() =
        let grammar =
            fsyacc.getMainProductions()
            |> Grammar.from

        let symbols =
            grammar.symbols
            |> Set.filter(fun x -> Regex.IsMatch(x,@"^\w+$"))

        let sourceCode =
            [
                for i in symbols do
                    i + " : \"\""
            ] 
            |> String.concat "\r\n"

        output.WriteLine(sourceCode)


    [<Fact(Skip="once for all!")>] // 
    member _.``06 - generate Fsyacc2ParseTable ParseTable``() =
        // ** input **
        let name = "Fsyacc2ParseTable"
        let moduleName = $"FslexFsyacc.Fsyacc.{name}"

        //解析表数据
        let parseTbl = fsyacc.toFsyaccParseTableFile()
        let fsharpCode = parseTbl.generateModule(moduleName)

        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode,System.Text.Encoding.UTF8)
        output.WriteLine("output fsyacc:"+outputDir)

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        let src = fsyacc.toFsyaccParseTableFile()

        Should.equal src.actions Fsyacc2ParseTable.actions
        Should.equal src.closures Fsyacc2ParseTable.closures

        let prodsFsyacc =
            List.map fst src.rules

        let prodsParseTable =
            List.map fst Fsyacc2ParseTable.rules
        Should.equal prodsFsyacc prodsParseTable

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFsyacc =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let filePath = Path.Combine(sourcePath, "Fsyacc2ParseTable.fs")

            File.ReadAllText(filePath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

    [<Fact(Skip="once for all!")>] // 
    member _.``101 - regularSymbol extract test``() =
        let startSymbol = "regularSymbol"
        let fsyacc = fsyacc.start(startSymbol,Set.empty)

        // ** input **
        let fname = $"{System.Char.ToUpper startSymbol.[0]}{startSymbol.[1..]}ParseTable" //Capital Case
        let moduleName = $"FslexFsyacc.Fsyacc.{fname}"

        //解析表数据
        let parseTbl = fsyacc.toFsyaccParseTableFile()
        let fsharpCode = parseTbl.generateModule(moduleName)

        let outputDir = Path.Combine(sourcePath, $"{fname}.fs")
        File.WriteAllText(outputDir,fsharpCode,Encoding.UTF8)
        output.WriteLine("output fsyacc:"+outputDir)