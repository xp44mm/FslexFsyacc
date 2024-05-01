namespace FslexFsyacc.Runtime.Lex

open Xunit
open Xunit.Abstractions
open FSharp.Idioms
open FSharp.xUnit

type DFATest(output:ITestOutputHelper) =
    let show res = 
        res 
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``dfinals``() =
        ///dfa的状态表，每个dfa状态，是一个nfa的状态集，
        let allStates = set [
            set [0u;1u;3u;7u];
            set [2u;4u;7u];
            set [5u;8u];
            set [6u;8u];
            set [7u];
            set [8u]
            ]

        let dfa = {
            dtran = Set.empty
            allStates = allStates
            //alphabet = Set.empty
        }
        //nfa的接受状态
        let nfinals = [2u;6u;8u]

        let y = dfa|>SubsetDFAUtils.getAcceptingStates nfinals

        //show y
        //列表顺序与nfinals相对应，每个nfa接受状态，可能是1个或几个dfa状态。
        let dfinals = [
            set [set [2u;4u;7u]];
            set [set [6u;8u]];
            set [set [5u;8u];set [8u]]
            ]

        Should.equal y dfinals


