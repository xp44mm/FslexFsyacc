namespace FslexFsyacc.Runtime.Lex

open Xunit
open Xunit.Abstractions
open FSharp.Idioms
open FSharp.xUnit

/// Example 3.29
type AnalyzerNFATest(output:ITestOutputHelper) =
    let show res = 
        res 
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``regex to nfa``() =
        let lookaheads = 
            [
               [Atomic 'a'                                    ]
               [Both(Both(Atomic 'a',Atomic 'b'),Atomic 'b')  ]
               [Both(Natural(Atomic 'a'),Plural(Atomic 'b'))]
            ]

        let nfa = AnalyzerNFAUtils.fromRgx(lookaheads)

        let y = {
            transition=set [
                0u,None,1u    ;0u,None,3u    ;0u,None,7u      ;
                1u,Some 'a',2u;3u,Some 'a',4u;4u,Some 'b',5u  ;
                5u,Some 'b',6u;7u,None,8u    ;7u,None,10u     ;8u,Some 'a',9u;
                9u,None,8u    ;9u,None,10u   ;10u,Some 'b',11u;11u,None,10u] ;
            finalLexemes=[
                2u,2u;
                6u,6u;
                11u,11u
                ]
            }

        //show nfa
        Should.equal y nfa
        
    [<Fact>]
    member _.``exercise 3-8-4-2: lookahead 0``() =
        //(a|ab)/ba
        //a+
        //b+
        let patterns = 
            [
                [Either(Atomic 'a',Both(Atomic 'a',Atomic 'b'));Both(Atomic 'b',Atomic 'a')]
                [Plural(Atomic 'a')]
                [Plural(Atomic 'b')]
            ]

        let nfa = AnalyzerNFAUtils.fromRgx patterns

        //show nfa
        let y = {
            transition=set [
                0u,None,1u    ;0u,None,11u     ;0u,None,13u    ;1u,None,2u      ;1u,None,4u;
                2u,Some 'a',3u;3u,None,7u      ;4u,Some 'a',5u ;5u,Some 'b',6u  ;6u,None,7u;
                7u,None,8u    ;8u,Some 'b',9u  ;9u,Some 'a',10u;11u,Some 'a',12u;
                12u,None,11u  ;13u,Some 'b',14u;14u,None,13u]  ;
            finalLexemes=[
                10u,7u;
                12u,12u;
                14u,14u]
        }
        Should.equal y nfa

