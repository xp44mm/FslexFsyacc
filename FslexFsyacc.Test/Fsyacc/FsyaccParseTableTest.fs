namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.xUnit
open FSharp.Literals

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

type FsyaccParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine
    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.Parent.FullName
    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fsyacc")
    let filePath = Path.Combine(sourcePath, @"fsyacc.fsyacc")
    let text = File.ReadAllText(filePath)
    let rawFsyacc = FsyaccFile.parse text
    let fsyacc = NormFsyaccFile.fromRaw rawFsyacc

    [<Fact>]
    member _.``01 - compiler test``() =
        let result = FsyaccCompiler.compile text
        show result

    [<Fact>]
    member _.``02 - extract FsyaccFile test``() =
        let fsyacc = rawFsyacc.start("rules",Set.empty).render()
        output.WriteLine(fsyacc)

    [<Fact>]
    member _.``03 - 显示冲突状态的冲突项目``() =
        let collection =
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create

        let conflictedClosures =
            collection.filterConflictedClosures()

        //show conflictedClosures
        Should.equal conflictedClosures Map.empty

    [<Fact>]
    member _.``04 - print the template of type annotaitions``() =
        let grammar =
            fsyacc.getMainProductions()
            |>          Grammar.from

        let symbols =
            grammar.symbols
            |> Set.filter(fun x -> Regex.IsMatch(x,@"^\w+$"))

        let sourceCode =
            [
                for i in symbols do
                    i + " \"\";"
            ] |> String.concat "\r\n"
        output.WriteLine(sourceCode)

    [<Fact>]
    member _.``05 - list all tokens``() =
        let grammar =
            fsyacc.getMainProductions()
            |> Grammar.from

        let tokens = grammar.symbols - grammar.nonterminals
        show tokens

    [<Fact(Skip="once for all!")>] //
    member _.``06 - generate ParseTable``() =
        // ** input **
        let name = "FsyaccParseTable"
        let moduleName = $"FslexFsyacc.Fsyacc.{name}"

        //解析表数据
        let parseTbl = fsyacc.toFsyaccParseTableFile()
        let fsharpCode = parseTbl.generate(moduleName)

        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode,System.Text.Encoding.UTF8)
        output.WriteLine("output yacc:"+outputDir)

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        let src = fsyacc.toFsyaccParseTableFile()

        Should.equal src.actions FsyaccParseTable.actions
        Should.equal src.closures FsyaccParseTable.closures

        let prodsFsyacc =
            Array.map fst src.rules

        let prodsParseTable =
            Array.map fst FsyaccParseTable.rules

        Should.equal prodsFsyacc prodsParseTable

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFsyacc =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let filePath = Path.Combine(sourcePath, "FsyaccParseTable.fs")

            File.ReadAllText(filePath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans


