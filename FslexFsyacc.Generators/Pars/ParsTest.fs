namespace FslexFsyacc.Pars

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

    [<Fact(
    Skip = "very long time!"
    )>]
    member _.``01 - fileModuleImpl test``() =
        let startSymbol = "fileModuleImpl"
        let symbols = set [
            //"ends_coming_soon_or_recover";
            //"TYPE_COMING_SOON";
            //"TYPE_IS_HERE"
            ]

        let heads = set [
            //"appType"
            ]

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


    [<Fact(
    Skip = "very long time!"
    )>]
    member _.``02 - openDecl test``() =
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

    [<Fact(
    Skip = "very long time!"
    )>]
    member _.``03 - tyconDefn test``() =
        let startSymbol = "tyconDefn"

        let symbols = set [
            "recover"
            "error"
            "simplePatterns"
            "prefixTyparDecls"
            "tyconDefnAugmentation"

            ]

        let heads = set [
            "opt_attributes";"attributes";
            "opt_access";"access";
            "openDecl"
            "defnBindings"
            "doBinding"
            "exconDefn"
            "hardwhiteLetBindings"
            "moduleIntro";"namedModuleDefnBlock"
        ]

        //let s0 = rawFsyacc.ruleGroups.Head.lhs
        let rules =
            fsyacc.rules
            |> Set.filter(fun rule -> Regex.IsMatch(rule.production.Head,"class",RegexOptions.IgnoreCase)=false)
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

    [<Fact(
    Skip = "very long time!"
    )>]
    member _.``04 - typeNameInfo test``() =
        let startSymbol = "typeNameInfo"

        let symbols = set [
            "ends_coming_soon_or_recover"
            //"recover"
            //"error"
            //"simplePatterns"
            "prefixTyparDecls"
            //"tyconDefnAugmentation"

            ]

        let heads = set [
            "opt_attributes";"attributes";
            "opt_access";"access";
            "opt_typeConstraints"; "typeConstraints"
            ]

        //let s0 = rawFsyacc.ruleGroups.Head.lhs
        let rules =
            fsyacc.rules
            //|> Set.filter(fun rule -> Regex.IsMatch(rule.production.Head,"class",RegexOptions.IgnoreCase)=false)
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

    [<Fact(
    Skip = "very long time!"
    )>]
    member _.``04 - tyconDefnOrSpfnSimpleRepr test``() =
        let startSymbol = "tyconDefnOrSpfnSimpleRepr"

        let symbols = set [
            //"ends_coming_soon_or_recover"
            //"recover"
            //"error"
            //"simplePatterns"
            //"prefixTyparDecls"
            //"tyconDefnAugmentation"

            ]

        let heads = set [
            //"opt_attributes";"attributes";
            //"opt_access";"access";
            //"opt_typeConstraints"; "typeConstraints"
            ]

        //let s0 = rawFsyacc.ruleGroups.Head.lhs
        let rules =
            fsyacc.rules
            //|> Set.filter(fun rule -> Regex.IsMatch(rule.production.Head,"class",RegexOptions.IgnoreCase)=false)
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


    [<Fact(
    Skip = "very long time!"
    )>]
    member _.``05 - typ test``() =
        let startSymbol = "typ"

        let symbols = set [
            //"ends_coming_soon_or_recover"
            //"recover"
            //"error"
            //"simplePatterns"
            //"prefixTyparDecls"
            //"tyconDefnAugmentation"

            ]

        let heads = set [
            "opt_attributes";"attributes";
            "opt_access";"access";
            //"opt_typeConstraints"; "typeConstraints"
            "atomicExpr"
            ]

        //let s0 = rawFsyacc.ruleGroups.Head.lhs
        let rules =
            fsyacc.rules
            //|> Set.filter(fun rule -> Regex.IsMatch(rule.production.Head,"class",RegexOptions.IgnoreCase)=false)
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


    [<Fact(
    Skip = "very long time!"
    )>]
    member _.``05 - tyconDefnRhs test``() =
        let startSymbol = "tyconDefnRhs"

        let symbols = set [
            //"recover"
            //"error"
            //"simplePatterns"
            //"prefixTyparDecls"
            //"tyconDefnAugmentation"

            ]

        let heads = set [
            //"opt_attributes";"attributes";
            //"opt_access";"access";
            //"openDecl"
            //"defnBindings"
            //"doBinding"
            //"exconDefn"
            //"hardwhiteLetBindings"
            //"moduleIntro";"namedModuleDefnBlock"
            ]

        //let s0 = rawFsyacc.ruleGroups.Head.lhs
        let rules =
            fsyacc.rules
            //|> Set.filter(fun rule -> Regex.IsMatch(rule.production.Head,"class",RegexOptions.IgnoreCase)=false)
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



    [<Fact(
    Skip = "very long time!"
    )>]
    member _.``06 - braceFieldDeclList test``() =
        let startSymbol = "braceFieldDeclList"

        //不出现
        let symbols = set [
            "recover"
            "error"
            "RBRACE_COMING_SOON"
            "RBRACE_IS_HERE"

            ]

        //不展开
        let heads = set [
            "opt_attributes";"attributes";
            "opt_access";"access";
            "typ"
            ]

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


