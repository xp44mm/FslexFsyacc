namespace FslexFsyacc.Runtime.ParseTables

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

type EncodeClosuresTest (output: ITestOutputHelper) =

    [<Fact>]
    member _.``BNF4_55 legacy``() =
        let mainProductions:string list list = BNF4_55.mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int> = Map []

        let crew = 
            FslexFsyacc.Yacc.EncodedParseTableCrewUtils.getEncodedParseTableCrew (
                mainProductions,
                dummyTokens,
                precedences
                )

        output.WriteLine("let encodeClosures = \n    [")
        for ls in crew.encodedClosures do
            output.WriteLine($"    {stringify ls}")
        output.WriteLine("    ]")

    [<Fact>]
    member _.``BNF4_55 ParseTableRow``() =
        let mainProductions:string list list = BNF4_55.mainProductions
        let productions = ProductionUtils.augment mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int> = Map []
        let tbl = ParseTableRow.from(productions, dummyTokens, precedences)
        Should.equal tbl.encodeClosures BNF4_55.encodeClosures

    [<Fact>]
    member _.``BNF4_3 legacy``() =
        let mainProductions:string list list = BNF4_3.mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int> = BNF4_3.precedences

        let crew = 
            FslexFsyacc.Yacc.EncodedParseTableCrewUtils.getEncodedParseTableCrew (
                mainProductions,
                dummyTokens,
                precedences
                )

        output.WriteLine("let encodeClosures = \n    [")
        for ls in crew.encodedClosures do
            output.WriteLine($"    {stringify ls}")
        output.WriteLine("    ]")

    [<Fact>]
    member _.``BNF4_3 ParseTableRow``() =
        let mainProductions:string list list = BNF4_3.mainProductions
        let productions = ProductionUtils.augment mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int> = BNF4_3.precedences
        let tbl = ParseTableRow.from(productions, dummyTokens, precedences)
        Should.equal tbl.encodeClosures BNF4_3.encodeClosures

    [<Fact>]
    member _.``BNF4_67 legacy``() =
        let mainProductions:string list list = BNF4_67.mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int> = BNF4_67.precedences

        let crew = 
            FslexFsyacc.Yacc.EncodedParseTableCrewUtils.getEncodedParseTableCrew (
                mainProductions,
                dummyTokens,
                precedences
                )

        output.WriteLine("let encodeClosures = \n    [")
        for ls in crew.encodedClosures do
            output.WriteLine($"    {stringify ls}")
        output.WriteLine("    ]")

    [<Fact>]
    member _.``BNF4_67 ParseTableRow``() =
        let mainProductions:string list list = BNF4_67.mainProductions
        let productions = ProductionUtils.augment mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int> = Map []
        let tbl = ParseTableRow.from(productions, dummyTokens, precedences)
        Should.equal tbl.encodeClosures BNF4_67.encodeClosures




