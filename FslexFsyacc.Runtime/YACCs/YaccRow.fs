﻿namespace FslexFsyacc.Runtime.YACCs

open FslexFsyacc.Runtime.Precedences
open FslexFsyacc.Runtime.BNFs
open FslexFsyacc.Runtime.ItemCores

open FSharp.Idioms
open FSharp.Idioms.Literal

type YaccRow = 
    {
    bnf: BNF
    dummyTokens:Map<string list,string>
    precedences:Map<string,int*Associativity>

    actions: Map<Set<ItemCore>,Map<string,ParseTableAction>>
    encodeActions: list<list<string*int>>

    //unambiguousItemCores: Map<Set<ItemCore>,Map<string,Set<ItemCore>>>
    resolvedClosures: Map<Set<ItemCore>,Map<ItemCore,Set<string>>>
    encodeClosures: list<list<int*int*string list>>
    }

    static member from (
        productions:Set<string list>, 
        dummyTokens:Map<string list,string>, 
        precedences:Map<string,int * Associativity >
        ) =
        let bnf = BNF.just productions
        let tryGetDummy = Precedence.tryGetDummy dummyTokens bnf.grammar.terminals

        let tryGetPrecedence = Precedence.tryGetPrecedence tryGetDummy precedences

        //let unambiguousItemCores =
        //    bnf.conflictedItemCores
        //    |> Map.map(fun k mp ->
        //        mp
        //        |> Map.map(fun s ics -> 
        //            SLR.just(ics).disambiguate(tryGetPrecedence))
        //    )

        let actions =
            bnf.actions
            |> Map.map(fun src mp ->
                mp
                |> Seq.choose(fun(KeyValue(sym, acts)) ->
                    acts
                    |> Conflict.disambiguate tryGetPrecedence
                    |> Option.map (Pair.prepend sym)
                )
                |> Map.ofSeq
            )
            |> Map.filter( fun src mp -> not mp.IsEmpty )

        let resolvedClosures =
            actions
            |> Map.map(fun kernel mp ->
                let x =
                    mp
                    |> Seq.collect(fun(KeyValue(sym, act))->
                        act.toItemCores(sym)
                    )
                    |> Seq.groupBy fst
                    |> Seq.map(fun (ic,sq) -> 
                        let symbols =
                            sq
                            |> Seq.choose snd
                            |> Set.ofSeq
                        ic, symbols)
                    |> Map.ofSeq
                x
            )

        let encoder =
            {
                productions =
                    ParseTableEncoder.getProductions bnf.grammar.productions
                kernels = bnf.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq
            } : ParseTableEncoder
        let encodeActions = encoder.encodeActions actions

        let encodeClosures = encoder.encodeClosures resolvedClosures

        {
        bnf         = bnf
        dummyTokens = dummyTokens
        precedences = precedences
        actions     = actions             
        encodeActions = encodeActions

        //unambiguousItemCores = unambiguousItemCores
        resolvedClosures = resolvedClosures
        encodeClosures = encodeClosures
        }
