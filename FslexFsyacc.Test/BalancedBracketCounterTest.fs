namespace FslexFsyacc

open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals
open FSharp.Idioms

type BalancedBracketCounterTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    static let sourceOfBrackets = SingleDataSource [
        "{}",[(0,'{'),1;(1,'}'),-1]
        "((){})[]",[
            (0,'('),1;
            (1,'('),2;
            (2,')'),-2;
            (3,'{'),3;
            (4,'}'),-3;
            (5,')'),-1;
            (6,'['),4;
            (7,']'),-4]
        ]

    static member keysOfBrackets = sourceOfBrackets.keys

    [<Theory>]
    [<MemberData(nameof BalancedBracketCounterTest.keysOfBrackets)>]
    member _.``getBrackets``(x:string) =
        let arr = x.ToCharArray()
        let counter = BalancedBracketCounter<int*char>()
        arr
        |> Array.iteri(fun i c ->
            match c with
            |'('|'['|'{' -> counter.addLeft(i,c) //字符的位置，字符作为数据
            |')'|']'|'}' -> counter.addRight(i,c)
            | _ -> ()
        )

        let y = counter.getBrackets()
        let e = sourceOfBrackets.[x]
        Should.equal y e

    [<Theory>]
    [<InlineData("{}",1)>]
    [<InlineData("((){})[]",5)>]
    member _.``getOpposite``(x:string,e:int) =
        let arr = x.ToCharArray()
        let counter = BalancedBracketCounter<_>()

        arr
        |> Array.iteri(fun i c ->
            match c with
            |'('|'['|'{' -> counter.addLeft(i) //字符的位置作为数据
            |')'|']'|'}' -> counter.addRight(i)
            | _ -> ()
        )
        //第0个字符的反括号的位置
        let z = counter.getOpposite(0)
        Should.equal z e


