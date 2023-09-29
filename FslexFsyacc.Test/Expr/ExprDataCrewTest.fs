namespace FslexFsyacc.Expr

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text

open FSharp.xUnit
open FSharp.Literals.Literal
open FSharp.Idioms

open FslexFsyacc.Runtime
open FslexFsyacc.Yacc
open FslexFsyacc.Fsyacc

type ExprDataCrewTest (output:ITestOutputHelper) =
    [<Fact>]
    member _.``01 - getAmbiguousCollectionCrew Test``() =
        let input = ExprData.inputProductionList

        let crew = 
            input
            |> GrammarCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew
            |> GrammarCrewUtils.getFirstLastCrew
            |> GrammarCrewUtils.getFollowPrecedeCrew
            |> GrammarCrewUtils.getItemCoresCrew
            |> LALRCollectionCrewUtils.getLALRCollectionCrew
            |> AmbiguousCollectionCrewUtils.getAmbiguousCollectionCrew
        Should.equal crew.mainProductions ExprData.mainProductions
        Should.equal crew.augmentedProductions ExprData.augmentedProductions

        Should.equal crew.symbols ExprData.symbols
        Should.equal crew.nonterminals ExprData.nonterminals
        Should.equal crew.terminals ExprData.terminals
        Should.equal crew.nullables ExprData.nullables


        Should.equal crew.firsts ExprData.firsts
        Should.equal crew.lasts ExprData.lasts


        Should.equal crew.follows ExprData.follows
        Should.equal crew.precedes ExprData.precedes


        let items = crew.itemCoreCrews

        let itemCores = items |> Map.keys |> Set.ofSeq

        Should.equal itemCores ExprData.itemCores

        Should.equal crew.kernels ExprData.kernels
        let closures =
            crew.closures
            |> Map.values
            |> Seq.toList

        Should.equal closures ExprData.closures

        let gotos =
            crew.GOTOs
            |> Map.values
            |> Seq.toList
        Should.equal gotos ExprData.gotos

        let conflicts =
            crew.conflicts
            |> Map.values
            |> Seq.toList
        Should.equal conflicts ExprData.conflicts

    [<Fact>]
    member _.``02 - getActionParseTableCrew test``() =
        let input = ExprData.inputProductionList
        let dummyTokens = ExprData.productionNames
        let precedences = ExprData.precedences

        let crew = 
            (input,dummyTokens,precedences)
            |||> ActionParseTableCrewUtils.getActionParseTableCrew

        let actions =
            crew.actions
            |> Map.values
            |> Seq.toList

        let resolvedClosures = 
            crew.resolvedClosures
            |> Map.values
            |> Seq.toList

        Should.equal actions ExprData.actions
        Should.equal resolvedClosures ExprData.resolvedClosures



