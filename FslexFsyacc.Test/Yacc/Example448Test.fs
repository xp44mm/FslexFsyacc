namespace FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type Example448Test(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    // grammar 4.49
    let S = "S"
    let L = "L"
    let R = "R"
    let id = "id"

    let mainProductions = [
        [ S; L; "="; R ]
        [ S; R ]
        [ L; "*"; R ]
        [ L; id ]
        [ R; L ]
    ]

    let grammar = Grammar.from mainProductions

    [<Fact>]
    member _.``closures``() =
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
            set [["";"S"],0],set [(["";"S"],0),set [""];(["L";"*";"R"],0),set ["";"="];(["L";"id"],0),set ["";"="];(["R";"L"],0),set [""];(["S";"L";"=";"R"],0),set [""];(["S";"R"],0),set [""]];
            set [["";"S"],1],set [(["";"S"],1),set [""]];
            set [["L";"*";"R"],1],set [(["L";"*";"R"],0),set ["";"="];(["L";"*";"R"],1),set ["";"="];(["L";"id"],0),set ["";"="];(["R";"L"],0),set ["";"="]];
            set [["L";"*";"R"],2],set [(["L";"*";"R"],2),set ["";"="]];
            set [["L";"id"],1],set [(["L";"id"],1),set ["";"="]];
            set [["R";"L"],1],set [(["R";"L"],1),set ["";"="]];
            set [["R";"L"],1;["S";"L";"=";"R"],1],set [(["R";"L"],1),set [""];(["S";"L";"=";"R"],1),set [""]];
            set [["S";"L";"=";"R"],2],set [(["L";"*";"R"],0),set [""];(["L";"id"],0),set [""];(["R";"L"],0),set [""];(["S";"L";"=";"R"],2),set [""]];
            set [["S";"L";"=";"R"],3],set [(["S";"L";"=";"R"],3),set [""]];
            set [["S";"R"],1],set [(["S";"R"],1),set [""]]]
        Should.equal y closures

    [<Fact>]
    member _.``goto factory``() =
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
            set [["";"S"],0],"*",set [["L";"*";"R"],1];
            set [["";"S"],0],"L",set [["R";"L"],1;["S";"L";"=";"R"],1];
            set [["";"S"],0],"R",set [["S";"R"],1];
            set [["";"S"],0],"S",set [["";"S"],1];
            set [["";"S"],0],"id",set [["L";"id"],1];
            set [["L";"*";"R"],1],"*",set [["L";"*";"R"],1];
            set [["L";"*";"R"],1],"L",set [["R";"L"],1];
            set [["L";"*";"R"],1],"R",set [["L";"*";"R"],2];
            set [["L";"*";"R"],1],"id",set [["L";"id"],1];
            set [["R";"L"],1;["S";"L";"=";"R"],1],"=",set [["S";"L";"=";"R"],2];
            set [["S";"L";"=";"R"],2],"*",set [["L";"*";"R"],1];
            set [["S";"L";"=";"R"],2],"L",set [["R";"L"],1];
            set [["S";"L";"=";"R"],2],"R",set [["S";"L";"=";"R"],3];
            set [["S";"L";"=";"R"],2],"id",set [["L";"id"],1]]

        Should.equal y gotos

