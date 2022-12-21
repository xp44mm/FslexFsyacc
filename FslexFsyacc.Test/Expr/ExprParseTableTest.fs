namespace Expr

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals

open System.IO
open System.Text

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime

type ExprParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"expr.fsyacc")
    let text = File.ReadAllText(filePath)
    let rawFsyacc = RawFsyaccFile.parse text
    let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    let parseTblName = "ExprParseTable"
    let parseTblPath = Path.Combine(__SOURCE_DIRECTORY__, $"{parseTblName}.fs")

    [<Fact(Skip="Run manually when required")>]
    member _.``01 - norm fsyacc file``() =
        let startSymbol = 
            fsyacc.rules
            |> FlatFsyaccFileRule.getStartSymbol
        let fsyacc = fsyacc.start(startSymbol, Set.empty)
        let txt = fsyacc.toRaw().render()
        output.WriteLine(txt)

    [<Fact>]
    member _.``02 - extract FsyaccFile test``() =
        let fsyacc = fsyacc.start("expr",Set.empty)
        let outp = fsyacc.toRaw().render()
        output.WriteLine(outp)

    [<Fact>]
    member _.``03 - list tokens``() =
        let grammar = 
            fsyacc.getMainProductions() 
            |> Grammar.from
        //show tokens
        let y = set ["(";")";"*";"+";"-";"/";"NUMBER"]
        Should.equal y grammar.terminals

    [<Fact>]
    member _.``04 - precedence Of Productions``() =
        let collection = 
            fsyacc.getMainProductions() 
            |> AmbiguousCollection.create

        let terminals = 
            collection.grammar.terminals

        let productions =
            collection.collectConflictedProductions()

        let pprods = 
            ProductionUtils.precedenceOfProductions terminals productions

        //show pprods

        //( production, dummy token)
        let expected =
            [
                ["expr";"-";"expr"]," - (0 of 2)";
                ["expr";"expr";"-";"expr"]," - (1 of 2)";
                ["expr";"expr";"*";"expr"],"*";
                ["expr";"expr";"+";"expr"],"+";
                ["expr";"expr";"/";"expr"],"/"
                ]

        Should.equal expected pprods

    [<Fact>]
    member _.``05 - ambiguous state details``() =
        let col = 
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create
        output.WriteLine(col.render())

    [<Fact(Skip="once for all!")>] // 
    member _.``07 - expr generateParseTable``() =
        let moduleName = $"Expr.{parseTblName}"

        //解析表数据
        let tbl = fsyacc.toFsyaccParseTableFile()
        let fsharpCode = tbl.generateModule(moduleName)

        File.WriteAllText(parseTblPath,fsharpCode,Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{parseTblPath}")


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
            let text = File.ReadAllText(parseTblPath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans


