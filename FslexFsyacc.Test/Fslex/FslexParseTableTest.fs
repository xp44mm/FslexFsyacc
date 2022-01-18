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
    let rawFsyacc = FsyaccFile.parse text
    let fsyacc = AlteredFsyaccFile.fromRaw rawFsyacc

    [<Fact>]
    member _.``0 - compiler test``() =
        let result = FsyaccCompiler.compile text
        show result

    [<Fact>]
    member _.``1 - 显示冲突状态的冲突项目``() =
        let collection =
            AmbiguousCollection.create fsyacc.mainProductions
        let conflicts =
            collection.filterConflictedClosures()
        //show conflicts
        let y = Map[
            21,Map[
                "&",set[
                    {production=["expr";"expr";"&";"expr"];dot=1};
                    {production=["expr";"expr";"&";"expr"];dot=3}];
                "*",set[
                    {production=["expr";"expr";"&";"expr"];dot=3};
                    {production=["expr";"expr";"*"];dot=1}];
                "+",set[
                    {production=["expr";"expr";"&";"expr"];dot=3};
                    {production=["expr";"expr";"+"];dot=1}];
                "?",set[
                    {production=["expr";"expr";"&";"expr"];dot=3};
                    {production=["expr";"expr";"?"];dot=1}];
                "|",set[
                    {production=["expr";"expr";"&";"expr"];dot=3};
                    {production=["expr";"expr";"|";"expr"];dot=1}]];
            22,Map[
                "&",set[
                    {production=["expr";"expr";"&";"expr"];dot=1};
                    {production=["expr";"expr";"|";"expr"];dot=3}];
                "*",set[
                    {production=["expr";"expr";"*"];dot=1};
                    {production=["expr";"expr";"|";"expr"];dot=3}];
                "+",set[
                    {production=["expr";"expr";"+"];dot=1};
                    {production=["expr";"expr";"|";"expr"];dot=3}];
                "?",set[
                    {production=["expr";"expr";"?"];dot=1};
                    {production=["expr";"expr";"|";"expr"];dot=3}];
                "|",set[
                    {production=["expr";"expr";"|";"expr"];dot=1};
                    {production=["expr";"expr";"|";"expr"];dot=3}]]
                    ]
        Should.equal y conflicts

    [<Fact>]
    member _.``2 - 汇总冲突的产生式``() =
        let collection =
            AmbiguousCollection.create fsyacc.mainProductions
        let conflicts =
            collection.filterConflictedClosures()

        let productions =
            AmbiguousCollection.gatherProductions conflicts
        // production -> %prec
        let pprods =
            ProductionUtils.precedenceOfProductions collection.grammar.terminals productions
            |> List.ofArray
        //优先级应该据此结果给出，不能少，也不应该多。
        let y = [
            ["expr";"expr";"&";"expr"],"&";
            ["expr";"expr";"*"],"*";
            ["expr";"expr";"+"],"+";
            ["expr";"expr";"?"],"?";
            ["expr";"expr";"|";"expr"],"|"
            ]

        Should.equal y pprods

    [<Fact>]
    member _.``3 - print the template of type annotaitions``() =
        let grammar = Grammar.from fsyacc.mainProductions

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
    member _.``4 - list all tokens``() =
        let grammar = Grammar.from fsyacc.mainProductions
        let y = set ["%%";"&";"(";")";"*";"+";"/";"=";"?";"CAP";"HEADER";"HOLE";"ID";"QUOTE";"SEMANTIC";"[";"]";"|"]

        let tokens = grammar.symbols - grammar.nonterminals
        show tokens

    [<Fact(Skip="once for all!")>] //
    member _.``5 - generate ParseTable``() =
        let name = "FslexParseTable"
        let moduleName = $"FslexFsyacc.Fslex.{name}"
        let parseTbl = fsyacc.toFsyaccParseTable()
        let fsharpCode = parseTbl.generate(moduleName)
        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine("output yacc:"+outputDir)

    [<Fact>]
    member _.``7 - valid ParseTable``() =
        let t = fsyacc.toFsyaccParseTable()

        Should.equal t.header       FslexParseTable.header
        Should.equal t.productions  FslexParseTable.productions
        Should.equal t.actions      FslexParseTable.actions
        Should.equal t.closures     FslexParseTable.closures
        Should.equal t.semantics    FslexParseTable.semantics
        Should.equal t.declarations FslexParseTable.declarations

    [<Fact>]
    member _.``8 - regex first or last token test``() =
        let grammar = Grammar.from fsyacc.mainProductions
        let lastsOfExpr = grammar.lasts.["expr"]
        let firstsOfExpr = grammar.firsts.["expr"]

        show lastsOfExpr
        show firstsOfExpr