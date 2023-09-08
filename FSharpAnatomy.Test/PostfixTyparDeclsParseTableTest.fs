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

type PostfixTyparDeclsParseTableTest (output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let parseTblName = "PostfixTyparDeclsParseTable"
    let moduleName = $"FSharpAnatomy.{parseTblName}"
    let filePath = Path.Combine(Dir.FSharpAnatomyPath,"postfixTyparDecls.fsyacc")
    let parseTblPath = Path.Combine(Dir.FSharpAnatomyPath, $"{parseTblName}.fs")


    //let text = File.ReadAllText(fsyaccPath)

    ////let rawFsyacc = RawFsyaccFile.parse text
    ////let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc


    //let grammar text =
    //    text
    //    |> FlatFsyaccFileUtils.parse
    //    |> FlatFsyaccFileUtils.toGrammar

    //let ambiguousCollection text =
    //    text
    //    |> FlatFsyaccFileUtils.parse
    //    |> FlatFsyaccFileUtils.toAmbiguousCollection

    ////解析表数据
    //let parseTbl text = 
    //    text
    //    |> FlatFsyaccFileUtils.parse
    //    |> FlatFsyaccFileUtils.toFsyaccParseTableFile

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

        //let startSymbol = 
        //    fsyacc.rules
        //    |> FlatFsyaccFileRule.getStartSymbol
        //let fsyacc = fsyacc.start(startSymbol, Set.empty)
        //let txt = fsyacc.toRaw().render()
        //output.WriteLine(txt)
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
        let grammar = grammar flatedFsyacc

        let tokens = grammar.terminals
        let res = set ["#";"(";")";"*";",";"->";".";":";":>";";";"<";">";"ARRAY_TYPE_SUFFIX";"HTYPAR";"IDENT";"OPERATOR_NAME";"QTYPAR";"_";"and";"comparison";"delegate";"enum";"equality";"member";"new";"not";"null";"or";"static";"struct";"unmanaged";"when";"{|";"|}"]

        //show tokens
        Should.equal tokens res

    [<Fact>]
    member _.``03 - precedence Of Productions``() =
        let collection = ambiguousCollection flatedFsyacc

        let terminals = 
            collection.grammar.terminals

        let productions =
            collection.collectConflictedProductions()

        let pprods = 
            ProductionUtils.precedenceOfProductions terminals productions

        Should.equal [] pprods

    [<Fact>]
    member _.``04 - list all states``() =
        let collection = ambiguousCollection flatedFsyacc
        
        let text = collection.render()
        output.WriteLine(text)

    [<Fact>]
    member _.``05 - list the type annotaitions``() =
        let grammar = grammar flatedFsyacc

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
        let parseTbl = parseTbl flatedFsyacc
        let fsharpCode = parseTbl.generateModule(moduleName)

        File.WriteAllText(parseTblPath,fsharpCode,Encoding.UTF8)
        output.WriteLine("output fsyacc:"+parseTblPath)

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        let src = parseTbl flatedFsyacc

        Should.equal src.actions PostfixTyparDeclsParseTable.actions
        Should.equal src.closures PostfixTyparDeclsParseTable.closures

        let prodsFsyacc =
            List.map fst src.rules

        let prodsParseTable =
            List.map fst PostfixTyparDeclsParseTable.rules

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

