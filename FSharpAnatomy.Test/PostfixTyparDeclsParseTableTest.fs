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

    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let fsyaccCrew =
        text
        |> RawFsyaccFileCrewUtils.parse
        |> FlatedFsyaccFileCrewUtils.getFlatedFsyaccFileCrew

    let tblCrew =
        fsyaccCrew
        |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let s0 = tblCrew.startSymbol
        let flatedFsyacc =
            fsyaccCrew
            |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

        let src = 
            flatedFsyacc 
            |> FlatFsyaccFileUtils.start(s0, Set.empty)
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        output.WriteLine(src)


    [<Fact>]
    member _.``02 - list all tokens``() =
        let tokens = tblCrew.terminals
        let res = set ["#";"(";")";"*";",";"->";".";":";":>";";";"<";">";"ARRAY_TYPE_SUFFIX";"HTYPAR";"IDENT";"OPERATOR_NAME";"QTYPAR";"_";"and";"comparison";"delegate";"enum";"equality";"member";"new";"not";"null";"or";"static";"struct";"unmanaged";"when";"{|";"|}"]

        //show tokens
        Should.equal tokens res

    [<Fact>]
    member _.``03 - precedence Of Productions``() =
        let terminals = 
            tblCrew.terminals

        let productions =
            AmbiguousCollectionUtils.collectConflictedProductions tblCrew.conflictedItemCores

        let pprods = 
            ProductionUtils.precedenceOfProductions terminals productions

        Should.equal [] pprods

    [<Fact>]
    member _.``04 - list all states``() =
        let text = 
            tblCrew.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq
            |> AmbiguousCollectionUtils.render 
                tblCrew.terminals
                tblCrew.conflictedItemCores
        output.WriteLine(text)

    [<Fact>]
    member _.``05 - list the type annotaitions``() =
        let terminals =
            tblCrew.terminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let nonterminals =
            tblCrew.nonterminals
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

    [<Fact(
    Skip="once for all!"
    )>] // 
    member _.``06 - generate ParseTable``() =
        let fsharpCode =
            tblCrew
            |> FsyaccParseTableFileUtils.ofSemanticParseTableCrew
            |> FsyaccParseTableFileUtils.generateModule(moduleName)

        File.WriteAllText(parseTblPath,fsharpCode,Encoding.UTF8)
        output.WriteLine("output fsyacc:"+parseTblPath)

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        Should.equal tblCrew.encodedActions PostfixTyparDeclsParseTable.actions
        Should.equal tblCrew.encodedClosures PostfixTyparDeclsParseTable.closures

        let prodsFsyacc =
            List.map fst tblCrew.rules

        let prodsParseTable =
            List.map fst PostfixTyparDeclsParseTable.rules

        Should.equal prodsFsyacc prodsParseTable

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",tblCrew.header)

        let semansFsyacc =
            let mappers = 
                tblCrew
                |> FsyaccParseTableFileUtils.ofSemanticParseTableCrew
                |> FsyaccParseTableFileUtils.generateMappers
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            File.ReadAllText(parseTblPath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

