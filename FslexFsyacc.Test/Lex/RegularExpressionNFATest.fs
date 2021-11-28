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
    member this.``Leaf test``() =
        let r1 = Character 'a'
        let nfa1 = RegularExpressionNFA.convertToNFA 0u r1
        let y = {
            transition=set [0u,Some 'a',1u];
            minState=0u;
            maxState=1u}
        //show nfa1
        Should.equal y nfa1

    [<Fact>]
    member this.``nfa3: leaf union``() =
        let r3 = Uion(Character 'a', Character 'b') // a | b
        let nfa3 = RegularExpressionNFA.convertToNFA 0u r3
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
    member this.``leaf concat test``() =
        let x = Concat(Character 'a', Character 'b') // a b
        let y = RegularExpressionNFA.convertToNFA 0u x

        let nfa = {
            transition=set [
                0u,Some 'a',1u;
                1u,Some 'b',2u];
            minState=0u;maxState=2u}
        //show y
        Should.equal y nfa

    [<Fact>]
    member this.``leaf natrual test``() =
        let x = Natural(Character 'a') // a *
        let y = RegularExpressionNFA.convertToNFA 0u x

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
    member this.``leaf positive test``() =
        let x = Positive(Character 'a') // a +
        let y = RegularExpressionNFA.convertToNFA 0u x

        let nfa = {
            transition=set [
                0u,Some 'a',1u;
                1u,None,0u];
            minState=0u;maxState=1u}
        //show y
        Should.equal y nfa

    [<Fact>]
    member this.``leaf maybe test``() =
        let x = Maybe(Character 'a') // a ?
        let y = RegularExpressionNFA.convertToNFA 0u x

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
    member this.``nfa5:natrual``() =
        let r5 = Natural(Uion(Character 'a',Character 'b'))
        let nfa5 = RegularExpressionNFA.convertToNFA 0u r5
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
    member this.``nfa7: concat``() =
        let r7 = Concat(Natural(Uion(Character 'a',Character 'b')),Character 'a')
        let nfa7 = RegularExpressionNFA.convertToNFA 0u r7
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
    member this.``Example 3-26: regex to nfa 2``() =
        let n2 = RegularExpressionNFA.convertToNFA 0u (Concat(Concat(Character 'a',Character 'b'),Character 'b'))
        let y2 = {
            transition=set [
                0u,Some 'a',1u;
                1u,Some 'b',2u;
                2u,Some 'b',3u];
            minState=0u;maxState=3u}
        Should.equal y2 n2


    [<Fact>]
    member this.``Example 3-26: regex to nfa 3``() =
        let n3 = RegularExpressionNFA.convertToNFA 0u (Concat(Natural(Character 'a'),Positive(Character 'b')))
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
    member this.``fig 3-34: regex to nfa``() =
        let r1 = Uion(Character 'a',Character 'b')
        let r2 = Concat(Concat(Character 'a',Character 'b'),Character 'b')
        let r3 = Concat(Natural r1,r2)

        let nfa = RegularExpressionNFA.convertToNFA 0u r3
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

