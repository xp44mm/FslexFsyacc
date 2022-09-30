namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals

open System.IO
open System.Text

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

type ParsParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"pars.fsyacc")
    let text = File.ReadAllText(filePath)
    let rawFsyacc = RawFsyaccFile.parse text
    let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    [<Fact>]
    member _.``00 - compiler test``() =
        show rawFsyacc.rules
        show rawFsyacc.precedences
        show rawFsyacc.declarations

    [<Fact>]
    member _.``01 - render FsyaccFile test``() =
        let fsyacc = rawFsyacc.render()
        output.WriteLine(fsyacc)

    [<Fact>]
    member _.``02 - render test``() =
        let fsyacc = rawFsyacc.render()
        output.WriteLine(fsyacc)

    [<Fact>]
    member _.``03 - topType test``() =
        let typ = fsyacc.start("topType",set [
            "attributes"; "atomicExpr"])
        let txt = typ.toRaw().render()
        output.WriteLine(txt)

    [<Fact>]
    member _.``04 - topType without action test``() =
        let typ = fsyacc.start("topType",set [
            "attributes"; 
            "atomicRationalConstant"
            "appTypePrefixArguments"
            "atomicExpr"
            ])

        let robust = [
            "error";
            "recover";
            "coming_soon";"COMING_SOON";
            ]

        let willBeRemoved (symbol: string) =
            robust
            |> List.exists(fun kw -> symbol.Contains kw)

        let reserve prod =
            prod 
            |> List.forall(fun (symbol:string) -> not(willBeRemoved symbol))

        let typ = 
            {
                typ with
                    rules = 
                        typ.rules
                        |> List.map(fun(prod,nm,ac)->prod,"","")
                        |> List.filter(fun(prod,nm,ac)->reserve prod)
            }

        let typ = typ.start("topType",set [])
        let txt = typ.toRaw().render()
        output.WriteLine(txt)

        
    [<Fact>]
    member _.``05 - eliminate test``() =
        let txt = fsyacc.eliminate("bar_rbrace").toRaw().render()
        output.WriteLine(txt)

    [<Fact>]
    member _.``06 - implementationFile test``() =
        let flat = fsyacc.start("implementationFile",set [])

        let robust = set [
            "error";
            "recover";
            "coming_soon";"COMING_SOON";
            "_IS_HERE"
            ]

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> FsyaccFileRules.removeErrorRules robust
                        |> List.map(fun(p,nm,act)-> p,"","")
            }

        let txt = flat.toRaw().render()
        output.WriteLine(txt)

    [<Fact>]
    member _.``07 - getChomsky test``() =
        let flat = fsyacc.start("implementationFile",set [])

        let robust = set [
            "error";
            "recover";
            "coming_soon";"COMING_SOON";
            "_IS_HERE"
            ]

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> FsyaccFileRules.removeErrorRules robust
                        |> List.map(fun(p,nm,act)-> p,"","")
            }

        let grammar =
            flat.getMainProductions()
            |> Grammar.from

        let terminals = grammar.terminals

        let chos =
            FsyaccFileRules.getChomsky terminals flat.rules

        let txt = 
            chos
            |> List.map( FslexFsyacc.Runtime.RenderUtils.renderProduction)
            |> String.concat "\r\n"
        output.WriteLine(txt)

    [<Fact>]
    member _.``08 - eliminateChomsky test``() =
        let flat = fsyacc.start("implementationFile",set [])

        let robust = set [
            "error";
            "recover";
            "coming_soon";"COMING_SOON";
            "_IS_HERE"
            ]

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> FsyaccFileRules.removeErrorRules robust
                        |> List.map(fun(p,nm,act)-> p,"","")
                        |> FsyaccFileRules.eliminateChomsky
            }
        let txt = flat.toRaw().render()
        output.WriteLine(txt)






