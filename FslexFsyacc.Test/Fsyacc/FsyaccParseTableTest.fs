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

    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fsyacc")
    let filePath = Path.Combine(sourcePath, @"fsyacc.fsyacc")
    let text = File.ReadAllText(filePath)
    let rawFsyacc = RawFsyaccFile.parse text
    let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    // ** input **
    let parseTblName = "FsyaccParseTable"
    let parseTblPath = Path.Combine(sourcePath, $"{parseTblName}.fs")

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let startSymbol = 
            fsyacc.rules
            |> FlatFsyaccFileRule.getStartSymbol
        let fsyacc = fsyacc.start(startSymbol, Set.empty)
        let txt = fsyacc.toRaw().render()
        output.WriteLine(txt)

    [<Fact>]
    member _.``02 - list all tokens``() =
        let grammar = 
            fsyacc.getMainProductions() 
            |> Grammar.from

        let y = set [ 
                  ]

        let tokens = grammar.symbols - grammar.nonterminals
        show tokens

    [<Fact>]
    member _.``03 - list all states``() =
        let collection =
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create
        
        let text = collection.render()
        output.WriteLine(text)

    [<Fact>]
    member _.``04 - 汇总冲突的产生式``() =
        let collection =
            fsyacc.getMainProductions ()
            |> AmbiguousCollection.create

        let productions = 
            collection.collectConflictedProductions()

        // production -> %prec
        let pprods =
            ProductionUtils.precedenceOfProductions collection.grammar.terminals productions

        //优先级应该据此结果给出，不能少，也不应该多。
        let y = []

        Should.equal y pprods

    [<Fact>]
    member _.``05 - list the type annotaitions``() =
        let grammar =
            fsyacc.getMainProductions()
            |> Grammar.from

        let sourceCode =
            [
                "// Do not list symbols whose return value is always `null`"
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


    [<Fact()>] // Skip="once for all!"
    member _.``06 - generate Fsyacc2ParseTable ParseTable``() =
        let parseTblModule = $"FslexFsyacc.Fsyacc.{parseTblName}"

        //解析表数据
        let parseTbl = fsyacc.toFsyaccParseTableFile()
        let fsharpCode = parseTbl.generateModule(parseTblModule)

        File.WriteAllText(parseTblPath,fsharpCode,Encoding.UTF8)
        output.WriteLine("output fsyacc:"+parseTblPath)

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        let src = fsyacc.toFsyaccParseTableFile()

        Should.equal src.actions FsyaccParseTable.actions
        Should.equal src.closures FsyaccParseTable.closures

        let prodsFsyacc =
            List.map fst src.rules

        let prodsParseTable =
            List.map fst FsyaccParseTable.rules
        Should.equal prodsFsyacc prodsParseTable

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFsyacc =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            File.ReadAllText(parseTblPath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

