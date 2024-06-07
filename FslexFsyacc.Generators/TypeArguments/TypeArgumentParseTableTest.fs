namespace FslexFsyacc.TypeArguments

open FslexFsyacc.Fsyacc
open FslexFsyacc.YACCs
open FslexFsyacc

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.xUnit
open FSharp.Idioms.Literal
open FSharp.Idioms

type TypeArgumentParseTableTest (output:ITestOutputHelper) =

    let parseTblName = "TypeArgumentParseTable"
    let parseTblModule = $"FslexFsyacc.TypeArguments.{parseTblName}"
    let parseTblPath = Path.Combine(Dir.bootstrap, "TypeArguments", $"{parseTblName}.fs")

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "typeArgument.fsyacc")
    let text = File.ReadAllText(filePath, Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler2.compile

    let fsyacc =
        rawFsyacc
        |> FlatFsyaccFile.from

    let coder = FsyaccParseTableCoder.from fsyacc

    let tbl =
        fsyacc.getYacc()

    let bnf = tbl.bnf


    //[<Fact>]
    //member _.``01 - norm fsyacc file``() =
    //    let s0 = tblCrew.startSymbol
    //    let flatedFsyacc =
    //        fsyaccCrew
    //        |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

    //    let src = 
    //        flatedFsyacc 
    //        |> FlatFsyaccFileUtils.start s0
    //        |> RawFsyaccFileUtils.fromFlat
    //        |> RawFsyaccFileUtils.render

    //    output.WriteLine(src)

    //[<Fact>]
    //member _.``02 - list all tokens``() =
    //    let tokens = tblCrew.terminals
    //    let res = set ["#";"(";")";"*";",";"->";".";":";":>";";";"<";">";"IDENT";"HTYPAR";"QTYPAR";"_";"ARRAY_TYPE_SUFFIX";"struct";"{|";"|}"]

    //    show tokens
    //    Should.equal tokens res

    [<Fact>]
    member _.``02 - print conflict productions``() =
        let st = ConflictedProduction.from fsyacc.rules
        if st.IsEmpty then
            output.WriteLine("no conflict")

        for cp in st do
        output.WriteLine($"{stringify cp}")

    //[<Fact>]
    //member _.``04 - list all states``() =        
    //    let text = 
    //        tblCrew.kernels 
    //        |> Seq.mapi(fun i k -> k,i) 
    //        |> Map.ofSeq
    //        |> AmbiguousCollectionUtils.render 
    //            tblCrew.terminals
    //            tblCrew.conflictedItemCores
    //    output.WriteLine(text)

    //[<Fact>]
    //member _.``05 - list the type annotaitions``() =
    //    let terminals =
    //        tblCrew.terminals
    //        |> Seq.map RenderUtils.renderSymbol
    //        |> String.concat " "

    //    let nonterminals =
    //        tblCrew.nonterminals
    //        |> Seq.map RenderUtils.renderSymbol
    //        |> String.concat " "

    //    let sourceCode =
    //        [
    //            "// Do not list symbols whose return value is always `null`"
    //            ""
    //            "// terminals: ref to the returned type of `getLexeme`"
    //            "%type<> " + terminals
    //            ""
    //            "// nonterminals"
    //            "%type<> " + nonterminals
    //        ] 
    //        |> String.concat "\r\n"

    //    output.WriteLine(sourceCode)

    [<Fact(
    Skip="once for all!"
    )>]
    member _.``06 - generate ParseTable``() =
        let outp = coder.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, outp, Encoding.UTF8)
        output.WriteLine("output yacc:")
        output.WriteLine($"{parseTblPath}")
        
    [<Fact>]
    member _.``10 - valid ParseTable``() =
        Should.equal coder.tokens TypeArgumentParseTable.tokens
        Should.equal coder.kernels TypeArgumentParseTable.kernels
        Should.equal coder.actions TypeArgumentParseTable.actions

        //产生式比较
        let prodsFsyacc =
            fsyacc.rules
            |> Seq.map (fun rule -> rule.production)
            |> Seq.toList

        let prodsParseTable =
            TypeArgumentParseTable.rules
            |> List.map fst

        Should.equal prodsFsyacc prodsParseTable

        //header,semantic代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",fsyacc.header)

        let semansFsyacc =
            let mappers = coder.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(parseTblPath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 4 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

    [<Fact>]
    member _.``08 - typeArgument follows test``() =
        bnf.follows.["typeArgument"]
        |> Seq.iter(fun tok -> output.WriteLine(stringify tok))


