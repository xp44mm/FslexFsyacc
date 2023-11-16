module FslexFsyacc.Yacc.ItemCoreCrewRecurs

open FslexFsyacc.Runtime
open FSharp.Idioms.Literal

/// 打印结果，永久化到源代码文件
let recurProductionCrew(crew:ProductionCrew) =
    let ps =
        [
            crew.production |> stringify
            crew.leftside   |> stringify
            crew.body       |> stringify
        ]
        |> String.concat ","

    $"ProductionCrew({ps})"

/// 打印结果，永久化到源代码文件
let recurItemCoreCrew(itemCoreCrew:ItemCoreCrew) =
    let proto = recurProductionCrew itemCoreCrew
    let ps =
        [
            itemCoreCrew.dot       |> stringify
            itemCoreCrew.backwards |> stringify
            itemCoreCrew.forwards  |> stringify
            itemCoreCrew.dotmax    |> stringify
            itemCoreCrew.isKernel  |> stringify
        ]
        |> String.concat ","

    $"ItemCoreCrew({proto},{ps})"
