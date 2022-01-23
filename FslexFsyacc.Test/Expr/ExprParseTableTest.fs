namespace Expr

open Xunit
open Xunit.Abstractions

open System.IO

open FSharp.xUnit
open FSharp.Literals
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
    member _.``0 - compiler test``() =
        show rawFsyacc.rules
        show rawFsyacc.precedences
        show rawFsyacc.declarations

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
        //show tbl
        let fsharpCode = tbl.generate(moduleName)

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine($"output yacc:\r\n{outputDir}")


    [<Fact>]
    member _.``6 - valid ParseTable``() =
        let t = fsyacc.toFsyaccParseTableFile()

        Should.equal t.header       ExprParseTable.header
        Should.equal t.rules        ExprParseTable.rules
        Should.equal t.actions      ExprParseTable.actions
        Should.equal t.closures     ExprParseTable.closures
        Should.equal t.declarations ExprParseTable.declarations


