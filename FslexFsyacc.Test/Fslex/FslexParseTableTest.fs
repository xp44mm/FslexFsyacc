namespace FslexFsyacc.Fslex

open System
open System.Text
open System.Text.RegularExpressions
open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Idioms
open FSharp.Literals
open FSharp.xUnit

open FslexFsyacc.Yacc
open FslexFsyacc.Fsyacc
open FslexFsyacc.Runtime

type FslexParseTableTest(output: ITestOutputHelper) =
    let show res =
        res |> Literal.stringify |> output.WriteLine

    let solutionPath =
        DirectoryInfo(
            __SOURCE_DIRECTORY__
        )
            .Parent
            .Parent
            .FullName

    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fslex")
    let filePath = Path.Combine(sourcePath, @"fslex.fsyacc")
    let text = File.ReadAllText(filePath)
    let rawFsyacc = RawFsyaccFile.parse text
    let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    let parseTblName = "FslexParseTable"
    let parseTblPath = Path.Combine(sourcePath, $"{parseTblName}.fs")

    [<Fact(Skip="Run manually when required")>]
    member _.``01 - norm fsyacc file``() =
        let startSymbol = 
            fsyacc.rules
            |> FlatFsyaccFileRule.getStartSymbol
        let fsyacc = fsyacc.start(startSymbol, Set.empty)
        let txt = fsyacc.toRaw().render()
        output.WriteLine(txt)

    [<Fact>]
    member _.``02 - list all tokens``() =
        let grammar = fsyacc.getMainProductions () |> Grammar.from

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
            "SEMANTIC"
            "["
            "]"
            "|" 
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
        let y =
            [ [ "expr"; "expr"; "&"; "expr" ], "&"
              [ "expr"; "expr"; "*" ], "*"
              [ "expr"; "expr"; "+" ], "+"
              [ "expr"; "expr"; "?" ], "?"
              [ "expr"; "expr"; "|"; "expr" ], "|" ]

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

    [<Fact(Skip="once for all!")>] // 
    member _.``06 - generate ParseTable``() =
        let moduleName = $"FslexFsyacc.Fslex.{parseTblName}"
        let parseTbl = fsyacc.toFsyaccParseTableFile ()
        let fsharpCode = parseTbl.generateModule(moduleName)
        File.WriteAllText(parseTblPath, fsharpCode, Encoding.UTF8)
        output.WriteLine("output yacc:" + parseTblPath)

    [<Fact>]
    member _.``07 - valid ParseTable``() =
        let src = fsyacc.toFsyaccParseTableFile()

        Should.equal src.actions FslexParseTable.actions
        Should.equal src.closures FslexParseTable.closures

        let prodsFsyacc = 
            List.map fst src.rules

        let prodsParseTable = 
            List.map fst FslexParseTable.rules

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

    [<Fact>]
    member _.``08 - regex first or last token test``() =
        let grammar = 
            fsyacc.getMainProductions () 
            |> Grammar.from

        let lastsOfExpr = grammar.lasts.["expr"]
        let firstsOfExpr = grammar.firsts.["expr"]

        show lastsOfExpr
        show firstsOfExpr

    [<Fact>]
    member _.``101 - format norm file test``() =
        let startSymbol = 
            fsyacc.rules.Head 
            |> Triple.first 
            |> List.head

        //show startSymbol

        let fsyacc = fsyacc.start(startSymbol,Set.empty).toRaw()
        output.WriteLine(fsyacc.render())

