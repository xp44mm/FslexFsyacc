namespace FslexFsyacc.Runtime.YACCs

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

type PrecedenceTest (output: ITestOutputHelper) =

    [<Theory>]
    [<Natural(3)>]
    member _.``lastTerminal``(i:int) =
        let terminals = set ["+";"*"]
        let productions = [
            [ "E"; "E"; "+"; "T" ]
            [ "E"; "T" ]
            [ "T"; "T"; "*"; "F" ]
            ]

        let es = [
            Some "+";
            None
            Some "*"
        ]
        let y = Precedence.lastTerminal terminals productions.[i]
        Should.equal es.[i] y

    [<Theory>]
    [<Natural(2)>]
    member _.``tryGetDummy``(i:int) =
        let dummyTokens = Map [
            [ "E"; "-"; "E" ],"MINUS"
            ]

        let terminals = set ["+";"*";"-";"/"]

        let productions = [
            [ "E"; "E"; "-"; "E" ]
            [ "E"; "-"; "E" ]
            ]

        let es = [
            Some "-";
            Some "MINUS"
        ]

        let y = Precedence.tryGetDummy dummyTokens terminals productions.[i]

        Should.equal es.[i] y



