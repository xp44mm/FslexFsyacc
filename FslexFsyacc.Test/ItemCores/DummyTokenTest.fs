namespace FslexFsyacc.ItemCores

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms
open FSharp.Idioms.Literal

type DummyTokenTest (output:ITestOutputHelper) =

    [<Fact>]
    member _.``lastTerminal test``() =
        let terminals = set ["-"]
        let production = ["e";"-";"e"]
        let y = DummyToken.lastTerminal terminals production
        output.WriteLine(stringify y)
        let e = Some(0,"-")
        Should.equal e y
