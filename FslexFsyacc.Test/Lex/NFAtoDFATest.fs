namespace FslexFsyacc.Lex

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type NFAtoDFATest(output:ITestOutputHelper) =
    let show res = 
        res 
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``get nfinals``() =
        // fig 3-34
        let ntran = set [
            (0u,None,1u);
            (0u,None,7u);
            (1u,None,2u);
            (1u,None,4u);
            (2u,Some 'a',3u);
            (3u,None,6u);
            (4u,Some 'b',5u);
            (5u,None,6u);
            (6u,None,1u);
            (6u,None,7u);
            (7u,Some 'a',8u);
            (8u,Some 'b',9u);
            (9u,Some 'b',10u)
            ]

        let nfinalLexemes = [
            10u, None
            ]

        let dfa = SubsetDFA<_,_>.create(ntran)
        let nfinals = 
            nfinalLexemes
            |> List.map fst
            |> Array.ofList

        Should.equal dfa {dtran=set [set [0u;1u;2u;4u;7u],'a',set [1u;2u;3u;4u;6u;7u;8u];set [0u;1u;2u;4u;7u],'b',set [1u;2u;4u;5u;6u;7u];set [1u;2u;3u;4u;6u;7u;8u],'a',set [1u;2u;3u;4u;6u;7u;8u];set [1u;2u;3u;4u;6u;7u;8u],'b',set [1u;2u;4u;5u;6u;7u;9u];set [1u;2u;4u;5u;6u;7u],'a',set [1u;2u;3u;4u;6u;7u;8u];set [1u;2u;4u;5u;6u;7u],'b',set [1u;2u;4u;5u;6u;7u];set [1u;2u;4u;5u;6u;7u;9u],'a',set [1u;2u;3u;4u;6u;7u;8u];set [1u;2u;4u;5u;6u;7u;9u],'b',set [1u;2u;4u;5u;6u;7u;10u];set [1u;2u;4u;5u;6u;7u;10u],'a',set [1u;2u;3u;4u;6u;7u;8u];set [1u;2u;4u;5u;6u;7u;10u],'b',set [1u;2u;4u;5u;6u;7u]];allStates=set [set [0u;1u;2u;4u;7u];set [1u;2u;3u;4u;6u;7u;8u];set [1u;2u;4u;5u;6u;7u];set [1u;2u;4u;5u;6u;7u;9u];set [1u;2u;4u;5u;6u;7u;10u]]}

        Should.equal nfinals [|10u|]

    [<Fact>]
    member _.``Example 3-21: nfa to dfa``() =
        // fig 3-34
        let ntran = set [
            (0u,None,1u);
            (0u,None,7u);
            (1u,None,2u);
            (1u,None,4u);
            (2u,Some 'a',3u);
            (3u,None,6u);
            (4u,Some 'b',5u);
            (5u,None,6u);
            (6u,None,1u);
            (6u,None,7u);
            (7u,Some 'a',8u);
            (8u,Some 'b',9u);
            (9u,Some 'b',10u)
            ]

        let nfinalLexemes = [
            10u, 10u
            ]

        let dfa = DFA.fromNFA(ntran, nfinalLexemes)

        //fig 3-36
        let dtran = set [
            (0u,'a',1u);
            (0u,'b',0u);
            (1u,'a',1u);
            (1u,'b',2u);
            (2u,'a',1u);
            (2u,'b',3u);
            (3u,'a',1u);
            (3u,'b',0u)
            ]

        let finalLexemes = [|
            set [3u], Set.empty
            |]

        Should.equal dfa.nextStates (DFATools.getNextStates dtran)
        Should.equal dfa.finalLexemes finalLexemes


    [<Fact>]
    member _.``Example 3-26: r3 to dfa``() =
        // a*b+
        let n3 = set [
            0u,None,1u;
            0u,None,3u;
            1u,Some 'a',2u;
            2u,None,1u;
            2u,None,3u;
            3u,Some 'b',4u;
            4u,None,3u
            ]

        let nfinalLexemes = [4u,4u]

        let dfa = DFA.fromNFA(n3, nfinalLexemes)

        let dtran = set [
            0u,'a',0u;
            0u,'b',1u;
            1u,'b',1u
            ]

        let finalLexemes = [|
            set [1u],Set.empty
            |]

        Should.equal dfa.nextStates (DFATools.getNextStates dtran)
        Should.equal dfa.finalLexemes finalLexemes
    [<Fact>]
    member _.``Example 3-28: fig3-52 to fig3-54``() =
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

        let finalLexemes = [
            2u,2u
            6u,6u
            8u,8u
            ]

        let dfa = DFA.fromNFA(ntran, finalLexemes)

        // fig 3-54
        let dtran =set [
            (0u,'a',1u);
            (0u,'b',5u);
            (1u,'a',4u);
            (1u,'b',2u);
            (2u,'b',3u);
            (3u,'b',5u);
            (4u,'a',4u);
            (4u,'b',5u);
            (5u,'b',5u)]

        let finalLexemes = [|
            set [1u]   ,Set.empty
            set [3u]   ,Set.empty
            set [2u;5u],Set.empty
            |]

        Should.equal dfa.nextStates (DFATools.getNextStates dtran)
        Should.equal dfa.finalLexemes finalLexemes
