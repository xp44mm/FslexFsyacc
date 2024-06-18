namespace FslexFsyacc.RawFsharp

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc
open FslexFsyacc.Dir
open FslexFsyacc.Fsyacc
open FslexFsyacc
open FslexFsyacc.YACCs

open System
open System.IO
open System.Text
open System.Text.RegularExpressions
open Xunit
open Xunit.Abstractions

type ParsTest(output:ITestOutputHelper) =
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "pars.fsyacc")
    let text = File.ReadAllText(filePath, Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler2.compile

    let fsyacc =
        rawFsyacc
        |> FlatFsyaccFile.from

    //let coder = FsyaccParseTableCoder.from fsyacc

    //let tbl =
    //    fsyacc.getYacc()

    [<Fact(
    Skip = "very long time!"
    )>]
    member _.``00 - file test``() =

        let robust = set [
            "error";
            "recover";
            "coming_soon";"COMING_SOON";
            "_IS_HERE"
            "ends_coming_soon_or_recover";"ends_other_than_rparen_coming_soon_or_recover"
            "cPrototype";"exconDefn";"HASH"
            ]

        let heads = set [
            "attributes";"opt_attributes"
            "access";"opt_access"
            "typar";"appType";"atomType";"tupleType";"typ"
            "classDefnMember";"measureTypePower";
            "declExpr";
        ]

        //let s0 = rawFsyacc.ruleGroups.Head.lhs
        let rules =
            fsyacc.rules
            |> RuleSet.removeSymbols robust
            |> RuleSet.removeHeads heads
            |> RuleSet.crawl "fileModuleImpl"
            |> List.map(fun rule -> { rule with reducer = "" })

        let raw = fsyacc.toRaw(rules)

        let src = raw.toCode()
        //output.WriteLine(src)

        let path = Path.Combine(__SOURCE_DIRECTORY__, "pars1.fsyacc")

        File.WriteAllText(path, src, Encoding.UTF8)
        output.WriteLine("output yacc:")
        output.WriteLine(path)

    [<Fact(
    Skip = "very long time!"
    )>]
    member _.``00 - openDecl test``() =
    //https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/import-declarations-the-open-keyword
        let symbols = set [
            "ends_coming_soon_or_recover";
            "TYPE_COMING_SOON";
            "TYPE_IS_HERE"
            ]

        let heads = set [
            "appType"
        ]
        let startSymbol = "openDecl"

        //let s0 = rawFsyacc.ruleGroups.Head.lhs
        let rules =
            fsyacc.rules
            |> RuleSet.removeSymbols symbols
            |> RuleSet.removeHeads heads
            |> RuleSet.crawl startSymbol
            |> List.map(fun rule -> { rule with reducer = "" })

        let raw = fsyacc.toRaw(rules)

        let src = raw.toCode()
        //output.WriteLine(src)

        let path = Path.Combine(__SOURCE_DIRECTORY__, $"{startSymbol}.fsyacc")

        File.WriteAllText(path, src, Encoding.UTF8)
        output.WriteLine("output yacc:")
        output.WriteLine(path)
