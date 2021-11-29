namespace FslexFsyacc.Fslex

open System.IO
open System

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit

open FslexFsyacc.Yacc
open FslexFsyacc.Fsyacc
open System.Text.RegularExpressions

type FslexParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.Parent.FullName
    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fslex")
    let filePath = Path.Combine(sourcePath, @"fslex.fsyacc")
    let text = File.ReadAllText(filePath)
    let fsyacc = FsyaccFile.parse text
    let parseTbl = fsyacc.toFsyaccParseTable()

    [<Fact(Skip="once for all!")>] // 
    member this.``0 - generate ParseTable``() =
        let name = "FslexParseTable"
        let moduleName = $"FslexFsyacc.Fslex.{name}"
        //解析表数据
        let fsharpCode = parseTbl.generateParseTable(moduleName)

        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine("output yacc:"+outputDir)

    [<Fact>]
    member this.``1 - valid ParseTable``() =
        let t = parseTbl

        Should.equal t.header        FslexParseTable.header
        Should.equal t.productions   FslexParseTable.productions
        Should.equal t.actions       FslexParseTable.actions
        Should.equal t.kernelSymbols FslexParseTable.kernelSymbols
        Should.equal t.semantics     FslexParseTable.semantics
        Should.equal t.declarations  FslexParseTable.declarations

    [<Fact>]
    member this.``3 - print the template of type annotaitions``() =
        let grammar = Grammar.from fsyacc.mainProductions

        let symbols = grammar.symbols |> Set.filter(fun x -> Regex.IsMatch(x,@"^\w+$"))

        let sourceCode = 
            [
                for i in symbols do
                    i + " \"\";"
            ] |> String.concat "\r\n"
        output.WriteLine(sourceCode)

    [<Fact>]
    member this.``5 - list all tokens``() =
        let grammar = Grammar.from fsyacc.mainProductions

        let tokens = grammar.symbols - grammar.nonterminals
        show tokens

    [<Fact>]
    member this.``8 - regex first or last token test``() =
        let grammar = Grammar.from fsyacc.mainProductions
        let lastsOfExpr = grammar.lasts.["expr"]
        let firstsOfExpr = grammar.firsts.["expr"]

        show lastsOfExpr
        show firstsOfExpr