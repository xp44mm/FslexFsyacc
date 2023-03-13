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
open FSharp.Literals.Literal
open FSharp.Idioms

type TypeArgumentParseTableTest (output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    let fsyaccPath = Path.Combine(Dir.FSharpAnatomyPath,"typeArgument.fsyacc")
    let text = File.ReadAllText(fsyaccPath)

    let parseTblName = "TypeArgumentParseTable"
    let parseTblModule = $"FSharpAnatomy.{parseTblName}"
    let parseTblPath = Path.Combine(Dir.FSharpAnatomyPath, $"{parseTblName}.fs")
    
    let grammar text =
        text
        |> FlatFsyaccFileUtils.parse
        |> FlatFsyaccFileUtils.toGrammar

    let ambiguousCollection text =
        text
        |> FlatFsyaccFileUtils.parse
        |> FlatFsyaccFileUtils.toAmbiguousCollection

    //解析表数据
    let parseTbl text = 
        text
        |> FlatFsyaccFileUtils.parse
        |> FlatFsyaccFileUtils.toFsyaccParseTableFile

    [<Fact>] // 
    member _.``01 - norm fsyacc file``() =
        let fsyacc = 
            text
            |> FlatFsyaccFileUtils.parse

        let s0 = 
            fsyacc.rules
            |> FlatFsyaccFileRule.getStartSymbol

        let src = 
            fsyacc.start(s0, Set.empty)
            |> RawFsyaccFile2Utils.fromFlat
            |> RawFsyaccFile2Utils.render

        output.WriteLine(src)

    [<Fact>]
    member _.``02 - list all tokens``() =
        let grammar = grammar text

        let tokens = grammar.terminals
        let res = set ["#";"(";")";"*";",";"->";".";":";":>";";";"<";">";"IDENT";"HTYPAR";"QTYPAR";"_";"ARRAY_TYPE_SUFFIX";"struct";"{|";"|}"]

        show tokens
        Should.equal tokens res

    [<Fact>]
    member _.``03 - precedence Of Productions``() =
        let collection = ambiguousCollection text

        let terminals = 
            collection.grammar.terminals

        let productions =
            collection.collectConflictedProductions()

        let pprods = 
            ProductionUtils.precedenceOfProductions terminals productions

        Should.equal [] pprods

    [<Fact>]
    member _.``04 - list all states``() =
        let collection = ambiguousCollection text
        
        let text = collection.render()
        output.WriteLine(text)

    [<Fact>]
    member _.``05 - list the type annotaitions``() =
        let grammar = grammar text
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

    [<Fact(Skip="once for all!")>] // 
    member _.``06 - generate ParseTable``() =
        let parseTbl = parseTbl text

        let fsrc = parseTbl.generateModule(parseTblModule)

        File.WriteAllText(parseTblPath,fsrc,Encoding.UTF8)
        output.WriteLine("output fsyacc:"+parseTblPath)

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        let src = parseTbl text

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

    [<Fact>]
    member _.``08 - typeArgument follows test``() =
        let grammar = grammar text
        grammar.follows.["typeArgument"]
        |> Seq.iter(fun tok -> output.WriteLine(stringify tok))


