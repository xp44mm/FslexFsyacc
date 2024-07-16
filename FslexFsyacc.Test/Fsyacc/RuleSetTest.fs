namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open FSharp.Idioms.Literal

open FSharp.xUnit
open FslexFsyacc.Precedences
open FslexFsyacc.Fsyacc

type RuleSetTest(output:ITestOutputHelper) =

    [<Fact>]
    member _.``delete symbols test``() =
        let symbols = set ["x";"y";"z"]
        let getRule production =
            {
            production = production
            dummy = ""
            reducer = ""
            }

        let rules = Set [
            getRule ["x";"y";"z"]
            getRule ["x";"x";"a"]
        ]

        let y =
            RuleSet.deleteSymbols symbols rules
        let e = set [{production=["x"];dummy="";reducer=""};{production=["x";"a"];dummy="";reducer=""}]
        output.WriteLine(stringify y)
        Should.equal e y

    [<Fact>]
    member _.``headOrLastTerminal test``() =
        let rules = set [
            Rule.just(["a"],"x","")
            Rule.just(["m";"-";"m"],"y","")
            Rule.just(["e";"e";"+";"e"],"z","")
            Rule.just(["e";"e";"-";"e"],"t","")
            ]

        let productions = 
            rules
            |> Set.map(fun rule -> rule.production)
        let symbols =
            productions
            |> Set.map Set.ofList
            |> Set.unionMany

        let nonterminals = 
            productions
            |> Set.map List.head
    
        let terminals = 
            Set.difference symbols nonterminals

        let data = Precedence.DummyData.just(productions,terminals,Map[])

        //let tryGetLastTerminal = Precedence.lastTerminal terminals

        ////首先要冲突，不然
        rules
        |> Seq.map(fun rule ->
            let p = rule.production
            p, data.tryGetDummy p            
        )
        |> Seq.iter(fun c -> output.WriteLine (stringify c))

