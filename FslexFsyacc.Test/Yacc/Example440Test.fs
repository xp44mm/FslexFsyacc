namespace FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit
open FSharp.Idioms

type Example440Test(output:ITestOutputHelper) =
    let show res = 
        res 
        |> Literal.stringify
        |> output.WriteLine

    ///表达式文法(4.1)
    let E = "E"
    let T = "T"
    let F = "F"
    let id = "id"

    let mainProductions = [
        [ E; E; "+"; T ]
        [ E; T ]
        [ T; T; "*"; F ]
        [ T; F ]
        [ F; "("; E; ")" ]
        [ F; id ]
    ]

    let grammar = 
        mainProductions
        |> GrammarCrewUtils.getProductionsCrew
        |> GrammarCrewUtils.getNullableCrew
        |> GrammarCrewUtils.getFirstLastCrew

    [<Fact>]
    member _.``all of item cores``() =
        let itemCores = 
            ItemCoreUtils.make grammar.augmentedProductions
            |> Set.map(fun i -> i.production,i.dot)
        //show itemCores
        let y = set [
            ["";"E"],0;
            ["";"E"],1;
            ["E";"E";"+";"T"],0;
            ["E";"E";"+";"T"],1;
            ["E";"E";"+";"T"],2;
            ["E";"E";"+";"T"],3;
            ["E";"T"],0;
            ["E";"T"],1;
            ["F";"(";"E";")"],0;
            ["F";"(";"E";")"],1;
            ["F";"(";"E";")"],2;["F";"(";"E";")"],3;
            ["F";"id"],0;["F";"id"],1;
            ["T";"F"],0;["T";"F"],1;
            ["T";"T";"*";"F"],0;["T";"T";"*";"F"],1;["T";"T";"*";"F"],2;["T";"T";"*";"F"],3]
        Should.equal y itemCores

    //[<Fact>]
    //member _.``item core attributes``() =
    //    let itemCores = 
    //        ItemCoreFactory.make grammar.augmentedProductions

    //    let itemCoreAttributes = 
    //        ItemCoreAttributeFactory.make grammar.nonterminals grammar.nullables grammar.firsts itemCores
    //        |> Map.mapEntry(fun i v -> (i.production,i.dot), v )
    //        |> Map.map(fun _ sq -> Seq.exactlyOne sq)
   
    //    //show itemCoreAttributes
    //    let y = Map.ofList [
    //        (["";"E"],0),(true,Set.empty);
    //        (["E";"E";"+";"T"],0),(false,set ["+"]);
    //        (["E";"E";"+";"T"],2),(true,Set.empty);
    //        (["E";"T"],0),(true,Set.empty);
    //        (["F";"(";"E";")"],1),(false,set [")"]);
    //        (["T";"F"],0),(true,Set.empty);
    //        (["T";"T";"*";"F"],0),(false,set ["*"]);
    //        (["T";"T";"*";"F"],2),(true,Set.empty)]
    //    Should.equal y itemCoreAttributes

    [<Fact>]
    member _.``closures``() =
        let grammar = 
            mainProductions
            |> GrammarCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew
            |> GrammarCrewUtils.getFirstLastCrew
            |> GrammarCrewUtils.getFollowPrecedeCrew
            |> GrammarCrewUtils.getItemCoresCrew

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
            set [["";"E"],0],set [(["";"E"],0),set [""];(["E";"E";"+";"T"],0),set ["";"+"];(["E";"T"],0),set ["";"+"];(["F";"(";"E";")"],0),set ["";"*";"+"];(["F";"id"],0),set ["";"*";"+"];(["T";"F"],0),set ["";"*";"+"];(["T";"T";"*";"F"],0),set ["";"*";"+"]];
            set [["";"E"],1;["E";"E";"+";"T"],1],set [(["";"E"],1),set [""];(["E";"E";"+";"T"],1),set ["";"+"]];
            set [["E";"E";"+";"T"],1;["F";"(";"E";")"],2],set [(["E";"E";"+";"T"],1),set [")";"+"];(["F";"(";"E";")"],2),set ["";")";"*";"+"]];
            set [["E";"E";"+";"T"],2],set [(["E";"E";"+";"T"],2),set ["";")";"+"];(["F";"(";"E";")"],0),set ["";")";"*";"+"];(["F";"id"],0),set ["";")";"*";"+"];(["T";"F"],0),set ["";")";"*";"+"];(["T";"T";"*";"F"],0),set ["";")";"*";"+"]];
            set [["E";"E";"+";"T"],3;["T";"T";"*";"F"],1],set [(["E";"E";"+";"T"],3),set ["";")";"+"];(["T";"T";"*";"F"],1),set ["";")";"*";"+"]];
            set [["E";"T"],1;["T";"T";"*";"F"],1],set [(["E";"T"],1),set ["";")";"+"];(["T";"T";"*";"F"],1),set ["";")";"*";"+"]];
            set [["F";"(";"E";")"],1],set [(["E";"E";"+";"T"],0),set [")";"+"];(["E";"T"],0),set [")";"+"];(["F";"(";"E";")"],0),set [")";"*";"+"];(["F";"(";"E";")"],1),set ["";")";"*";"+"];(["F";"id"],0),set [")";"*";"+"];(["T";"F"],0),set [")";"*";"+"];(["T";"T";"*";"F"],0),set [")";"*";"+"]];
            set [["F";"(";"E";")"],3],set [(["F";"(";"E";")"],3),set ["";")";"*";"+"]];
            set [["F";"id"],1],set [(["F";"id"],1),set ["";")";"*";"+"]];
            set [["T";"F"],1],set [(["T";"F"],1),set ["";")";"*";"+"]];
            set [["T";"T";"*";"F"],2],set [(["F";"(";"E";")"],0),set ["";")";"*";"+"];(["F";"id"],0),set ["";")";"*";"+"];(["T";"T";"*";"F"],2),set ["";")";"*";"+"]];
            set [["T";"T";"*";"F"],3],set [(["T";"T";"*";"F"],3),set ["";")";"*";"+"]]]
        Should.equal y closures

    [<Fact>]
    member _.``goto factory``() =
        let itemCores = 
            ItemCoreUtils.make grammar.augmentedProductions

        //let itemCoreAttributes = 
        //    ItemCoreAttributeFactory.make grammar.nonterminals grammar.nullables grammar.firsts itemCores
   
        //let closures = 
        //    CollectionFactory.make itemCores itemCoreAttributes grammar.augmentedProductions

        //let y = 
        //    GotoFactory.make closures
        //    |> Set.map(fun(k1,s,k2)-> k1 |> Set.map(fun i -> i.production,i.dot),s,k2 |> Set.map(fun i -> i.production,i.dot))

        //show y
        //Should.equal y gotos
        ()
