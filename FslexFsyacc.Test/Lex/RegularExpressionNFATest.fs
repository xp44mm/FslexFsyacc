namespace FslexFsyacc.Lex

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

/// Example 3-24: Test Algorithm 3-23
type RegularExpressionNFATest(output:ITestOutputHelper) =
    let show res = 
        res
        |> Literal.stringify
        |> output.WriteLine
    
    [<Fact>]
    member _.``Leaf test``() =
        let r1 = Atomic 'a'
        let nfa1 = RegularNFAUtils.fromRgx 0u r1
        let y = {
            transition=set [0u,Some 'a',1u];
            minState=0u;
            maxState=1u}
        //show nfa1
        Should.equal y nfa1

    [<Fact>]
    member _.``nfa3: leaf union``() =
        let r3 = Either(Atomic 'a', Atomic 'b') // a | b
        let nfa3 = RegularNFAUtils.fromRgx 0u r3
        let y = {
            transition=set [
                0u,None,1u;
                0u,None,3u;
                1u,Some 'a',2u;
                2u,None,5u;
                3u,Some 'b',4u;
                4u,None,5u];
            minState=0u;maxState=5u}
        //show nfa3
        Should.equal y nfa3

    [<Fact>]
    member _.``leaf concat test``() =
        let x = Both(Atomic 'a', Atomic 'b') // a b
        let y = RegularNFAUtils.fromRgx 0u x

        let nfa = {
            transition=set [
                0u,Some 'a',1u;
                1u,Some 'b',2u];
            minState=0u;maxState=2u}
        //show y
        Should.equal y nfa

    [<Fact>]
    member _.``leaf natrual test``() =
        let x = Natural(Atomic 'a') // a *
        let y = RegularNFAUtils.fromRgx 0u x

        let nfa = {
            transition=set [
                0u,None,1u;
                0u,None,3u;
                1u,Some 'a',2u;
                2u,None,1u;
                2u,None,3u];
            minState=0u;maxState=3u}
        //show y
        Should.equal y nfa

    [<Fact>]
    member _.``leaf positive test``() =
        let x = Plural(Atomic 'a') // a +
        let y = RegularNFAUtils.fromRgx 0u x

        let nfa = {
            transition=set [
                0u,Some 'a',1u;
                1u,None,0u];
            minState=0u;maxState=1u}
        //show y
        Should.equal y nfa

    [<Fact>]
    member _.``leaf maybe test``() =
        let x = Optional(Atomic 'a') // a ?
        let y = RegularNFAUtils.fromRgx 0u x

        let nfa = {
            transition=set [
                0u,None,1u;
                0u,None,3u;
                1u,Some 'a',2u;
                2u,None,3u];
            minState=0u;maxState=3u}
        //show y
        Should.equal y nfa

    [<Fact>]
    member _.``nfa5:natrual``() =
        let r5 = Natural(Either(Atomic 'a',Atomic 'b'))
        let nfa5 = RegularNFAUtils.fromRgx 0u r5
        let y = {
            transition=set [
                0u,None,1u;
                0u,None,7u;
                1u,None,2u;
                1u,None,4u;
                2u,Some 'a',3u;
                3u,None,6u;
                4u,Some 'b',5u;
                5u,None,6u;
                6u,None,1u;
                6u,None,7u];
            minState=0u;maxState=7u}
        //show nfa5
        Should.equal y nfa5

    [<Fact>]
    member _.``nfa7: concat``() =
        let r7 = Both(Natural(Either(Atomic 'a',Atomic 'b')),Atomic 'a')
        let nfa7 = RegularNFAUtils.fromRgx 0u r7
        let y = {
            transition=set [
                0u,None,1u;
                0u,None,7u;
                1u,None,2u;
                1u,None,4u;
                2u,Some 'a',3u;
                3u,None,6u;
                4u,Some 'b',5u;
                5u,None,6u;
                6u,None,1u;
                6u,None,7u;
                7u,Some 'a',8u];
            minState=0u;maxState=8u}
        //show nfa7
        Should.equal y nfa7

    [<Fact>]
    member _.``Example 3-26: regex to nfa 2``() =
        let n2 = RegularNFAUtils.fromRgx 0u (Both(Both(Atomic 'a',Atomic 'b'),Atomic 'b'))
        let y2 = {
            transition=set [
                0u,Some 'a',1u;
                1u,Some 'b',2u;
                2u,Some 'b',3u];
            minState=0u;maxState=3u}
        Should.equal y2 n2


    [<Fact>]
    member _.``Example 3-26: regex to nfa 3``() =
        let n3 = RegularNFAUtils.fromRgx 0u (Both(Natural(Atomic 'a'),Plural(Atomic 'b')))
        let y3 = {
            transition=set [
                0u,None,1u;
                0u,None,3u;
                1u,Some 'a',2u;
                2u,None,1u;
                2u,None,3u;
                3u,Some 'b',4u;
                4u,None,3u];
            minState=0u;maxState=4u}
        Should.equal y3 n3

    [<Fact>]
    member _.``fig 3-34: regex to nfa``() =
        let r1 = Either(Atomic 'a',Atomic 'b')
        let r2 = Both(Both(Atomic 'a',Atomic 'b'),Atomic 'b')
        let r3 = Both(Natural r1,r2)

        let nfa = RegularNFAUtils.fromRgx 0u r3
        let y = {
            transition=set [
                0u,None,1u;
                0u,None,7u;
                1u,None,2u;
                1u,None,4u;
                2u,Some 'a',3u;
                3u,None,6u;
                4u,Some 'b',5u;
                5u,None,6u;
                6u,None,1u;
                6u,None,7u;
                7u,Some 'a',8u;
                8u,Some 'b',9u;
                9u,Some 'b',10u];
            minState=0u;maxState=10u}
        //show nfa
        Should.equal y nfa

