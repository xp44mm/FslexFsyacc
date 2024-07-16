namespace FslexFsyacc.YACCs

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FslexFsyacc.BNFs
open FslexFsyacc.Precedences

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
        let e = es.[i]
        let p = productions.[i]
        let y =
            match p with
            | Precedence.LastTerminalAsDummy terminals dumm ->
                Some dumm
            | _ -> None
        Should.equal e y


