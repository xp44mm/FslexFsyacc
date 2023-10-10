namespace FslexFsyacc

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime
open FslexFsyacc.Dir

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.xUnit
open FSharp.Literals
open FSharp.Literals.Literal

type BNF428CrewTest(output:ITestOutputHelper) =
    [<Fact>]
    member _.``01 - getProductionsCrew test``() =
        let input = BNF428Data.inputProductionList
        let crew = ProductionsCrewUtils.getProductionsCrew input
        Should.equal crew.mainProductions BNF428Data.mainProductions
        Should.equal crew.augmentedProductions BNF428Data.augmentedProductions

    [<Fact>]
    member _.``02 - getNullableCrew test``() =
        let input = BNF428Data.inputProductionList
        let crew = 
            input
            |> ProductionsCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew

        Should.equal crew.symbols BNF428Data.symbols
        Should.equal crew.nonterminals BNF428Data.nonterminals
        Should.equal crew.terminals BNF428Data.terminals
        Should.equal crew.nullables BNF428Data.nullables

    [<Fact>]
    member _.``03 - getFirstLastCrew test``() =
        let input = BNF428Data.inputProductionList
        let crew = 
            input
            |> ProductionsCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew
            |> GrammarCrewUtils.getFirstLastCrew

        Should.equal crew.firsts BNF428Data.firsts
        Should.equal crew.lasts BNF428Data.lasts

    [<Fact>]
    member _.``04 - getFollowPrecedeCrew test``() =
        let input = BNF428Data.inputProductionList
        let crew = 
            input
            |> ProductionsCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew
            |> GrammarCrewUtils.getFirstLastCrew
            |> GrammarCrewUtils.getFollowPrecedeCrew

        Should.equal crew.follows BNF428Data.follows
        Should.equal crew.precedes BNF428Data.precedes

    [<Fact>]
    member _.``05 - getItemCoresCrew test``() =
        let input = BNF428Data.inputProductionList
        let crew = 
            input
            |> ProductionsCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew
            |> GrammarCrewUtils.getFirstLastCrew
            |> GrammarCrewUtils.getFollowPrecedeCrew
            |> GrammarCrewUtils.getItemCoresCrew

        let items = crew.itemCoreCrews

        let itemCores = items |> Map.keys |> Set.ofSeq

        Should.equal itemCores BNF428Data.itemCores

    [<Fact>]
    member _.``06 - getLALRCollectionCrew test``() =
        let input = BNF428Data.inputProductionList
        let crew = 
            input
            |> ProductionsCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew
            |> GrammarCrewUtils.getFirstLastCrew
            |> GrammarCrewUtils.getFollowPrecedeCrew
            |> GrammarCrewUtils.getItemCoresCrew
            |> LALRCollectionCrewUtils.getLALRCollectionCrew

        Should.equal crew.kernels BNF428Data.kernels
        let closures =
            crew.closures
            |> Map.values
            |> Seq.toList

        Should.equal closures BNF428Data.closures

        let gotos =
            crew.GOTOs
            |> Map.values
            |> Seq.toList
        Should.equal gotos BNF428Data.gotos

    [<Fact>]
    member _.``06 - getAmbiguousCollectionCrew test``() =
        let input = BNF428Data.inputProductionList
        let crew = 
            input
            |> ProductionsCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew
            |> GrammarCrewUtils.getFirstLastCrew
            |> GrammarCrewUtils.getFollowPrecedeCrew
            |> GrammarCrewUtils.getItemCoresCrew
            |> LALRCollectionCrewUtils.getLALRCollectionCrew
            |> AmbiguousCollectionCrewUtils.getAmbiguousCollectionCrew

        let conflicts =
            crew.conflictedItemCores
            |> Map.values
            |> Seq.toList
        Should.equal conflicts BNF428Data.conflicts

    [<Fact>]
    member _.``07 - getActionParseTableCrew test``() =
        let input = BNF428Data.inputProductionList
        let crew = 
            input
            |> ActionParseTableCrewUtils.getActionParseTableCrew <| (Map []) <| (Map [])

        let actions =
            crew.actions
            |> Map.values
            |> Seq.toList

        let resolvedClosures = 
            crew.resolvedClosures
            |> Map.values
            |> Seq.toList

        Should.equal actions BNF428Data.actions
        Should.equal resolvedClosures BNF428Data.resolvedClosures

    [<Fact>]
    member _.``07 - getEncodedParseTableCrew Test``() =
        let input = BNF428Data.inputProductionList
        let crew =             
            EncodedParseTableCrewUtils.getEncodedParseTableCrew(input,Map [],Map [])

        let encodedActions =
            crew.encodedActions

        let encodedClosures = 
            crew.encodedClosures

        Should.equal encodedActions BNF428Data.encodedActions
        Should.equal encodedClosures BNF428Data.encodedClosures



