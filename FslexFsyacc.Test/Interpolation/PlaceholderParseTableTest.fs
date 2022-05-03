namespace Interpolation

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open System.IO
open System.Text

open FSharp.Literals

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

type PlaceholderParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "placeholder.fsyacc")
    let text = File.ReadAllText(filePath)
    let rawFsyacc = FsyaccFile.parse text
    let fsyacc = NormFsyaccFile.fromRaw rawFsyacc

    [<Fact>]
    member _.``00 - all terminals``() =
        let grammar = 
            fsyacc.getMainProductions() 
            |> Grammar.from

        show grammar.terminals
        let y = set ["(";")";"*";"+";"-";"/";"NUMBER";"}"]
        Should.equal y grammar.terminals

    [<Fact>]
    member _.``01 - all conflicts in coreitems of state``() =
        let collection =
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create

        let conflicts =
            collection.filterConflictedClosures()

        // 显示冲突状态的冲突项目
        show conflicts
        let y = Map [8,Map ["*",set [{production= ["expr";"-";"expr"];dot= 2};{production= ["expr";"expr";"*";"expr"];dot= 1}];"+",set [{production= ["expr";"-";"expr"];dot= 2};{production= ["expr";"expr";"+";"expr"];dot= 1}];"-",set [{production= ["expr";"-";"expr"];dot= 2};{production= ["expr";"expr";"-";"expr"];dot= 1}];"/",set [{production= ["expr";"-";"expr"];dot= 2};{production= ["expr";"expr";"/";"expr"];dot= 1}]];10,Map ["*",set [{production= ["expr";"expr";"*";"expr"];dot= 1};{production= ["expr";"expr";"*";"expr"];dot= 3}];"+",set [{production= ["expr";"expr";"*";"expr"];dot= 3};{production= ["expr";"expr";"+";"expr"];dot= 1}];"-",set [{production= ["expr";"expr";"*";"expr"];dot= 3};{production= ["expr";"expr";"-";"expr"];dot= 1}];"/",set [{production= ["expr";"expr";"*";"expr"];dot= 3};{production= ["expr";"expr";"/";"expr"];dot= 1}]];11,Map ["*",set [{production= ["expr";"expr";"*";"expr"];dot= 1};{production= ["expr";"expr";"+";"expr"];dot= 3}];"+",set [{production= ["expr";"expr";"+";"expr"];dot= 1};{production= ["expr";"expr";"+";"expr"];dot= 3}];"-",set [{production= ["expr";"expr";"+";"expr"];dot= 3};{production= ["expr";"expr";"-";"expr"];dot= 1}];"/",set [{production= ["expr";"expr";"+";"expr"];dot= 3};{production= ["expr";"expr";"/";"expr"];dot= 1}]];12,Map ["*",set [{production= ["expr";"expr";"*";"expr"];dot= 1};{production= ["expr";"expr";"-";"expr"];dot= 3}];"+",set [{production= ["expr";"expr";"+";"expr"];dot= 1};{production= ["expr";"expr";"-";"expr"];dot= 3}];"-",set [{production= ["expr";"expr";"-";"expr"];dot= 1};{production= ["expr";"expr";"-";"expr"];dot= 3}];"/",set [{production= ["expr";"expr";"-";"expr"];dot= 3};{production= ["expr";"expr";"/";"expr"];dot= 1}]];13,Map ["*",set [{production= ["expr";"expr";"*";"expr"];dot= 1};{production= ["expr";"expr";"/";"expr"];dot= 3}];"+",set [{production= ["expr";"expr";"+";"expr"];dot= 1};{production= ["expr";"expr";"/";"expr"];dot= 3}];"-",set [{production= ["expr";"expr";"-";"expr"];dot= 1};{production= ["expr";"expr";"/";"expr"];dot= 3}];"/",set [{production= ["expr";"expr";"/";"expr"];dot= 1};{production= ["expr";"expr";"/";"expr"];dot= 3}]]]
        Should.equal y conflicts

    [<Fact>]
    member _.``02 - the productions that should provide prec``() =
        let collection =
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create

        let conflicts =
            collection.filterConflictedClosures()

        let productions =
            AmbiguousCollection.gatherProductions conflicts
        // production -> %prec
        let pprods =
            ProductionUtils.precedenceOfProductions collection.grammar.terminals productions
            |> List.ofArray
        show pprods
        // production -> tip
        let y = [
            ["expr";"-";"expr"]," - 0";
            ["expr";"expr";"-";"expr"]," - 1";
            ["expr";"expr";"*";"expr"],"*";
            ["expr";"expr";"+";"expr"],"+";
            ["expr";"expr";"/";"expr"],"/"]

        //优先级应该据此结果给出，不能少，也不应该多。
        Should.equal y pprods

    [<Fact(Skip="once for all!")>] // 
    member _.``03 - generate ParseTable``() =
        let name = "PlaceholderParseTable"
        let moduleName = $"Interpolation.{name}"

        //解析表数据
        let tbl = fsyacc.toFsyaccParseTableFile()
        //show tbl
        let fsharpCode = tbl.generateX(moduleName)

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode,Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{outputDir}")

    [<Fact>] // (Skip="once for all!")
    member _.``04 - printClosures``() =
        let tbl = fsyacc.toFsyaccParseTableFile()
        //show tbl
        let x = tbl.printClosures()
        output.WriteLine(x)

    //[<Fact>]
    //member _.``6 - valid ParseTable``() =
    //    let t = fsyacc.toFsyaccParseTableFile()

    //    Should.equal t.header       ExprParseTable.header
    //    Should.equal t.rules        ExprParseTable.rules
    //    Should.equal t.actions      ExprParseTable.actions
    //    Should.equal t.closures     ExprParseTable.closures
    //    Should.equal t.declarations ExprParseTable.declarations


