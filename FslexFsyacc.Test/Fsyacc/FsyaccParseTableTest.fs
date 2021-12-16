namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open System.IO

open FSharp.xUnit
open FSharp.Literals
open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open System.Text.RegularExpressions

type FsyaccParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine
    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.Parent.FullName
    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fsyacc")
    let filePath = Path.Combine(sourcePath, @"fsyacc.fsyacc")
    let text = File.ReadAllText(filePath)
    let fsyacc = FsyaccFile.parse text

    [<Fact>]
    member this.``0 - compiler test``() =
        let result = FsyaccCompiler.compile text
        show result

    [<Fact>]
    member this.``1 - 产生式冲突``() =
        let tbl = AmbiguousTable.create fsyacc.mainProductions
        let pconflicts = ConflictFactory.productionConflict tbl.ambiguousTable
        show pconflicts
        Assert.True(pconflicts.IsEmpty)

    [<Fact>]
    member this.``2 - 符号多用警告``() =
        let tbl = AmbiguousTable.create fsyacc.mainProductions
        let warning = ConflictFactory.overloadsWarning tbl
        show warning
        let y = [            set [
                ["file";"HEADER";"rules";"%%";"declarations"];
                ["file";"HEADER";"rules";"%%";"precedences"];
                ["file";"HEADER";"rules";"%%";"precedences";"%%";"declarations"]
                ]]
        Should.equal y warning

    [<Fact>]
    member this.``3 - 优先级冲突``() =
        let tbl = AmbiguousTable.create fsyacc.mainProductions
        let srconflicts = ConflictFactory.shiftReduceConflict tbl

        show srconflicts
        let y = Set.empty
        Should.equal y srconflicts

    [<Fact>]
    member this.``4 - print the template of type annotaitions``() =
        
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
    member this.``5 - list all tokens``() =
        let grammar = Grammar.from fsyacc.mainProductions

        let tokens = grammar.symbols - grammar.nonterminals
        show tokens


    [<Fact(Skip="once for all!")>] // 
    member this.``6 - generate ParseTable``() =
        let parseTbl = fsyacc.toFsyaccParseTable()

        let name = "FsyaccParseTable"
        let moduleName = $"FslexFsyacc.Fsyacc.{name}"

        //解析表数据
        let fsharpCode = parseTbl.generate(moduleName)

        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine("output yacc:"+outputDir)

    [<Fact>]
    member this.``7 - valid ParseTable``() =
        let t = fsyacc.toFsyaccParseTable()

        Should.equal t.header        FsyaccParseTable.header
        Should.equal t.productions   FsyaccParseTable.productions
        Should.equal t.actions       FsyaccParseTable.actions
        Should.equal t.kernelSymbols FsyaccParseTable.kernelSymbols
        Should.equal t.semantics     FsyaccParseTable.semantics
        Should.equal t.declarations  FsyaccParseTable.declarations

