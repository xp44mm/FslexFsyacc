namespace FslexFsyacc.YACCs

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FslexFsyacc.BNFs
open FslexFsyacc
open FslexFsyacc.Precedences

type ResolvedClosuresTest (output: ITestOutputHelper) =

    //[<Fact>]
    //member _.``BNF4_55 legacy``() =
    //    let mainProductions:string list list = BNF4_55.mainProductions
    //    let dummyTokens:Map<string list,string> = Map []
    //    let precedences:Map<string,int> = Map []

    //    let crew = 
    //        FslexFsyacc.Yacc.ActionParseTableCrewUtils.getActionParseTableCrew
    //            mainProductions
    //            dummyTokens
    //            precedences

    //    output.WriteLine("let resolvedClosures = \n    [")
    //    for KeyValue(i,mp) in crew.resolvedClosures do
    //        output.WriteLine($"    kernel_{i},{stringify mp}")
    //    output.WriteLine("    ]")

    [<Fact>]
    member _.``BNF4_55 ParseTableRow``() =
        let mainProductions:string list list = BNF4_55.mainProductions
        let productions = ProductionUtils.augment mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int*Associativity> = Map []
        let tbl = YaccRow.from(productions, dummyTokens, precedences)
        Should.equal tbl.resolvedClosures BNF4_55.resolvedClosures

    //[<Fact>]
    //member _.``BNF4_3 legacy``() =
    //    let mainProductions:string list list = BNF4_3.mainProductions
    //    let dummyTokens:Map<string list,string> = Map []
    //    let precedences:Map<string,int> = BNF4_3.precedences

    //    let crew = 
    //        FslexFsyacc.Yacc.ActionParseTableCrewUtils.getActionParseTableCrew
    //            mainProductions
    //            dummyTokens
    //            precedences

    //    output.WriteLine("let resolvedClosures = \n    [")
    //    for KeyValue(i,mp) in crew.resolvedClosures do
    //        output.WriteLine($"    kernel_{i},{stringify mp}")
    //    output.WriteLine("    ]")

    [<Fact>]
    member _.``BNF4_3 ParseTableRow``() =
        let mainProductions:string list list = BNF4_3.mainProductions
        let productions = ProductionUtils.augment mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int*Associativity> = BNF4_3.precedences
        let tbl = YaccRow.from(productions, dummyTokens, precedences)
        Should.equal tbl.resolvedClosures BNF4_3.resolvedClosures

    //[<Fact>]
    //member _.``BNF4_67 legacy``() =
    //    let mainProductions:string list list = BNF4_67.mainProductions
    //    let dummyTokens:Map<string list,string> = Map []
    //    let precedences:Map<string,int> = BNF4_67.precedences

    //    let crew = 
    //        FslexFsyacc.Yacc.ActionParseTableCrewUtils.getActionParseTableCrew
    //            mainProductions
    //            dummyTokens
    //            precedences

    //    output.WriteLine("let resolvedClosures = \n    [")
    //    for KeyValue(i,mp) in crew.resolvedClosures do
    //        output.WriteLine($"    kernel_{i},{stringify mp}")
    //    output.WriteLine("    ]")

    [<Fact>]
    member _.``BNF4_67 ParseTableRow``() =
        let mainProductions:string list list = BNF4_67.mainProductions
        let productions = ProductionUtils.augment mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int*Associativity> = Map []
        let tbl = YaccRow.from(productions, dummyTokens, precedences)
        Should.equal tbl.resolvedClosures BNF4_67.resolvedClosures




