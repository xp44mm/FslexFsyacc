namespace Expr

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals

open System.IO
open System.Text

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

type ExprParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"expr.fsyacc")
    let text = File.ReadAllText(filePath)
    let rawFsyacc = RawFsyaccFile.parse text
    let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    [<Fact>]
    member _.``00 - compiler test``() =
        show rawFsyacc.rules
        show rawFsyacc.precedences
        show rawFsyacc.declarations

    [<Fact>]
    member _.``01 - render FsyaccFile test``() =
        let fsyacc = rawFsyacc.render()
        output.WriteLine(fsyacc)

    [<Fact>]
    member _.``02 - extract FsyaccFile test``() =
        let fsyacc = fsyacc.start("expr",Set.empty)
        output.WriteLine(Literal.stringify fsyacc)

    [<Fact>]
    member _.``03 - all tokens``() =
        let grammar = 
            fsyacc.getMainProductions() 
            |> Grammar.from
        //show tokens
        let y = set ["(";")";"*";"+";"-";"/";"NUMBER"]
        Should.equal y grammar.terminals

    [<Fact>]
    member _.``04 - ambiguous state details``() =
        let states = 
            fsyacc.getMainProductions() 
            |> AmbiguousCollection.create
        show states

    [<Fact>]
    member _.``05 - conflicted items``() =
        let collection = 
            fsyacc.getMainProductions() 
            |> AmbiguousCollection.create

        // 显示冲突状态的冲突项目
        let conflictedClosures =
            collection.filterConflictedClosures()

        show conflictedClosures

    [<Fact>]
    member _.``06 - %prec of productions``() =
        let collection = 
            fsyacc.getMainProductions() 
            |> AmbiguousCollection.create

        let conflictedClosures =
            collection.filterConflictedClosures() 

        let productions =
            AmbiguousCollection.gatherProductions conflictedClosures

        let pprods = 
            ProductionUtils.precedenceOfProductions collection.grammar.terminals productions
        show pprods

    [<Fact(Skip="once for all!")>] // 
    member _.``07 - expr generateParseTable``() =
        let name = "ExprParseTable"
        let moduleName = $"Expr.{name}"

        //解析表数据
        let tbl = fsyacc.toFsyaccParseTableFile()
        let fsharpCode = tbl.generateModule(moduleName)

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode,Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{outputDir}")


    [<Fact>]
    member _.``10 - valid ParseTable``() =
        let src = fsyacc.toFsyaccParseTableFile()

        Should.equal src.actions ExprParseTable.actions
        Should.equal src.closures ExprParseTable.closures

        //产生式比较
        let prodsFsyacc = 
            List.map fst src.rules

        let prodsParseTable = 
            List.map fst ExprParseTable.rules

        Should.equal prodsFsyacc prodsParseTable

        //header,semantic代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFsyacc =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let filePath = Path.Combine(__SOURCE_DIRECTORY__, "ExprParseTable.fs")
            let text = File.ReadAllText(filePath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans


