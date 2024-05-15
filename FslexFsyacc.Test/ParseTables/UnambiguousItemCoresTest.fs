namespace FslexFsyacc.Runtime.ParseTables

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

type unambiguousItemCoresTest (output: ITestOutputHelper) =

    [<Fact>]
    member _.``BNF4_55 legacy``() =
        let mainProductions:string list list = BNF4_55.mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int> = Map []

        let crew = 
            FslexFsyacc.Yacc.ActionParseTableCrewUtils.getActionParseTableCrew
                mainProductions
                dummyTokens
                precedences

        output.WriteLine("let unambiguousItemCores = " + stringify crew.unambiguousItemCores)

    [<Fact>]
    member _.``BNF4_55 ParseTableRow``() =
        let mainProductions:string list list = BNF4_55.mainProductions
        let productions = ProductionUtils.augment mainProductions

        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int> = Map []

        let bnf = BNF.from mainProductions

        let tbl = ParseTableRow.from(productions, dummyTokens, precedences)
        Should.equal tbl.unambiguousItemCores BNF4_55.unambiguousItemCores

    [<Fact>]
    member _.``BNF4_3 legacy``() =
        let mainProductions:string list list = BNF4_3.mainProductions
        let productions = ProductionUtils.augment mainProductions

        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int> = BNF4_3.precedences

        let crew = 
            FslexFsyacc.Yacc.ActionParseTableCrewUtils.getActionParseTableCrew
                mainProductions
                dummyTokens
                precedences

        let rows =
            crew.unambiguousItemCores
            |> Seq.map(fun(KeyValue(i,mp))->
                $"    kernel_{i},{stringify mp}"
            )
            |> String.concat "\n"
        output.WriteLine($"let unambiguousItemCores = [\n{rows}\n]")

    [<Fact>]
    member _.``BNF4_3 ParseTableRow``() =
        let mainProductions:string list list = BNF4_3.mainProductions
        let productions = ProductionUtils.augment mainProductions

        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int> = BNF4_3.precedences

        let bnf = BNF.from mainProductions

        let tbl = ParseTableRow.from(productions, dummyTokens, precedences)
        Should.equal tbl.unambiguousItemCores BNF4_3.unambiguousItemCores
        ()


    [<Fact>]
    member _.``BNF4_67 legacy``() =
        let mainProductions:string list list = BNF4_67.mainProductions
        let productions = ProductionUtils.augment mainProductions

        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int> = BNF4_67.precedences

        let crew = 
            FslexFsyacc.Yacc.ActionParseTableCrewUtils.getActionParseTableCrew
                mainProductions
                dummyTokens
                precedences

        let rows =
            crew.unambiguousItemCores
            |> Seq.map(fun(KeyValue(i,mp))->
                $"    kernel_{i},{stringify mp}"
            )
            |> String.concat "\n"
        output.WriteLine($"let unambiguousItemCores = [\n{rows}\n]")

    [<Fact>]
    member _.``BNF4_67 ParseTableRow``() =
        let mainProductions:string list list = BNF4_67.mainProductions
        let productions = ProductionUtils.augment mainProductions

        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int> = Map []

        let bnf = BNF.from mainProductions

        let tbl = ParseTableRow.from(productions, dummyTokens, precedences)
        Should.equal tbl.unambiguousItemCores BNF4_67.unambiguousItemCores
        ()



