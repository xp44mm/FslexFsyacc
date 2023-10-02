namespace FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit
open FSharp.Idioms

type Example427Test(output:ITestOutputHelper) =
    let show res = 
        res 
        |> Literal.stringify
        |> output.WriteLine

    let E = "E"
    let E' = "E'"
    let T = "T"
    let T' = "T'"
    let F = "F"
    let id = "id"

    ///表达式语法(4.28)
    let mainProductions = [
        [ E   ; T   ; E' ]
        [ E'  ; "+" ; T   ; E' ]
        [ E' ]
        [ T   ; F   ; T' ]
        [ T'  ; "*" ; F   ; T' ]
        [ T' ]
        [ F   ; "(" ; E   ; ")" ]
        [ F   ; id ]
        ]

    [<Fact>]
    member _.``0101 - augment grammar productions``() =
        //show grammar.productions
        let y = set [
            ["";"E"];
            ["E";"T";"E'"];
            ["E'"];
            ["E'";"+";"T";"E'"];
            ["F";"(";"E";")"];
            ["F";"id"];
            ["T";"F";"T'"];
            ["T'"];
            ["T'";"*";"F";"T'"]]
        let grammar = 
            mainProductions
            |> GrammarCrewUtils.getProductionsCrew

        Should.equal y grammar.augmentedProductions 

    [<Fact>]
    member _.``0201 - symbols``() =
        //show grammar.symbols

        let y = set ["(";")";"*";"+";"E";"E'";"F";"T";"T'";"id"]
        let grammar = 
            mainProductions
            |> GrammarCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew

        Should.equal y grammar.symbols

    [<Fact>]
    member _.``0202 nonterminals``() =
        //show grammar.nonterminals

        let y = set ["E";"E'";"F";"T";"T'"]
        let grammar = 
            mainProductions
            |> GrammarCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew

        Should.equal y grammar.nonterminals

    [<Fact>]
    member _.``0203 nullables``() =
        //show grammar.nullables
        let y = set ["E'";"T'"]
        let grammar = 
            mainProductions
            |> GrammarCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew

        Should.equal y grammar.nullables

    [<Fact>]
    member _.``0301 firsts``() =
        let grammar = 
            mainProductions
            |> GrammarCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew
            |> GrammarCrewUtils.getFirstLastCrew

        show grammar.firsts
        let y = Map.ofList [
            "E",set ["(";"id"];
            "E'",set ["+"];
            "F",set ["(";"id"];
            "T",set ["(";"id"];
            "T'",set ["*"];
            ]

        Should.equal y grammar.firsts

    [<Fact>]
    member _.``0302 lasts``() =
        let grammar = 
            mainProductions
            |> GrammarCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew
            |> GrammarCrewUtils.getFirstLastCrew

        //show grammar.lasts
        let y = Map.ofList [
            "E",set [")";"id"];
            "E'",set [")";"id"];
            "F",set [")";"id"];
            "T",set [")";"id"];
            "T'",set [")";"id"];
            ]

        Should.equal y grammar.lasts

    [<Fact>]
    member _.``0303 follows``() =
        let grammar = 
            mainProductions
            |> GrammarCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew
            |> GrammarCrewUtils.getFirstLastCrew
            |> GrammarCrewUtils.getFollowPrecedeCrew

        //show grammar.follows

        //空字符串代表书中的$
        let z = Map [
            "(",set ["(";"id"];")",set ["";")";"*";"+"];
            "*",set ["(";"id"];"+",set ["(";"id"];
            "E",set ["";")"];"E'",set ["";")"];
            "F",set ["";")";"*";"+"];"T",set ["";")";"+"];
            "T'",set ["";")";"+"];"id",set ["";")";"*";"+"]]
        Should.equal z grammar.follows

    [<Fact>]
    member _.``0304 precedes``() =
        let grammar = 
            mainProductions
            |> GrammarCrewUtils.getProductionsCrew
            |> GrammarCrewUtils.getNullableCrew
            |> GrammarCrewUtils.getFirstLastCrew
            |> GrammarCrewUtils.getFollowPrecedeCrew

        show grammar.precedes
        ////空字符串代表BOF
        let y = Map [
            "(",set ["";"(";"*";"+"];")",set [")";"id"];
            "*",set [")";"id"];"+",set [")";"id"];
            "E",set ["";"("];"E'",set [")";"id"];
            "F",set ["";"(";"*";"+"];"T",set ["";"(";"+"];
            "T'",set [")";"id"];"id",set ["";"(";"*";"+"]]
        Should.equal y grammar.precedes

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
        //show grammar.closures

        let y = set [
            set [["";"E"],0],set [(["";"E"],0),set [""];(["E";"T";"E'"],0),set [""];(["F";"(";"E";")"],0),set ["";"*";"+"];(["F";"id"],0),set ["";"*";"+"];(["T";"F";"T'"],0),set ["";"+"]];
            set [["";"E"],1],set [(["";"E"],1),set [""]];
            set [["E";"T";"E'"],1],set [(["E";"T";"E'"],1),set ["";")"];(["E'"],0),set ["";")"];(["E'";"+";"T";"E'"],0),set ["";")"]];
            set [["E";"T";"E'"],2],set [(["E";"T";"E'"],2),set ["";")"]];
            set [["E'";"+";"T";"E'"],1],set [(["E'";"+";"T";"E'"],1),set ["";")"];(["F";"(";"E";")"],0),set ["";")";"*";"+"];(["F";"id"],0),set ["";")";"*";"+"];(["T";"F";"T'"],0),set ["";")";"+"]];
            set [["E'";"+";"T";"E'"],2],set [(["E'"],0),set ["";")"];(["E'";"+";"T";"E'"],0),set ["";")"];(["E'";"+";"T";"E'"],2),set ["";")"]];
            set [["E'";"+";"T";"E'"],3],set [(["E'";"+";"T";"E'"],3),set ["";")"]];
            set [["F";"(";"E";")"],1],set [(["E";"T";"E'"],0),set [")"];(["F";"(";"E";")"],0),set [")";"*";"+"];(["F";"(";"E";")"],1),set ["";")";"*";"+"];(["F";"id"],0),set [")";"*";"+"];(["T";"F";"T'"],0),set [")";"+"]];
            set [["F";"(";"E";")"],2],set [(["F";"(";"E";")"],2),set ["";")";"*";"+"]];
            set [["F";"(";"E";")"],3],set [(["F";"(";"E";")"],3),set ["";")";"*";"+"]];
            set [["F";"id"],1],set [(["F";"id"],1),set ["";")";"*";"+"]];
            set [["T";"F";"T'"],1],set [(["T";"F";"T'"],1),set ["";")";"+"];(["T'"],0),set ["";")";"+"];(["T'";"*";"F";"T'"],0),set ["";")";"+"]];
            set [["T";"F";"T'"],2],set [(["T";"F";"T'"],2),set ["";")";"+"]];
            set [["T'";"*";"F";"T'"],1],set [(["F";"(";"E";")"],0),set ["";")";"*";"+"];(["F";"id"],0),set ["";")";"*";"+"];(["T'";"*";"F";"T'"],1),set ["";")";"+"]];
            set [["T'";"*";"F";"T'"],2],set [(["T'"],0),set ["";")";"+"];(["T'";"*";"F";"T'"],0),set ["";")";"+"];(["T'";"*";"F";"T'"],2),set ["";")";"+"]];
            set [["T'";"*";"F";"T'"],3],set [(["T'";"*";"F";"T'"],3),set ["";")";"+"]]]

        Should.equal y closures
    
    [<Fact>]
    member _.``goto factory``() =
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

        ()

        //let gotos = 
        //    GotoFactory.make closures
        //    |> Set.map(fun(k1,s,k2)-> k1 |> Set.map(fun i -> i.production,i.dot),s,k2 |> Set.map(fun i -> i.production,i.dot))

        ////show y
        //let y = set [
        //    set [["";"E"],0],"(",set [["F";"(";"E";")"],1];
        //    set [["";"E"],0],"E",set [["";"E"],1];
        //    set [["";"E"],0],"F",set [["T";"F";"T'"],1];
        //    set [["";"E"],0],"T",set [["E";"T";"E'"],1];
        //    set [["";"E"],0],"id",set [["F";"id"],1];
        //    set [["E";"T";"E'"],1],"+",set [["E'";"+";"T";"E'"],1];
        //    set [["E";"T";"E'"],1],"E'",set [["E";"T";"E'"],2];
        //    set [["E'";"+";"T";"E'"],1],"(",set [["F";"(";"E";")"],1];
        //    set [["E'";"+";"T";"E'"],1],"F",set [["T";"F";"T'"],1];
        //    set [["E'";"+";"T";"E'"],1],"T",set [["E'";"+";"T";"E'"],2];
        //    set [["E'";"+";"T";"E'"],1],"id",set [["F";"id"],1];
        //    set [["E'";"+";"T";"E'"],2],"+",set [["E'";"+";"T";"E'"],1];
        //    set [["E'";"+";"T";"E'"],2],"E'",set [["E'";"+";"T";"E'"],3];
        //    set [["F";"(";"E";")"],1],"(",set [["F";"(";"E";")"],1];
        //    set [["F";"(";"E";")"],1],"E",set [["F";"(";"E";")"],2];
        //    set [["F";"(";"E";")"],1],"F",set [["T";"F";"T'"],1];
        //    set [["F";"(";"E";")"],1],"T",set [["E";"T";"E'"],1];
        //    set [["F";"(";"E";")"],1],"id",set [["F";"id"],1];
        //    set [["F";"(";"E";")"],2],")",set [["F";"(";"E";")"],3];
        //    set [["T";"F";"T'"],1],"*",set [["T'";"*";"F";"T'"],1];
        //    set [["T";"F";"T'"],1],"T'",set [["T";"F";"T'"],2];
        //    set [["T'";"*";"F";"T'"],1],"(",set [["F";"(";"E";")"],1];
        //    set [["T'";"*";"F";"T'"],1],"F",set [["T'";"*";"F";"T'"],2];
        //    set [["T'";"*";"F";"T'"],1],"id",set [["F";"id"],1];
        //    set [["T'";"*";"F";"T'"],2],"*",set [["T'";"*";"F";"T'"],1];
        //    set [["T'";"*";"F";"T'"],2],"T'",set [["T'";"*";"F";"T'"],3]]

        //Should.equal y gotos
