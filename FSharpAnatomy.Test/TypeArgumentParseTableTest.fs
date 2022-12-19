﻿namespace FSharpAnatomy

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.xUnit
open FSharp.Literals

type TypeArgumentParseTableTest (output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let fsyaccPath = Path.Combine(Dir.FSharpAnatomyPath,"typeArgument.fsyacc")
    let text = File.ReadAllText(fsyaccPath)

    let rawFsyacc = RawFsyaccFile.parse text
    let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    [<Fact>]
    member _.``01 - typeArgument test``() =
        let s0 = "typeArgument"
        let terminals = set []
        let fsyacc = fsyacc.start(s0,terminals)

        let txt = fsyacc.toRaw().render()
        //let outputDir = Path.Combine(Dir.FSharpAnatomyPath, $"{s0}.fsyacc")
        //File.WriteAllText(outputDir,txt,Encoding.UTF8)
        //output.WriteLine("output:\r\n" + outputDir)
        output.WriteLine(txt)

    [<Fact>]
    member _.``02 - list all tokens``() =
        let grammar =
            fsyacc.getMainProductions()
            |> Grammar.from

        let tokens = grammar.terminals
        let res = set ["#";"(";")";"*";",";"->";".";":";":>";";";"<";">";"IDENT";"INLINE_TYPAR";"TYPAR";"_";"ARRAY_TYPE_SUFFIX";"struct";"{|";"|}"]

        //show tokens
        Should.equal tokens res

    [<Fact>]
    member _.``03 - precedence Of Productions``() =
        let collection = 
            fsyacc.getMainProductions() 
            |> AmbiguousCollection.create

        let terminals = 
            collection.grammar.terminals

        let productions =
            collection.collectConflictedProductions()

        let pprods = 
            ProductionUtils.precedenceOfProductions terminals productions

        Should.equal [] pprods


    [<Fact>]
    member _.``04 - list all states``() =
        let collection =
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create
        
        let text = collection.render()
        output.WriteLine(text)

    [<Fact>]
    member _.``05 - print the template of type annotaitions``() =
        let grammar =
            fsyacc.getMainProductions()
            |> Grammar.from

        let sourceCode =
            [
                "// terminals: ref to the returned type of getLexeme"
                for i in grammar.terminals do
                    let i = RenderUtils.renderSymbol i
                    i + " : \"\""
                "\r\n// nonterminals"
                for i in grammar.nonterminals do
                    let i = RenderUtils.renderSymbol i
                    i + " : \"\""
            ] 
            |> String.concat "\r\n"

        output.WriteLine(sourceCode)


    [<Fact(Skip="once for all!")>] // 
    member _.``06 - generate TypeArgumentParseTable``() =
        // ** input **
        let name = "TypeArgumentParseTable"
        let moduleName = $"FSharpAnatomy.{name}"

        //解析表数据
        let parseTbl = fsyacc.toFsyaccParseTableFile()
        let fsharpCode = parseTbl.generateModule(moduleName)

        let outputDir = Path.Combine(Dir.FSharpAnatomyPath, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode,Encoding.UTF8)
        output.WriteLine("output fsyacc:"+outputDir)

    //[<Fact>]
    //member _.``10 - valid ParseTable``() =
    //    let src = fsyacc.toFsyaccParseTableFile()

    //    Should.equal src.actions TypeArgumentParseTable.actions
    //    Should.equal src.closures TypeArgumentParseTable.closures

    //    let prodsFsyacc =
    //        List.map fst src.rules

    //    let prodsParseTable =
    //        List.map fst TypeArgumentParseTable.rules
    //    Should.equal prodsFsyacc prodsParseTable

    //    let headerFromFsyacc =
    //        FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

    //    let semansFsyacc =
    //        let mappers = src.generateMappers()
    //        FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

    //    let header,semans =
    //        let filePath = Path.Combine(sourcePath, "TypeArgumentParseTable.fs")

    //        File.ReadAllText(filePath, Encoding.UTF8)
    //        |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

    //    Should.equal headerFromFsyacc header
    //    Should.equal semansFsyacc semans

