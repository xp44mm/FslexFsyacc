namespace FslexFsyacc.Fsyacc

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime
open FslexFsyacc.Dir

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.xUnit
open FSharp.Literals

type FsyaccParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    // ** input **
    let parseTblName = "FsyaccParseTable"
    let parseTblModule = $"FslexFsyacc.Fsyacc.{parseTblName}"
    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fsyacc")
    let filePath = Path.Combine(sourcePath, @"fsyacc.fsyacc")
    let parseTblPath = Path.Combine(sourcePath, $"{parseTblName}.fs")

    let text = File.ReadAllText(filePath,Encoding.UTF8)

    // 与fsyacc文件完全相对应的结构树
    let rawFsyacc = 
        text
        |> RawFsyaccFileUtils.parse 

    let flatedFsyacc = 
        rawFsyacc 
        |> RawFsyaccFileUtils.toFlated

    let grammar (flatedFsyacc) =
        flatedFsyacc
        |> FlatFsyaccFileUtils.toGrammar

    let ambiguousCollection (flatedFsyacc) =
        flatedFsyacc
        |> FlatFsyaccFileUtils.toAmbiguousCollection

    //解析表数据
    let parseTbl (flatedFsyacc) = 
        flatedFsyacc
        //|> FlatFsyaccFileUtils.parse
        |> FlatFsyaccFileUtils.toFsyaccParseTableFile

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let fsyacc = 
            text
            |> FlatFsyaccFileUtils.parse
        // the start symbol of bnf 
        let s0 = 
            fsyacc.rules
            |> FlatFsyaccFileRule.getStartSymbol

        let src = 
            fsyacc.start(s0, Set.empty)
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        output.WriteLine(src)

    [<Fact>]
    member _.``02 - list all tokens``() =
        let grammar = grammar (flatedFsyacc)
        let y = set ["%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"HEADER";"ID";"LITERAL";"SEMANTIC";"TYPE_ARGUMENT";"[";"]";"|"]
        show grammar.terminals
        Should.equal y grammar.terminals

    [<Fact>]
    member _.``03 - list all states``() =
        let collection = ambiguousCollection (flatedFsyacc)
        
        let src = collection.render()
        output.WriteLine(src)

    [<Fact>]
    member _.``04 - precedence Of Productions`` () =
        let collection = ambiguousCollection (flatedFsyacc)

        let productions = 
            collection.collectConflictedProductions()

        // production -> %prec
        let pprods =
            ProductionUtils.precedenceOfProductions collection.grammar.terminals productions

        //优先级应该据此结果给出，不能少，也不应该多。
        let y = []

        Should.equal y pprods

    [<Fact>]
    member _.``05 - list declarations``() =
        let grammar = grammar (flatedFsyacc)

        let terminals =
            grammar.terminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let nonterminals =
            grammar.nonterminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let sourceCode =
            [
                "// Do not list symbols whose return value is always `null`"
                ""
                "// terminals: ref to the returned type of `getLexeme`"
                "%type<> " + terminals
                ""
                "// nonterminals"
                "%type<> " + nonterminals
            ] 
            |> String.concat "\r\n"

        output.WriteLine(sourceCode)


    [<Fact>] // (Skip="once for all!")
    member _.``06 - generate FsyaccParseTable``() =
        let parseTbl = parseTbl (flatedFsyacc)

        let fsharpCode = parseTbl.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath,fsharpCode,Encoding.UTF8)
        output.WriteLine("output fsyacc:"+parseTblPath)

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        let parseTbl = parseTbl (flatedFsyacc)

        Should.equal parseTbl.actions FsyaccParseTable.actions
        Should.equal parseTbl.closures FsyaccParseTable.closures

        let prodsFsyacc =
            List.map fst parseTbl.rules

        let prodsParseTable =
            List.map fst FsyaccParseTable.rules
        Should.equal prodsFsyacc prodsParseTable

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",parseTbl.header)

        let semansFsyacc =
            let mappers = parseTbl.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            File.ReadAllText(parseTblPath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

