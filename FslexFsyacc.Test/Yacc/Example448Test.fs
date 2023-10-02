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

    let grammar = 
        mainProductions
        |> GrammarCrewUtils.getProductionsCrew
        |> GrammarCrewUtils.getNullableCrew
        |> GrammarCrewUtils.getFirstLastCrew

    [<Fact>]
    member _.``closures``() =
        let grammar = 
            mainProductions
            |> GrammarCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew
            |> GrammarCrewUtils.getFirstLastCrew
            |> GrammarCrewUtils.getFollowPrecedeCrew
            |> GrammarCrewUtils.getItemCoresCrew

        //let itemCores = 
        //    ItemCoreFactory.make grammar.augmentedProductions

        //let itemCoreAttributes = 
        //    ItemCoreAttributeFactory.make grammar.nonterminals grammar.nullables grammar.firsts itemCores
            

        let closures = 
            //CollectionFactory.make itemCores itemCoreAttributes grammar.augmentedProductions
            grammar
            |> GrammarCrewUtils.getClosureCollection 
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
            ItemCoreUtils.make grammar.augmentedProductions

        //let itemCoreAttributes = 
        //    ItemCoreAttributeFactory.make grammar.nonterminals grammar.nullables grammar.firsts itemCores
   
        //let closures = 
        //    CollectionFactory.make itemCores itemCoreAttributes grammar.augmentedProductions

        //let gotos = 
        //    GotoFactory.make closures
        //    |> Set.map(fun(k1,s,k2)-> k1 |> Set.map(fun i -> i.production,i.dot),s,k2 |> Set.map(fun i -> i.production,i.dot))


        //Should.equal y gotos
        ()
