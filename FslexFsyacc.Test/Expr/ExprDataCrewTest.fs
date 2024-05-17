namespace FslexFsyacc.Expr

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text

open FSharp.xUnit
open FSharp.Idioms.Literal
open FSharp.Idioms

open FslexFsyacc.Runtime
open FslexFsyacc.Yacc
open FslexFsyacc.Fsyacc

type ExprDataCrewTest (output:ITestOutputHelper) =
    do ()
    //[<Fact>]
    //member _.``01 - getAmbiguousCollectionCrew Test``() =
    //    let input = ExprData.inputProductionList

    //    let collectionCrew = 
    //        input
    //        |> ProductionsCrewUtils.getProductionsCrew
    //        |> GrammarCrewUtils.getNullableCrew
    //        |> GrammarCrewUtils.getFirstLastCrew
    //        |> GrammarCrewUtils.getFollowPrecedeCrew
    //        |> GrammarCrewUtils.getItemCoresCrew
    //        |> LALRCollectionCrewUtils.getLALRCollectionCrew
    //        |> AmbiguousCollectionCrewUtils.getAmbiguousCollectionCrew

    //    Should.equal collectionCrew.mainProductions ExprData.mainProductions
    //    Should.equal collectionCrew.augmentedProductions ExprData.augmentedProductions

    //    Should.equal collectionCrew.symbols ExprData.symbols
    //    Should.equal collectionCrew.nonterminals ExprData.nonterminals
    //    Should.equal collectionCrew.terminals ExprData.terminals
    //    Should.equal collectionCrew.nullables ExprData.nullables


    //    Should.equal collectionCrew.firsts ExprData.firsts
    //    Should.equal collectionCrew.lasts ExprData.lasts


    //    Should.equal collectionCrew.follows ExprData.follows
    //    Should.equal collectionCrew.precedes ExprData.precedes

    //    let itemCores = collectionCrew.itemCoreCrews |> Map.keys |> Set.ofSeq

    //    Should.equal itemCores ExprData.itemCores

    //    Should.equal collectionCrew.kernels ExprData.kernels
    //    let closures =
    //        collectionCrew.closures
    //        |> Map.values
    //        |> Seq.toList

    //    Should.equal closures ExprData.closures

    //    let gotos =
    //        collectionCrew.GOTOs
    //        |> Map.values
    //        |> Seq.toList
    //    Should.equal gotos ExprData.gotos

    //    let conflicts =
    //        collectionCrew.conflictedItemCores
    //        |> Map.values
    //        |> Seq.toList
    //    Should.equal conflicts ExprData.conflicts

    //[<Fact>]
    //member _.``02 - getActionParseTableCrew test``() =
    //    let input = ExprData.inputProductionList
    //    let dummyTokens = ExprData.productionNames
    //    let precedences = ExprData.precedences

    //    let crew = 
    //        (input,dummyTokens,precedences)
    //        |||> ActionParseTableCrewUtils.getActionParseTableCrew

    //    let actions =
    //        crew.actions
    //        |> Map.values
    //        |> Seq.toList

    //    let resolvedClosures = 
    //        crew.resolvedClosures
    //        |> Map.values
    //        |> Seq.toList

    //    Should.equal actions ExprData.actions
    //    output.WriteLine($"let resolvedClosures = {stringify resolvedClosures}")
    //    Should.equal resolvedClosures ExprData.resolvedClosures

    //[<Fact>]
    //member _.``03 - getEncodedParseTableCrew Test``() =
    //    let input = ExprData.inputProductionList
    //    let dummyTokens = ExprData.productionNames
    //    let precedences = ExprData.precedences

    //    let crew = 
    //        (input,dummyTokens,precedences)
    //        |> EncodedParseTableCrewUtils.getEncodedParseTableCrew

    //    //output.WriteLine($"let encodedActions = {stringify crew.encodedActions}")
    //    Should.equal crew.encodedActions ExprData.encodedActions

    //    //output.WriteLine($"let encodedClosures = {stringify crew.encodedClosures}")
    //    Should.equal crew.encodedClosures ExprData.encodedClosures



