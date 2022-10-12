namespace FslexFsyacc.FSharpGrammar

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Literals.Literal
open FSharp.Idioms

open System.IO
open System.Text

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

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
        output.WriteLine("output fsyacc:"+outputDir)

    [<Fact>]
    member _.``002 - implementationFile test``() =
        let s0 = "implementationFile"
        let terminals = set [
            "attributes"
            "declExpr"
            "moduleDefn"
            "hashDirective"

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
        output.WriteLine("output fsyacc:"+outputDir)

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
        output.WriteLine("output fsyacc:"+outputDir)


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
        output.WriteLine("output fsyacc:"+outputDir)

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
        output.WriteLine("output fsyacc:"+outputDir)

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

            //"typedSequentialExpr"
            
            //"anonMatchingExpr"
            //"anonLambdaExpr"
            //"withClauses"
            //"parenPattern"
            //"headBindingPattern"
            //"activePatternCaseNames"
            //"atomicExpr"
            //"unionCaseRepr"
            //"classDefnBlock"
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
        output.WriteLine("output fsyacc:"+outputDir)








