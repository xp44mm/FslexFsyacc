namespace FslexFsyacc.Fslex

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc
open FslexFsyacc.Fsyacc
open FslexFsyacc
open FslexFsyacc.YACCs
open System
open System.IO
open System.Text
open System.Text.RegularExpressions
open Xunit
open Xunit.Abstractions

type FslexParseTableTest(output: ITestOutputHelper) =
    let name = "FslexParseTable"
    let moduleName = $"FslexFsyacc.Fslex.{name}"
    let modulePath = Path.Combine(Dir.bootstrap, "Fslex", $"{name}.fs")

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "fslex.fsyacc")
    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler2.compile

    let fsyacc =
        rawFsyacc
        |> FlatFsyaccFile.from

    let coder = FsyaccParseTableCoder.from fsyacc

    let tbl =
        fsyacc.getYacc()


    [<Fact>]
    member _.``02 - list all tokens``() =
        let y = set [ 
            "%%"
            "&"
            "("
            ")"
            "*"
            "+"
            "/"
            "="
            "?"
            "CAP"
            "HEADER"
            "HOLE"
            "ID"
            "LITERAL"
            "REDUCER"
            "["
            "]"
            "|" 
            ]
        Should.equal y tbl.bnf.terminals

    [<Fact>]
    member _.``02 - print conflict productions``() =
        let st = ConflictedProduction.from fsyacc.rules
        if st.IsEmpty then
            output.WriteLine("no conflict")
        for cp in st do
        output.WriteLine($"{stringify cp}")

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

    //    let src =
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

    //    output.WriteLine(src)

    [<Fact(
    Skip="按需更新源代码"
    )>]
    member _.``06 - generate ParseTable``() =
        let outp = coder.generateModule(moduleName)
        File.WriteAllText(modulePath, outp, Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{modulePath}")

    [<Fact>]
    member _.``07 - valid ParseTable``() =
        Should.equal coder.tokens FslexParseTable.tokens
        Should.equal coder.kernels FslexParseTable.kernels
        Should.equal coder.actions FslexParseTable.actions

        //产生式比较
        let prodsFsyacc =
            fsyacc.rules
            |> Seq.map (fun rule -> rule.production)
            |> Seq.toList

        let prodsParseTable =
            FslexParseTable.rules
            |> List.map fst

        Should.equal prodsFsyacc prodsParseTable

        //header,reducers代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",fsyacc.header)

        let semansFsyacc =
            let mappers = coder.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(modulePath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 4 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

