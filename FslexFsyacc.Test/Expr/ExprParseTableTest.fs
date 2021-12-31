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
    member _.``1 - fsyacc generateParseTable``() =
        let parseTbl = fsyacc.toFsyaccParseTable()
        let name = "ExprParseTable"
        let moduleName = $"Expr.{name}"

        //解析表数据
        let fsharpCode = parseTbl.generate(moduleName)

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine("output yacc:"+outputDir)

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

