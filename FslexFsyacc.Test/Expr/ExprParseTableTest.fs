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
    let fsyacc = FsyaccFile.parse text

    [<Fact>]
    member _.``0 - compiler test``() =
        show fsyacc.rules
        show fsyacc.precedences
        show fsyacc.declarations

    [<Fact(Skip="once for all!")>] // 
    member _.``1 - expr generateParseTable``() =
        let parseTbl = fsyacc.toFsyaccParseTable()
        let name = "ExprParseTable"
        let moduleName = $"Expr.{name}"

        //解析表数据
        let fsharpCode = parseTbl.generate(moduleName)

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine("output yacc:"+outputDir)

    [<Fact>] // (Skip="once for all!")
    member _.``1 - expr generateParseTable2``() =
        let fsyacc = AlteredFsyaccFile.fromRaw(fsyacc)
        let tbl = fsyacc.toFsyaccParseTable()
        //show tbl

        let name = "ExprParseTable2"
        let moduleName = $"Expr.{name}"

        //解析表数据
        let fsharpCode = tbl.generate(moduleName)

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine($"output yacc:{outputDir}")


    [<Fact>]
    member _.``2 - verify parsing table``() =
        let parseTbl = fsyacc.toFsyaccParseTable()

        Should.equal parseTbl.productions   ExprParseTable.productions
        Should.equal parseTbl.kernelSymbols ExprParseTable.kernelSymbols
        Should.equal parseTbl.actions       ExprParseTable.actions
        Should.equal parseTbl.semantics     ExprParseTable.semantics
        Should.equal parseTbl.declarations  ExprParseTable.declarations

    [<Fact>]
    member _.``3 - all tokens``() =
        let grammar = Grammar.from fsyacc.mainProductions

        let tokens = 
            grammar.symbols - grammar.nonterminals
        //show tokens
        let y = set ["(";")";"*";"+";"-";"/";"NUMBER"]
        Should.equal y tokens

    [<Fact>]
    member _.``4 - ambiguous state details``() =
        let states = 
            AmbiguousCollection.create fsyacc.mainProductions
        show states

    [<Fact>]
    member _.``5 - conflicted items``() =
        let collection = 
            AmbiguousCollection.create fsyacc.mainProductions

        // 显示冲突状态的冲突项目
        let conflictedClosures =
            collection.filterConflictedClosures()

        show conflictedClosures

    [<Fact>]
    member _.``6 - %prec of productions``() =          
        let collection = 
            AmbiguousCollection.create fsyacc.mainProductions

        // 显示冲突状态的冲突项目
        let conflictedClosures =
            collection.filterConflictedClosures() 

        // 提取冲突的产生式
        let productions =
            AmbiguousCollection.gatherProductions conflictedClosures

        let pprods = 
            ProductionUtils.precedenceOfProductions collection.grammar.terminals productions
        show pprods

