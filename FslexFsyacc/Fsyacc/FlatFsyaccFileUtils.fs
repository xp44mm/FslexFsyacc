module FslexFsyacc.Fsyacc.FlatFsyaccFileUtils

open System
open System.Text
open System.Text.RegularExpressions
open System.IO

open FSharp.Idioms
open FSharp.Idioms.Literal

/////根据startSymbol提取相关规则，无用规则被无视忽略。
//let start (startSymbol:string) (fsyacc:FlatFsyaccFile) =
//    let rules =
//        fsyacc.rules
//        |> FlatRulesUtils.extractRules startSymbol

//    let symbols =
//        rules
//        |> List.collect Triple.first
//        |> Set.ofList

//    let precedences =
//        fsyacc.precedences
//        |> Map.filter(fun symbol level ->
//            symbols.Contains symbol
//        )

//    let declarations =
//        fsyacc.declarations
//        |> Map.filter(fun sym _ -> symbols.Contains sym)

//    {
//        fsyacc with
//            rules = rules
//            precedences = precedences
//            declarations = declarations
//    }

