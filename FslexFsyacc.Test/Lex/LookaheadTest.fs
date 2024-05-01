namespace FslexFsyacc.Runtime.Lex

open Xunit
open Xunit.Abstractions
open FSharp.Idioms
open FSharp.xUnit

type LookaheadTest(output:ITestOutputHelper) =
    let show res = 
        res
        |> Literal.stringify
        |> output.WriteLine

    //fig. 3-55 省略括號中的路徑

    [<Fact>]
    member _.``combine test``() =
        let n1 = {
            transition = set [
                0u, Some 'I', 1u 
                1u, Some 'F', 2u         
                ]
            minState = 0u
            maxState = 2u
            }

        let n2 = {
            transition = set [
                3u, Some '(', 4u
                4u, Some ')', 5u
                ]
            minState = 3u
            maxState = 5u
            }

        let nfa = PatternNFAUtils.lookahead(n1, n2)
        //show nfa
        let y = {
            transition=set [
                0u,Some 'I',1u;
                1u,Some 'F',2u;
                2u,None,3u;
                3u,Some '(',4u;
                4u,Some ')',5u];
            minState=0u;
            lexemeState=2u;
            maxState=5u}

        Should.equal y nfa
