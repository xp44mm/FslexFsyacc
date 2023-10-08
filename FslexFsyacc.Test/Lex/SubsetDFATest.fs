namespace FslexFsyacc.Lex

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type SubsetDFATest(output:ITestOutputHelper) =
    let show res = 
        res 
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``from ntran 52 to subset DFA fig 54``() =
        //fig 3-52
        let ntran = set [
            (0u,None,1u);
            (0u,None,3u);
            (0u,None,7u);
            (1u,Some 'a',2u);
            (3u,Some 'a',4u);
            (4u,Some 'b',5u);
            (5u,Some 'b',6u);
            (7u,Some 'a',7u);
            (7u,Some 'b',8u);
            (8u,Some 'b',8u)
            ]

        let nfinals = [
            2u
            6u
            8u
            ]

        let dfa = SubsetDFAUtils.create ntran

        let dfinals = dfa|>SubsetDFAUtils.getAcceptingStates nfinals
            
        //show dfa.allStates
        //show dfinals

        let ys = set [
            set [0u;1u;3u;7u];
            set [2u;4u;7u];
            set [5u;8u];
            set [6u;8u];
            set [7u];
            set [8u]
            ]
        let yf = [
            set [set [2u;4u;7u]];
            set [set [6u;8u]];
            set [set [5u;8u];
            set [8u]]
            ]
        Should.equal ys dfa.allStates
        Should.equal yf dfinals

    [<Fact>]
    member _.``from ntran to subset DFA``() =
        ///figure 3.34
        let ntran = set [
            (0u, None, 1u);
            (0u, None, 7u);
            (1u, None, 2u);
            (1u, None, 4u);
            (2u, Some 'a', 3u);
            (4u, Some 'b', 5u);
            (3u, None, 6u);
            (5u, None, 6u);
            (6u, None, 1u);
            (6u, None, 7u);
            (7u, Some 'a', 8u);
            (8u, Some 'b', 9u);
            (9u, Some 'b', 10u)
        ]
        let y = SubsetDFAUtils.create ntran
        //show y
        let dfa = {
            dtran=set [
                set [0u;1u;2u;4u;7u],'a',set [1u;2u;3u;4u;6u;7u;8u];
                set [0u;1u;2u;4u;7u],'b',set [1u;2u;4u;5u;6u;7u];
                set [1u;2u;3u;4u;6u;7u;8u],'a',set [1u;2u;3u;4u;6u;7u;8u];
                set [1u;2u;3u;4u;6u;7u;8u],'b',set [1u;2u;4u;5u;6u;7u;9u];
                set [1u;2u;4u;5u;6u;7u],'a',set [1u;2u;3u;4u;6u;7u;8u];
                set [1u;2u;4u;5u;6u;7u],'b',set [1u;2u;4u;5u;6u;7u];
                set [1u;2u;4u;5u;6u;7u;9u],'a',set [1u;2u;3u;4u;6u;7u;8u];
                set [1u;2u;4u;5u;6u;7u;9u],'b',set [1u;2u;4u;5u;6u;7u;10u];
                set [1u;2u;4u;5u;6u;7u;10u],'a',set [1u;2u;3u;4u;6u;7u;8u];
                set [1u;2u;4u;5u;6u;7u;10u],'b',set [1u;2u;4u;5u;6u;7u]];
            allStates=set [
                set [0u;1u;2u;4u;7u];
                set [1u;2u;3u;4u;6u;7u;8u];
                set [1u;2u;4u;5u;6u;7u];
                set [1u;2u;4u;5u;6u;7u;9u];
                set [1u;2u;4u;5u;6u;7u;10u]];
            //alphabet=set ['a';'b']
            }

        Should.equal y dfa

    [<Fact>]
    member _.``get dfinals``() = 
        let dfa = {
            dtran=set [];
            allStates=set [
                set [0u;1u;];
                set [1u;2u;3u;4u;];
                set [1u;2u;4u;];
                ];
            }
        let nfinals = [3u;4u]
        let dfinals = dfa|>SubsetDFAUtils.getAcceptingStates nfinals
        //show dfinals
        let y = [
            set [set [1u;2u;3u;4u]];
            set [set [1u;2u;4u]]
            ]
        Should.equal y dfinals
