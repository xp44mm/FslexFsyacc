namespace FslexFsyacc

open Xunit
open Xunit.Abstractions
open System
open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.Idioms
open FSharp.Literals
open FSharp.xUnit

open FslexFsyacc.Yacc
open FslexFsyacc.Fsyacc

type IfElseParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "ifelse.fsyacc")
    let text = File.ReadAllText(filePath)
    let rawFsyacc = FsyaccFile.parse text
    let fsyacc = NormFsyaccFile.fromRaw rawFsyacc

    [<Fact>]
    member _.``1 - 显示冲突状态的冲突项目``() =
        let collection =
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create

        let conflicts =
            collection.filterConflictedClosures()

        //show conflicts
        // state -> lookahead -> conflicted items
        let y = Map [
            7,Map [
            "else",set [
            {production= ["Statement";"if";"(";"Expression";")";"Statement"];dot= 5};
            {production= ["Statement";"if";"(";"Expression";")";"Statement";"else";"Statement"];dot= 5}
            ]]]

        Should.equal y conflicts

    [<Fact>]
    member _.``2 - 汇总冲突的产生式``() =
        let collection =
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create
        let conflicts =
            collection.filterConflictedClosures()

        let productions =
            AmbiguousCollection.gatherProductions conflicts
        // production -> %prec
        let pprods =
            productions
            |> ProductionUtils.precedenceOfProductions collection.grammar.terminals
            |> List.ofArray

        //优先级应该据此结果给出，不能少，也不应该多。
        let y = [
            ["Statement";"if";"(";"Expression";")";"Statement"],")";
            ["Statement";"if";"(";"Expression";")";"Statement";"else";"Statement"],"else"
            ]

        Should.equal y pprods

    [<Fact>]
    member _.``5 - verify parsing table``() =
        let parseTbl = fsyacc.toFsyaccParseTableFile()
        show parseTbl