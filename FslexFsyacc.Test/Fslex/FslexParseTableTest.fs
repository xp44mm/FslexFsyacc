namespace FslexFsyacc.Fslex

open System.IO
open System
open System.Text
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit

open FslexFsyacc.Yacc
open FslexFsyacc.Fsyacc

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
    let rawFsyacc = FsyaccFile.parse text
    let fsyacc = NormFsyaccFile.fromRaw rawFsyacc

    [<Fact>]
    member _.``01 - compiler test``() =
        let result = FsyaccCompiler.compile text
        show result

    [<Fact>]
    member _.``02 - 显示冲突状态的冲突项目``() =
        let collection =
            fsyacc.getMainProductions ()
            |> AmbiguousCollection.create

        let conflicts = collection.filterConflictedClosures ()
        //show conflicts
        let y = Map [21,Map ["&",set [{production= ["expr";"expr";"&";"expr"];dot= 1};{production= ["expr";"expr";"&";"expr"];dot= 3}];"*",set [{production= ["expr";"expr";"&";"expr"];dot= 3};{production= ["expr";"expr";"*"];dot= 1}];"+",set [{production= ["expr";"expr";"&";"expr"];dot= 3};{production= ["expr";"expr";"+"];dot= 1}];"?",set [{production= ["expr";"expr";"&";"expr"];dot= 3};{production= ["expr";"expr";"?"];dot= 1}];"|",set [{production= ["expr";"expr";"&";"expr"];dot= 3};{production= ["expr";"expr";"|";"expr"];dot= 1}]];22,Map ["&",set [{production= ["expr";"expr";"&";"expr"];dot= 1};{production= ["expr";"expr";"|";"expr"];dot= 3}];"*",set [{production= ["expr";"expr";"*"];dot= 1};{production= ["expr";"expr";"|";"expr"];dot= 3}];"+",set [{production= ["expr";"expr";"+"];dot= 1};{production= ["expr";"expr";"|";"expr"];dot= 3}];"?",set [{production= ["expr";"expr";"?"];dot= 1};{production= ["expr";"expr";"|";"expr"];dot= 3}];"|",set [{production= ["expr";"expr";"|";"expr"];dot= 1};{production= ["expr";"expr";"|";"expr"];dot= 3}]]]
        Should.equal y conflicts

    [<Fact>]
    member _.``03 - 汇总冲突的产生式``() =
        let collection =
            fsyacc.getMainProductions ()
            |> AmbiguousCollection.create

        let conflicts = collection.filterConflictedClosures ()

        let productions = AmbiguousCollection.gatherProductions conflicts
        // production -> %prec
        let pprods =
            ProductionUtils.precedenceOfProductions collection.grammar.terminals productions
            |> List.ofArray
        //优先级应该据此结果给出，不能少，也不应该多。
        let y =
            [ [ "expr"; "expr"; "&"; "expr" ], "&"
              [ "expr"; "expr"; "*" ], "*"
              [ "expr"; "expr"; "+" ], "+"
              [ "expr"; "expr"; "?" ], "?"
              [ "expr"; "expr"; "|"; "expr" ], "|" ]

        Should.equal y pprods

    [<Fact>]
    member _.``04 - print the template of type annotaitions``() =
        let grammar = fsyacc.getMainProductions () |> Grammar.from

        let symbols =
            grammar.symbols
            |> Set.filter (fun x -> Regex.IsMatch(x, @"^\w+$"))

        let sourceCode =
            [ for i in symbols do
                  i + " \"\";" ]
            |> String.concat "\r\n"

        output.WriteLine(sourceCode)

    [<Fact>]
    member _.``05 - list all tokens``() =
        let grammar = fsyacc.getMainProductions () |> Grammar.from

        let y =
            set [ "%%"
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
                  "QUOTE"
                  "SEMANTIC"
                  "["
                  "]"
                  "|" ]

        let tokens = grammar.symbols - grammar.nonterminals
        show tokens

    [<Fact(Skip="once for all!")>] // 
    member _.``06 - generate ParseTable``() =
        let name = "FslexParseTable"
        let moduleName = $"FslexFsyacc.Fslex.{name}"
        let fsyacc = NormFsyaccFile.fromRaw rawFsyacc
        let parseTbl = fsyacc.toFsyaccParseTableFile ()
        let fsharpCode = parseTbl.generate (moduleName)
        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir, fsharpCode, System.Text.Encoding.UTF8)
        output.WriteLine("output yacc:" + outputDir)

    [<Fact>]
    member _.``08 - regex first or last token test``() =
        let grammar = fsyacc.getMainProductions () |> Grammar.from

        let lastsOfExpr = grammar.lasts.["expr"]
        let firstsOfExpr = grammar.firsts.["expr"]

        show lastsOfExpr
        show firstsOfExpr

    [<Fact>]
    member _.``10 - valid ParseTable``() =
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
            let filePath = Path.Combine(sourcePath, "FslexParseTable.fs")

            File.ReadAllText(filePath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans


