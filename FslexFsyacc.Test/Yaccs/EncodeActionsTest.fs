﻿namespace FslexFsyacc.YACCs

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

type EncodeActionsTest (output: ITestOutputHelper) =

    [<Fact>]
    member _.``BNF4_55``() =
        //let bnf = BNF.from BNF4_55.mainProductions
        let productions = ProductionUtils.augment BNF4_55.mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int*Associativity> = Map []

        let tbl = YaccRow.from(productions, dummyTokens, precedences)
        //output.WriteLine("let encodeActions = " + stringify tbl.encodeActions)
        Should.equal BNF4_55.encodeActions tbl.encodeActions

    [<Fact>]
    member _.``BNF4_3``() =
        let bnf = BNF.from BNF4_3.mainProductions
        let productions = ProductionUtils.augment BNF4_3.mainProductions

        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int*Associativity> = BNF4_3.precedences

        let tbl = YaccRow.from(productions, dummyTokens, precedences)
        output.WriteLine("let encodeActions = " + stringify tbl.encodeActions)
        Should.equal BNF4_3.encodeActions tbl.encodeActions

    [<Fact>]
    member _.``BNF4_67``() =
        let bnf = BNF.from BNF4_67.mainProductions
        let productions = ProductionUtils.augment BNF4_67.mainProductions

        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int*Associativity> = Map []

        let tbl = YaccRow.from(productions, dummyTokens, precedences)
        //output.WriteLine("let encodeActions = " + stringify tbl.encodeActions)
        Should.equal BNF4_67.encodeActions tbl.encodeActions



