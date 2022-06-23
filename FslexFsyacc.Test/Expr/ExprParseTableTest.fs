﻿namespace Expr

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
    let rawFsyacc = FsyaccFile.parse text
    let fsyacc = NormFsyaccFile.fromRaw rawFsyacc

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
        let fsyacc = rawFsyacc.start("expr",Set.empty)
        output.WriteLine(Literal.stringify fsyacc)

    [<Fact>]
    member _.``1 - all tokens``() =
        let grammar = 
            fsyacc.getMainProductions() 
            |> Grammar.from

        //show tokens
        let y = set ["(";")";"*";"+";"-";"/";"NUMBER"]
        Should.equal y grammar.terminals

    [<Fact>]
    member _.``2 - ambiguous state details``() =
        let states = 
            fsyacc.getMainProductions() 
            |> AmbiguousCollection.create
        show states

    [<Fact>]
    member _.``3 - conflicted items``() =
        let collection = 
            fsyacc.getMainProductions() 
            |> AmbiguousCollection.create

        // 显示冲突状态的冲突项目
        let conflictedClosures =
            collection.filterConflictedClosures()

        show conflictedClosures

    [<Fact>]
    member _.``4 - %prec of productions``() =
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
    member _.``5 - expr generateParseTable``() =
        let name = "ExprParseTable"
        let moduleName = $"Expr.{name}"

        //解析表数据
        let tbl = fsyacc.toFsyaccParseTableFile()
        let fsharpCode = tbl.generate(moduleName)

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode,Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{outputDir}")

    [<Fact>] // (Skip="once for all!")
    member _.``501 - expr generateParseTable``() =
        let name = "ExprParseTable2"
        let moduleName = $"Expr.{name}"

        //解析表数据
        let tbl = fsyacc.toFsyaccParseTableFile()
        let fsharpCode = tbl.generate(moduleName)

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode,Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{outputDir}")

    [<Fact>]
    member _.``6 - valid ParseTable``() =
        let t = fsyacc.toFsyaccParseTableFile()

        //Should.equal t.header       ExprParseTable2.header
        //Should.equal t.rules        ExprParseTable2.rules
        Should.equal t.actions      ExprParseTable2.actions
        Should.equal t.closures     ExprParseTable2.closures
        //Should.equal t.declarations ExprParseTable2.declarations

    [<Fact>]
    member _.``7 - output closures``() =
        let tbl = ExprParseTable2.parser.getParserTable()
        let str = tbl.collection()
        let name = "expr"
        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.txt")
        File.WriteAllText(outputDir,str,Encoding.UTF8)
        output.WriteLine($"output:\r\n{outputDir}")
