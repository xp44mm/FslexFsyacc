namespace FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type Example454Test(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

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

    let grammar = Grammar.from mainProductions

    [<Fact>]
    member this.``closures``() =
        let itemCores = 
            ItemCoreFactory.make grammar.productions

        let itemCoreAttributes = 
            ItemCoreAttributeFactory.make grammar.nonterminals grammar.nullables grammar.firsts itemCores
   
        let closures = 
            CollectionFactory.make itemCores itemCoreAttributes grammar.productions
            |> Set.map(fun (kernel,closure)->
                let k = kernel |> Set.map(fun i -> i.production,i.dot)
                let c = closure |> Set.map(fun (i,la)->(i.production,i.dot),la)
                k,c
            )

        //show closures
        let y = set [
            set [
                ["";"S"],0],set [
                    (["";"S"],0),set [""];
                    (["C";"c";"C"],0),set ["c";"d"];
                    (["C";"d"],0),set ["c";"d"];(["S";"C";"C"],0),set [""]];
            set [["";"S"],1],set [(["";"S"],1),set [""]];
            set [["C";"c";"C"],1],set [(["C";"c";"C"],0),set ["";"c";"d"];(["C";"c";"C"],1),set ["";"c";"d"];(["C";"d"],0),set ["";"c";"d"]];
            set [["C";"c";"C"],2],set [(["C";"c";"C"],2),set ["";"c";"d"]];
            set [["C";"d"],1],set [(["C";"d"],1),set ["";"c";"d"]];
            set [["S";"C";"C"],1],set [(["C";"c";"C"],0),set [""];(["C";"d"],0),set [""];(["S";"C";"C"],1),set [""]];
            set [["S";"C";"C"],2],set [(["S";"C";"C"],2),set [""]]]
        Should.equal y closures

    [<Fact>]
    member this.``goto factory``() =
        let itemCores = 
            ItemCoreFactory.make grammar.productions

        let itemCoreAttributes = 
            ItemCoreAttributeFactory.make grammar.nonterminals grammar.nullables grammar.firsts itemCores
   
        let closures = 
            CollectionFactory.make itemCores itemCoreAttributes grammar.productions

        let gotos = 
            GotoFactory.make closures
            |> Set.map(fun(k1,s,k2)-> k1 |> Set.map(fun i -> i.production,i.dot),s,k2 |> Set.map(fun i -> i.production,i.dot))

        //show gotos
        let y = set [
            set [["";"S"],0],"C",set [["S";"C";"C"],1];
            set [["";"S"],0],"S",set [["";"S"],1];
            set [["";"S"],0],"c",set [["C";"c";"C"],1];
            set [["";"S"],0],"d",set [["C";"d"],1];
            set [["C";"c";"C"],1],"C",set [["C";"c";"C"],2];
            set [["C";"c";"C"],1],"c",set [["C";"c";"C"],1];
            set [["C";"c";"C"],1],"d",set [["C";"d"],1];
            set [["S";"C";"C"],1],"C",set [["S";"C";"C"],2];
            set [["S";"C";"C"],1],"c",set [["C";"c";"C"],1];
            set [["S";"C";"C"],1],"d",set [["C";"d"],1]]
        Should.equal y gotos

