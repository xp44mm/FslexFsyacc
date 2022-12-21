namespace FSharpAnatomy

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.xUnit
open FSharp.Literals
open FSharp.Idioms


type TypeArgumentParseTableTest (output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let fsyaccPath = Path.Combine(Dir.FSharpAnatomyPath,"typeArgument.fsyacc")
    let text = File.ReadAllText(fsyaccPath)

    let rawFsyacc = RawFsyaccFile.parse text
    let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    let parseTblName = "TypeArgumentParseTable"
    let parseTblPath = Path.Combine(Dir.FSharpAnatomyPath, $"{parseTblName}.fs")

    [<Fact>] // (Skip="Run manually when required")
    member _.``01 - norm fsyacc file``() =
        let startSymbol = 
            fsyacc.rules
            |> FlatFsyaccFileRule.getStartSymbol
        let fsyacc = fsyacc.start(startSymbol, Set.empty)
        let txt = fsyacc.toRaw().render()
        output.WriteLine(txt)

    [<Fact>]
    member _.``02 - list all tokens``() =
        let grammar =
            fsyacc.getMainProductions()
            |> Grammar.from

        let tokens = grammar.terminals
        let res = set ["#";"(";")";"*";",";"->";".";":";":>";";";"<";">";"IDENT";"INLINE_TYPAR";"TYPAR";"_";"ARRAY_TYPE_SUFFIX";"struct";"{|";"|}"]

        //show tokens
        Should.equal tokens res

    [<Fact>]
    member _.``03 - precedence Of Productions``() =
        let collection = 
            fsyacc.getMainProductions() 
            |> AmbiguousCollection.create

        let terminals = 
            collection.grammar.terminals

        let productions =
            collection.collectConflictedProductions()

        let pprods = 
            ProductionUtils.precedenceOfProductions terminals productions

        Should.equal [] pprods

    [<Fact>]
    member _.``04 - list all states``() =
        let collection =
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create
        
        let text = collection.render()
        output.WriteLine(text)

    [<Fact>]
    member _.``05 - list the type annotaitions``() =
        let grammar =
            fsyacc.getMainProductions()
            |> Grammar.from

        let sourceCode =
            [
                "// Do not list symbols whose return value is always `null`"
                "// terminals: ref to the returned type of getLexeme"
                for i in grammar.terminals do
                    let i = RenderUtils.renderSymbol i
                    i + " : \"\""
                "\r\n// nonterminals"
                for i in grammar.nonterminals do
                    let i = RenderUtils.renderSymbol i
                    i + " : \"\""
            ] 
            |> String.concat "\r\n"

        output.WriteLine(sourceCode)

    [<Fact(Skip="once for all!")>] // 
    member _.``06 - generate ParseTable``() =
        let moduleName = $"FSharpAnatomy.{parseTblName}"

        //解析表数据
        let parseTbl = fsyacc.toFsyaccParseTableFile()
        let fsharpCode = parseTbl.generateModule(moduleName)

        File.WriteAllText(parseTblPath,fsharpCode,Encoding.UTF8)
        output.WriteLine("output fsyacc:"+parseTblPath)

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        let src = fsyacc.toFsyaccParseTableFile()

        Should.equal src.actions TypeArgumentParseTable.actions
        Should.equal src.closures TypeArgumentParseTable.closures

        let prodsFsyacc =
            List.map fst src.rules

        let prodsParseTable =
            List.map fst TypeArgumentParseTable.rules

        Should.equal prodsFsyacc prodsParseTable

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFsyacc =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            File.ReadAllText(parseTblPath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

