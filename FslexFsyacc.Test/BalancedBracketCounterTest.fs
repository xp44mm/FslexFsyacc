namespace FslexFsyacc

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals
open System.Text.RegularExpressions
open FSharp.Idioms
open FSharp.Idioms.StringOps
open FslexFsyacc.Runtime

type BalancedBracketCounterTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Theory>]
    [<InlineData("{}")>]
    [<InlineData("((){})[]")>]
    member _.``getBrackets``(x:string) =
        let counter = BalancedBracketCounter<_>()
        let arr = x.ToCharArray()

        arr
        |> Array.iteri(fun i c ->
            match c with
            |'('|'['|'{' -> counter.addLeft(i,c)
            |')'|']'|'}' -> counter.addRight(i,c)
            | _ -> ()
        )

        let y = counter.getBrackets()
        show y

    [<Theory>]
    [<InlineData("{}")>]
    [<InlineData("((){})[]")>]
    member _.``getOpposite``(x:string) =
        let counter = BalancedBracketCounter<_>()
        let arr = x.ToCharArray()

        arr
        |> Array.iteri(fun i c ->
            match c with
            |'('|'['|'{' -> counter.addLeft(i)
            |')'|']'|'}' -> counter.addRight(i)
            | _ -> ()
        )

        let y = counter.getBrackets()
        show y

        let z = counter.getOpposite(0)
        show z