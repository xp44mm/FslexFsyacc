﻿namespace FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FslexFsyacc.ItemCores
open FslexFsyacc
open FSharp.Idioms
open FSharp.Idioms.Literal

type Example454Test(output: ITestOutputHelper) =

    // grammar 4.55
    let S = "S"
    let C = "C"
    let c = "c"
    let d = "d"

    let mainProductions = [
        [ S; C; C ]
        [ C; c; C ]
        [ C; d ]
    ]

    //let grammar = 
    //    mainProductions
    //    |> ProductionsCrewUtils.getProductionsCrew
    //    |> GrammarCrewUtils.getNullableCrew
    //    |> GrammarCrewUtils.getFirstLastCrew
    //    |> GrammarCrewUtils.getFollowPrecedeCrew
    //    |> GrammarCrewUtils.getItemCoresCrew

    //[<Fact>]
    //member _.``kernels``() =
    //    let kernels = 
    //        grammar
    //        |> GrammarCrewUtils.getKernelCollection
            
    //    for i, kernel in Seq.indexed kernels do
    //    let kernel =
    //        kernel 
    //        |> Set.toList
    //        |> List.map(fun (ic,la) -> ic.production, ic.dot, Set.toList la)

    //    output.WriteLine($"let kernel_{i} = {stringify kernel}")

    //[<Fact>]
    //member _.``closures``() =
    //    let closures = 
    //        grammar
    //        |> GrammarCrewUtils.getClosureCollection
            
    //    for i,(knl,cls) in Seq.indexed closures do
    //    let cls =
    //        cls 
    //        |> Set.map(fun (i,la) -> i.production, i.dot, Set.toList la)
    //        |> Set.toList
    //    output.WriteLine($"let closure_{i} = {stringify cls}")

    //[<Fact>]
    //member _.``spread closures``() =
    //    let crew =
    //        grammar
    //        |> LALRCollectionCrewUtils.getLALRCollectionCrew

    //    for KeyValue(i, cls) in crew.closures do
    //    let cls =
    //        cls 
    //        |> Set.toList
    //        |> List.map(fun (la,ic) -> la, ic.production, ic.dot )
        
    //    output.WriteLine($"let spreadClosure_{i} = {stringify cls}")

    //[<Fact>]
    //member _.``conflicts``() =
    //    let crew =
    //        grammar
    //        |> LALRCollectionCrewUtils.getLALRCollectionCrew

    //    let conflicts =
    //        crew.closures
    //        |> Map.map(fun i closure ->
    //            closure
    //            |> Seq.groupBy(fun (la,_) -> la)
    //            |> Seq.map(fun(la,pairs)->
    //                let kernel =
    //                    pairs
    //                    |> Seq.map snd
    //                    |> Set.ofSeq
    //                la, kernel
    //            )
    //            |> Map.ofSeq
    //        )

    //    for KeyValue(src, mp) in conflicts do
    //    for KeyValue(sym, tgt) in mp do
    //    let tgt =
    //        tgt
    //        |> List.ofSeq
    //        |> List.map(fun i -> i.production,i.dot)
    //    output.WriteLine($"{stringify (src,sym,tgt)}")


