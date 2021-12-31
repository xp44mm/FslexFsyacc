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

    [<Fact>]
    member _.``0 - compiler test``() =
        let result = FsyaccCompiler.compile text
        show result

    [<Fact>]
    member _.``1 - 产生式冲突``() =
        let tbl = AmbiguousTable.create fsyacc.mainProductions
        let pconflicts = ConflictFactory.productionConflict tbl.ambiguousTable
        show pconflicts
        Assert.True(pconflicts.IsEmpty)

    [<Fact>]
    member _.``2 - 符号多用警告``() =
        let tbl = AmbiguousTable.create fsyacc.mainProductions
        let warning = ConflictFactory.overloadsWarning tbl
        show warning
        let y = []
        Should.equal y warning

    [<Fact>]
    member _.``3 - 优先级冲突``() =
        let tbl = AmbiguousTable.create fsyacc.mainProductions
        let srconflicts = ConflictFactory.shiftReduceConflict tbl

        show srconflicts
        let y = set [
            set [["expr";"expr";"&";"expr"]];
            set [["expr";"expr";"&";"expr"];["expr";"expr";"*"]];
            set [["expr";"expr";"&";"expr"];["expr";"expr";"+"]];
            set [["expr";"expr";"&";"expr"];["expr";"expr";"?"]];
            set [["expr";"expr";"&";"expr"];["expr";"expr";"|";"expr"]];
            set [["expr";"expr";"*"];["expr";"expr";"|";"expr"]];
            set [["expr";"expr";"+"];["expr";"expr";"|";"expr"]];
            set [["expr";"expr";"?"];["expr";"expr";"|";"expr"]];
            set [["expr";"expr";"|";"expr"]]]
        Should.equal y srconflicts

    [<Fact>]
    member _.``4 - print the template of type annotaitions``() =
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
    member _.``5 - list all tokens``() =
        let grammar = Grammar.from fsyacc.mainProductions
        let y = set ["%%";"&";"(";")";"*";"+";"/";"=";"?";"CAP";"HEADER";"HOLE";"ID";"QUOTE";"SEMANTIC";"[";"]";"|"]
        
        let tokens = grammar.symbols - grammar.nonterminals
        show tokens

    [<Fact(Skip="once for all!")>] // 
    member _.``6 - generate ParseTable``() =
        let name = "FslexParseTable"
        let moduleName = $"FslexFsyacc.Fslex.{name}"
        //解析表数据
        let parseTbl = fsyacc.toFsyaccParseTable()
        let fsharpCode = parseTbl.generate(moduleName)

        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine("output yacc:"+outputDir)

    [<Fact>]
    member _.``7 - valid ParseTable``() =
        let t = fsyacc.toFsyaccParseTable()

        Should.equal t.header        FslexParseTable.header
        Should.equal t.productions   FslexParseTable.productions
        Should.equal t.actions       FslexParseTable.actions
        Should.equal t.kernelSymbols FslexParseTable.kernelSymbols
        Should.equal t.semantics     FslexParseTable.semantics
        Should.equal t.declarations  FslexParseTable.declarations

    [<Fact>]
    member _.``8 - regex first or last token test``() =
        let grammar = Grammar.from fsyacc.mainProductions
        let lastsOfExpr = grammar.lasts.["expr"]
        let firstsOfExpr = grammar.firsts.["expr"]

        show lastsOfExpr
        show firstsOfExpr