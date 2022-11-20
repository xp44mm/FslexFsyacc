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

/* NOTE: no "recover" */

*)


(*


optLiteralValueSpfn -> opt_LiteralValueSpfn

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

(*

topTupleType
topTupleTypeElements

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
    member _.``000 - unused symbol in decls test``() =
        let x = 
            fsyacc.precedences
            |> Map.keys

        let y = 
            fsyacc.rules
            |> List.collect(fun(p,t,_)->t::p)
            |> Set.ofList

        let e = set ["ASR";"COMMENT";"CONSTRAINT";"CONSTRUCTOR";"DOT_DOT_HAT";"GREATER_BAR_RBRACK";"HASH_ELSE";"HASH_ENDIF";"HASH_IDENT";"HASH_IF";"HASH_LIGHT";"HASH_LINE";"INACTIVECODE";"INSTANCE";"INT32_DOT_DOT";"INTERP_STRING_BEGIN";"LEX_FAILURE";"LINE_COMMENT";"ODUMMY";"ORESET";"QMARK_QMARK";"RESERVED";"RQUOTE_DOT";"STRING_TEXT";"WHITESPACE";"arg_expr_adjacent_minus";"decl_do";"decl_match";"expr_do";"expr_not";"head_expr_adjacent_minus";"matching_bar";"prec_defn_sep";"prec_interaction_empty";"prec_semiexpr_sep"]
        show (x-y)
            
        let outp =
            (x-y)
            |> Seq.map(fun s -> 
                [
                    "on"
                    s
                    ""
                    " C  W "
                    ""
                ] |> String.concat "\t" )
            |> String.concat "\r\n"

        output.WriteLine(outp)

    [<Fact>]
    member _.``001 - identOrOp test``() =
        let s0 = "identOrOp"

        //分解到关键字表达式（含）
        let terminals = set []

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
    member _.``002 - path test``() =
        let s0 = "path"

        //分解到关键字表达式（含）
        let terminals = set []

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
    member _.``003 - pathOp test``() =
        let s0 = "pathOp"

        //分解到关键字表达式（含）
        let terminals = set ["opName"]

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
    member _.``004 - rawConstant test``() =
        let s0 = "rawConstant"

        //分解到关键字表达式（含）
        let terminals = set []

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
    member _.``005 - topTypeWithTypeConstraints test``() =
        let s0 = "topTypeWithTypeConstraints"
        let terminals = set [
            "attributes"
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
    member _.``006 - classMemberSpfn test``() =
        let s0 = "classMemberSpfn"
        let terminals = set [
            "attributes"
            "identOrOp"
            "typeConstraints"
            "topType"
            "declExpr"
            "appType"
            "typ"
            "path"
            "rawConstant"
            "measureTypeArg"

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


    //[<Fact>]
    //member _.``001 - implementationFile test``() =
    //    let s0 = "implementationFile"

    //    //分解到关键字表达式（含）
    //    let terminals = set [
    //        "attributes"
    //        "hashDirective"
    //        "cPrototype"
    //        "localBindings"
    //        "sequentialExpr"
    //        "typeWithTypeConstraints"
    //        "withPatternClauses"
    //        "atomicPatterns"
    //        "withClauses"
    //        "ifExprCases"
    //        "parenPattern"
    //        "headBindingPattern"
    //        "typ" // :? typ
    //        "appType" // open type
    //        "atomTypeNonAtomicDeprecated"
    //        "atomicExprAfterType"
    //        "identOrOp"
    //        "atomicExpr"
    //        "tupleExpr"
    //        "tyconDefn"
    //        "unionCaseRepr"
    //        "classDefnBlock"
    //    ]

    //    let flat = fsyacc.start(s0,terminals)

    //    let flat =
    //        {
    //            flat with
    //                rules = 
    //                    flat.rules
    //                    |> removeErrorRules
    //                    |> FsyaccFileRules.eliminateChomsky
    //                    |> List.map (fun(prod,nm,ac)->prod,"","")
    //        }

    //    let txt = flat.toRaw().render()

    //    let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
    //    File.WriteAllText(outputDir,txt,Encoding.UTF8)
    //    output.WriteLine("output:\r\n" + outputDir)

    //[<Fact>]
    //member _.``003 - typeConstraints test``() =
    //    let s0 = "typeConstraints"
    //    let terminals = set [
    //        "attributes"
    //        "typ"
    //        "typar"
    //        "appType"
    //        "classMemberSpfn"
    //        "typeArgsNoHpaDeprecated"
    //        ]

    //    let flat = fsyacc.start(s0, terminals)

    //    let flat =
    //        {
    //            flat with
    //                rules = 
    //                    flat.rules
    //                    |> removeErrorRules
    //                    |> FsyaccFileRules.eliminateChomsky
    //                    |> List.map (fun(prod,nm,ac)->prod,"","")
    //        }

    //    let txt = flat.toRaw().render()

    //    let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
    //    File.WriteAllText(outputDir,txt,Encoding.UTF8)
    //    output.WriteLine("output:\r\n"+outputDir)

    //[<Fact>]
    //member _.``003 - interaction test``() =
    //    let s0 = "interaction"
    //    let terminals = set [
    //        "attributes"
    //        "moduleDefn"
    //        "hashDirective"
    //        "declExpr"
    //    ]

    //    let flat = fsyacc.start(s0,terminals)

    //    let flat =
    //        {
    //            flat with
    //                rules = 
    //                    flat.rules
    //                    |> removeErrorRules
    //                    |> FsyaccFileRules.eliminateChomsky
    //                    |> List.map (fun(prod,nm,ac)->prod,"","")
    //        }

    //    let txt = flat.toRaw().render()

    //    let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
    //    File.WriteAllText(outputDir,txt,Encoding.UTF8)
    //    output.WriteLine("output:\r\n"+outputDir)

    //[<Fact>]
    //member _.``004 - typedSequentialExprEOF test``() =
    //    let s0 = "typedSequentialExprEOF"
    //    let terminals = set [
    //        "declExpr"
    //        "hardwhiteLetBindings"
    //        "typeWithTypeConstraints"
    //    ]

    //    let flat = fsyacc.start(s0, terminals)

    //    let flat =
    //        {
    //            flat with
    //                rules = 
    //                    flat.rules
    //                    |> removeErrorRules
    //                    |> FsyaccFileRules.eliminateChomsky
    //                    |> List.map (fun(prod,nm,ac)->prod,"","")
    //        }

    //    let txt = flat.toRaw().render()

    //    let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
    //    File.WriteAllText(outputDir,txt,Encoding.UTF8)
    //    output.WriteLine("output:\r\n"+outputDir)

    //[<Fact>]
    //member _.``005 - hashDirective test``() =
    //    let s0 = "hashDirective"
    //    let terminals = set [
    //        ]

    //    let flat = fsyacc.start(s0,terminals)

    //    let flat =
    //        {
    //            flat with
    //                rules = 
    //                    flat.rules
    //                    |> removeErrorRules
    //                    |> FsyaccFileRules.eliminateChomsky
    //                    |> List.map (fun(prod,nm,ac)->prod,"","")
    //        }

    //    let txt = flat.toRaw().render()

    //    let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
    //    File.WriteAllText(outputDir,txt,Encoding.UTF8)
    //    output.WriteLine("output:\r\n" + outputDir)

    //[<Fact>]
    //member _.``006 - signatureFile test``() =
    //    let s0 = "signatureFile"
    //    let terminals = set [
    //        "attributes"
    //        "hashDirective"
    //        "activePatternCaseNames"
    //        "typar"
    //        "typeConstraint"
    //        "topType"
    //        "declExpr"
    //        "typ"
    //        "unionTypeRepr"
    //        "braceFieldDeclList"
    //        "tyconClassSpfn"
    //        "classMemberSpfn"
    //        "unionCaseRepr"
    //        "appType"
    //        //"moduleSpfn"
    //        //"hashDirective"
    //    ]

    //    let flat = fsyacc.start(s0,terminals)

    //    let flat =
    //        {
    //            flat with
    //                rules = 
    //                    flat.rules
    //                    |> removeErrorRules
    //                    |> FsyaccFileRules.eliminateChomsky
    //                    |> List.map (fun(prod,nm,ac)->prod,"","")
    //        }

    //    let txt = flat.toRaw().render()

    //    let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{s0}.fsyacc")
    //    File.WriteAllText(outputDir,txt,Encoding.UTF8)
    //    output.WriteLine("output:\r\n"+outputDir)







