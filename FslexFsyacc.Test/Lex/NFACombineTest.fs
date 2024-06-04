namespace FslexFsyacc.Lex

open Xunit
open Xunit.Abstractions
open FSharp.Idioms
open FSharp.xUnit

/// Example 3-24: Test Algorithm 3-23
type NFACombineTest(output:ITestOutputHelper) =
    let show res = 
        res
        |> Literal.stringify
        |> output.WriteLine
    
    [<Fact>]
    member _.``union``() =
        let i = 0u
        let s = 
            {
                transition =  set [1u, Some 'a', 2u]
                minState = 1u
                maxState = 2u
            }

        let t = 
            {
                transition =  set [3u, Some 'b', 4u]
                minState = 3u
                maxState = 4u
            }

        let y = NFACombine.union i s t

        let res = {
            transition=set [
                0u,None,1u;
                0u,None,3u;
                1u,Some 'a',2u;
                2u,None,5u;
                3u,Some 'b',4u;
                4u,None,5u
                ];
            minState=0u;
            maxState=5u
            }

        //show y
        Should.equal y res

    [<Fact>]
    member _.``concat``() =
        let s = 
            {
                transition =  set [0u, Some 'a', 1u]
                minState = 0u
                maxState = 1u
            }

        let t = 
            {
                transition =  set [1u, Some 'b', 2u]
                minState = 1u
                maxState = 2u
            }

        let y = NFACombine.concat s t

        //show y
        let res = {
            transition=set [
                0u,Some 'a',1u;
                1u,Some 'b',2u];
            minState=0u;
            maxState=2u}

        Should.equal y res

    [<Fact>]
    member _.``natrual``() =
        let i = 0u
        let s = 
            {
                transition =  set [1u, Some 'a', 2u]
                minState = 1u
                maxState = 2u
            }

        let y = NFACombine.natural i s

        //show y
        let res = {
            transition=set [
                0u,None,1u;
                0u,None,3u;
                1u,Some 'a',2u;
                2u,None,1u;
                2u,None,3u];
            minState=0u;
            maxState=3u}
        Should.equal y res

    [<Fact>]
    member _.``positive``() =
        let s = 
            {
                transition =  set [0u, Some 'a', 1u]
                minState = 0u
                maxState = 1u
            }

        let y = NFACombine.positive s
        //show y
        let res = {
            transition=set [
                0u,Some 'a',1u;
                1u,None,0u];
            minState=0u;
            maxState=1u}

        Should.equal y res

    [<Fact>]
    member _.``maybe``() =
        let i = 0u
        let s = 
            {
                transition =  set [1u, Some 'a', 2u]
                minState = 1u
                maxState = 2u
            }

        let y = NFACombine.maybe i s

        //show y
        let res = {
            transition=set [
                0u,None,1u;
                0u,None,3u;
                1u,Some 'a',2u;
                2u,None,3u];
            minState=0u;
            maxState=3u}
        
        Should.equal y res


