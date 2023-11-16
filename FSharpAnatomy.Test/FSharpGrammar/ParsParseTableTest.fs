namespace FSharpAnatomy.FSharpGrammar

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms.Literal
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

    let sourcePath = Path.Combine(__SOURCE_DIRECTORY__, "fsyacc")
    let filePath = Path.Combine(sourcePath, "pars.fsyacc")

    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let fsyaccCrew =
        text
        |> RawFsyaccFileCrewUtils.parse
        |> FlatedFsyaccFileCrewUtils.fromRawFsyaccFileCrew

    let tblCrew =
        fsyaccCrew
        |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

    //let removeErrorRules =
    let robust = set [
        "error";
        "recover";
        "coming_soon";"COMING_SOON";
        "_IS_HERE"
        ]
    //ProductionUtils.isWithoutError robust

    [<Fact(
    Skip="no for verify"
    )>]
    member _.``000 - unused symbol in decls test``() =
        let x = 
            fsyaccCrew.flatedPrecedences
            |> Map.keys
            |> Set.ofSeq
        let y = 
            fsyaccCrew.flatedRules
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

    [<Fact(
    Skip="no for verify"
    )>]
    member _.``001 - identOrOp test``() =
        let s0 = "identOrOp"

        let flatedFsyacc =
            fsyaccCrew
            |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

        let flat = 
            flatedFsyacc
            |> FlatFsyaccFileUtils.start s0

        let flat =
            {
                flat with
                    rules = 
                        flat.rules
                        |> List.filter(fun (prod,_,_) -> 
                            prod 
                            |> ProductionUtils.without robust)
                        |> FlatRulesUtils.eliminateChomsky
                        |> List.map (fun (prod,_,_) ->prod,"","")
            }

        let txt =
            flat
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        let outputDir = Path.Combine(sourcePath, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n" + outputDir)

    [<Fact(
    Skip="no for verify"
    )>]
    member _.``002 - path test``() =
        let s0 = "path"

        let flatedFsyacc =
            fsyaccCrew
            |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

        let flat = 
            flatedFsyacc 
            |> FlatFsyaccFileUtils.start s0

        //let augmentRules = 
        //    flat.augmentRules
        //    |> Map.filter(fun prod _ -> 
        //        prod 
        //        |> ProductionUtils.isWithoutError robust)
        //    |> RuleSetUtils.eliminateChomsky
        //    |> Map.map (fun prod (nm,ac) ->"","")
        let rules = 
            flat.rules
            |> List.filter(fun (prod,_,_) -> 
                prod 
                |> ProductionUtils.without robust)
            |> FlatRulesUtils.eliminateChomsky
            |> List.map (fun (prod,_,_) ->prod,"","")

        let flat =
            {
                flat with
                    rules = rules
            }

        let txt =
            flat
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        let outputDir = Path.Combine(sourcePath, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n" + outputDir)

    [<Fact(
    Skip="no for verify"
    )>]
    member _.``003 - rawConstant test``() =
        let s0 = "rawConstant"

        let flatedFsyacc =
            fsyaccCrew
            |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

        let flat = 
            flatedFsyacc 
            |> FlatFsyaccFileUtils.start s0

        //let augmentRules = 
        //    flat.augmentRules
        //    |> Map.filter(fun prod _ -> 
        //        prod 
        //        |> ProductionUtils.isWithoutError robust)
        //    |> RuleSetUtils.eliminateChomsky
        //    |> Map.map (fun prod (nm,ac) ->"","")
        let rules = 
            flat.rules
            |> List.filter(fun (prod,_,_) -> 
                prod 
                |> ProductionUtils.without robust)
            |> FlatRulesUtils.eliminateChomsky
            |> List.map (fun (prod,_,_) ->prod,"","")

        let flat =
            {
                flat with
                    rules = rules
            }

        let txt =
            flat
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        let outputDir = Path.Combine(sourcePath, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n" + outputDir)

    [<Fact(
    Skip="no for verify"
    )>]
    member _.``004 - tyconDefn test``() =
        let s0 = "tyconDefn"
        let terminals = set [
            "attributes";"opt_attributes"
            "access";"opt_access"
            "typar";

        ]

        let flatedFsyacc =
            fsyaccCrew
            |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

        let flat = 
            {
                flatedFsyacc with
                    rules = 
                        flatedFsyacc.rules 
                        |> FlatRulesUtils.removeHeads terminals
            }
            |> FlatFsyaccFileUtils.start s0

        //let augmentRules = 
        //    flat.augmentRules
        //    |> Map.filter(fun prod _ -> 
        //        prod 
        //        |> ProductionUtils.isWithoutError robust)
        //    |> RuleSetUtils.eliminateChomsky
        //    |> Map.map (fun prod (nm,ac) ->"","")

        let rules = 
            flat.rules
            |> List.filter(fun (prod,_,_) -> 
                prod 
                |> ProductionUtils.without robust)
            |> FlatRulesUtils.eliminateChomsky
            |> List.map (fun (prod,_,_) ->prod,"","")

        let flat =
            {
                flat with
                    rules = rules
            }

        let txt =
            flat
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        let outputDir = Path.Combine(sourcePath, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n"+outputDir)

    [<Fact(
    Skip="no for verify"
    )>]
    member _.``005 - postfixTyparDecls test``() =
        let s0 = "postfixTyparDecls"
        let terminals = set [
            "attributes";"opt_attributes"
            "typeConstraints"
            ]

        let flatedFsyacc =
            fsyaccCrew
            |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

        let flat = 
            {
                flatedFsyacc with
                    rules = 
                    flatedFsyacc.rules
                    |> FlatRulesUtils.removeHeads terminals
            }
            |> FlatFsyaccFileUtils.start s0

        //let augmentRules = 
        //    flat.augmentRules
        //    |> Map.filter(fun prod _ -> 
        //        prod 
        //        |> ProductionUtils.isWithoutError robust)
        //    |> RuleSetUtils.eliminateChomsky
        //    |> Map.map (fun prod (nm,ac) ->"","")

        //let flat =
        //    {
        //        flat with
        //            augmentRules = augmentRules
        //    }
        let rules = 
            flat.rules
            |> List.filter(fun (prod,_,_) -> 
                prod 
                |> ProductionUtils.without robust)
            |> FlatRulesUtils.eliminateChomsky
            |> List.map (fun (prod,_,_) ->prod,"","")

        let flat =
            {
                flat with
                    rules = rules
            }

        let txt =
            flat
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        let outputDir = Path.Combine(sourcePath, $"{s0}.fsyacc")
        File.WriteAllText(outputDir,txt,Encoding.UTF8)
        output.WriteLine("output:\r\n"+outputDir)











