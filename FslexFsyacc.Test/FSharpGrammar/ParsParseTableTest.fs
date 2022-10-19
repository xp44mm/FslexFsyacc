namespace FslexFsyacc.FSharpGrammar

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Literals.Literal
open FSharp.Idioms

open System.IO
open System.Text

open type System.String

(*

pars.fsyacc
来源:pars.fsy
删除头部插入代码
删除声明部分代码
替换/**/为(**)

%start 
    signatureFile 
    implementationFile 
    interaction 
    typedSequentialExprEOF 
    typEOF

*)


(*

// 修改产生式：
opt_topSeparators: 
  | topSeparator opt_topSeparators { }
为：
opt_topSeparators: 
  | topSeparators { }

opt_staticOptimizations,同上问题
*)

(*
rename interactiveExpr -> aaDeclExpr
*)

(*
duplication of name: identOrOp/identExpr
别名：nameop
合并|QMARK nameop和|identExpr为同一动作。

*)

type ParsParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "pars.fsyacc")
    let text = File.ReadAllText(filePath)
    let rawFsyacc = RawFsyaccFile.parse text
    let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    let removeErrorRules =
        let robust = set [
            "error";
            "recover";
            "coming_soon";"COMING_SOON";
            "_IS_HERE"
            ]
        FsyaccFileRules.removeErrorRules robust

    [<Fact>]
    member _.``000 - fsharp keywords replace tsv gen``() =
        let keywords = ["yield";"with";"while";"when";"void";"val";"use";"upcast";"type";"try";"to";"then";"struct";"static";"select";"return";"rec";"public";"private";"override";"or";"open";"of";"null";"not";"new";"namespace";"mutable";"module";"member";"match";"let";"lazy";"internal";"interface";"inline";"inherit";"in";"if";"global";"function";"fun";"for";"fixed";"finally";"extern";"exception";"end";"else";"elif";"downto";"downcast";"done";"do";"delegate";"default";"const";"class";"begin";"base";"assert";"as";"and";"abstract"]

        let tsvText =
            keywords
            |> List.map(fun kw ->
                let fields = [
                    "on"
                    kw.ToUpper()
                    kw
                    " C  W "
                    ""
                ]
                fields |> String.concat "\t"
            )
            |> String.concat "\r\n"

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"fsharp keywords.tsv")
        File.WriteAllText(outputDir,tsvText,Encoding.UTF8)
        output.WriteLine("output:\r\n" + outputDir)


    [<Fact>]
    member _.``001 - signatureFile test``() =
        let s0 = "signatureFile"
        let terminals = set [
            "attributes"
            "hashDirective"
            "activePatternCaseNames"
            "typar"
            "typeConstraint"
            "topType"
            "declExpr"
            "typ"
            "unionTypeRepr"
            "braceFieldDeclList"
            "tyconClassSpfn"
            "classMemberSpfn"
            "unionCaseRepr"
            "appType"
            //"moduleSpfn"
            //"hashDirective"
        ]

        let flat = fsyacc.start(s0,terminals)

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> removeErrorRules
                        |> FsyaccFileRules.eliminateChomsky
                        |> List.map (fun(prod,nm,ac)->prod,"","")
            }

        let txt = flat.toRaw().render()

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n"+outputDir)

    [<Fact>]
    member _.``002 - implementationFile test``() =
        let s0 = "implementationFile"
        let terminals = set [
            "attributes"
            "hashDirective"
            //"declExpr"
            "defnBindings"
            "hardwhiteLetBindings"
            "doBinding"
            "tyconDefn"
            "tyconDefnList"
            "exconDefn"
            "appType"

            "sequentialExpr"
            "typ"
            "typeConstraint"

            "anonMatchingExpr"
            "anonLambdaExpr"
            "withClauses"
            "ifExprCases"
            "forLoopBinder"
            "forLoopRange"
            "headBindingPattern"
            "minusExpr"

            //"parenPattern"
        ]

        let flat = fsyacc.start(s0,terminals)

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> removeErrorRules
                        |> FsyaccFileRules.eliminateChomsky
                        |> List.map (fun(prod,nm,ac)->prod,"","")
            }

        let txt = flat.toRaw().render()

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n" + outputDir)

    [<Fact>]
    member _.``003 - interaction test``() =
        let s0 = "interaction"
        let terminals = set [
            "attributes"
            "moduleDefn"
            "hashDirective"
            "declExpr"
        ]

        let flat = fsyacc.start(s0,terminals)

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> removeErrorRules
                        |> FsyaccFileRules.eliminateChomsky
                        |> List.map (fun(prod,nm,ac)->prod,"","")
            }

        let txt = flat.toRaw().render()

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n"+outputDir)

    [<Fact>]
    member _.``004 - typedSequentialExprEOF test``() =
        let s0 = "typedSequentialExprEOF"
        let terminals = set [
            "declExpr"
            "hardwhiteLetBindings"
            "typeWithTypeConstraints"
        ]

        let flat = fsyacc.start(s0, terminals)

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> removeErrorRules
                        |> FsyaccFileRules.eliminateChomsky
                        |> List.map (fun(prod,nm,ac)->prod,"","")
            }

        let txt = flat.toRaw().render()

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n"+outputDir)

    [<Fact>]
    member _.``005 - typEOF test``() =
        let s0 = "typEOF"
        let terminals = set [
            "tupleType"
        ]

        let flat = fsyacc.start(s0, terminals)

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> removeErrorRules
                        |> FsyaccFileRules.eliminateChomsky
                        |> List.map (fun(prod,nm,ac)->prod,"","")
            }

        let txt = flat.toRaw().render()

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n"+outputDir)

    [<Fact>]
    member _.``006 - moduleDefnsOrExpr test``() =
        let s0 = "moduleDefnsOrExpr"
        let terminals = set [
            "attributes"
            "localBindings";"cPrototype"
            "typeWithTypeConstraints"
            "patternClauses"
            "atomicPatterns"
            "parenPattern"
            "headBindingPattern"
            "typ"
            "atomType";"appTypePrefixArguments";"appTypeConPower";
            "atomicExprAfterType";
            "atomicExprQualification"
            "typeArgActual"
            "opName"
            "tyconDefn";
            "exconIntro"
            "classDefnMember"
            "appType"
            "hashDirective"
        ]

        let flat = fsyacc.start(s0, terminals)

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> removeErrorRules
                        |> FsyaccFileRules.eliminateChomsky
                        |> List.map (fun(prod,nm,ac)->prod,"","")
            }

        let txt = flat.toRaw().render()

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n"+outputDir)


    [<Fact>]
    member _.``101 - declExpr test``() =
        let s0 = "declExpr"
        let terminals = set [
            "attributes"
            "cPrototype"
            "constrPattern"
            "tuplePatternElements"
            "conjPatternElements"
            "topType"
            "typeConstraints"
            "sequentialExpr"
            "typ"
            "typar"
            "parenPattern"
            "measureTypeExpr"
            "appTypePrefixArguments"
            "appTypeConPower"
            "anonRecdType"
            "atomType"
            "appType"
            "classMemberSpfn"
            "inlineAssemblyExpr"
            "recdExpr"
            "objExpr"
            "recdExprCore"
            "interpolatedString"
            "appExpr"
        ]

        let flat = fsyacc.start(s0, terminals)

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> removeErrorRules
                        |> FsyaccFileRules.eliminateChomsky
                        |> List.map (fun(prod,nm,ac)->prod,"","")
            }

        let txt = flat.toRaw().render()

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n"+outputDir)


    [<Fact>]
    member _.``102 - moduleDefn test``() =
        let s0 = "moduleDefn"
        let terminals = set [
            "attributes"
            "cPrototype"; "hashDirective"

            "localBinding"
            "sequentialExpr"

            "typ";"typeConstraint"; "appType"

            "tyconDefn"
            "exconDefn"

            "declExpr"
        ]

        let flat = fsyacc.start(s0,terminals)

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> removeErrorRules
                        |> FsyaccFileRules.eliminateChomsky
                        |> List.map (fun(prod,nm,ac)->prod,"","")
            }

        let txt = flat.toRaw().render()

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n" + outputDir)


    [<Fact>]
    member _.``103 - hashDirective test``() =
        let s0 = "hashDirective"
        let terminals = set [
            ]

        let flat = fsyacc.start(s0,terminals)

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> removeErrorRules
                        |> FsyaccFileRules.eliminateChomsky
                        |> List.map (fun(prod,nm,ac)->prod,"","")
            }

        let txt = flat.toRaw().render()

        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n" + outputDir)







