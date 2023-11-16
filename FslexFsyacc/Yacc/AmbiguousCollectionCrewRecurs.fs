module FslexFsyacc.Yacc.AmbiguousCollectionCrewRecurs
open FslexFsyacc.Runtime
open FSharp.Idioms
open FSharp.Idioms.Literal

let recurProductionsCrew (crew:ProductionsCrew) =
    let ps =
        [
            crew.inputProductionList |> stringify
            crew.startSymbol |> stringify
            crew.mainProductions |> stringify
            crew.augmentedProductions |> stringify
        ]
        |> String.concat ","
    $"ProductionsCrew({ps})"

let recurNullableCrew (crew:NullableCrew) =
    let prototype = recurProductionsCrew crew
    let ps =
        [
            crew.symbols      |> stringify
            crew.nonterminals |> stringify
            crew.terminals    |> stringify
            crew.nullables    |> stringify
        ]
        |> String.concat ","
    $"NullableCrew({prototype},{ps})"

let recurFirstLastCrew (crew:FirstLastCrew) =
    let prototype = recurNullableCrew crew
    let ps =
        [
           crew.firsts |> stringify
           crew.lasts  |> stringify
        ]
        |> String.concat ","
    $"FirstLastCrew({prototype},{ps})"

let recurFollowPrecedeCrew (crew:FollowPrecedeCrew) =
    let prototype = recurFirstLastCrew crew
    let ps =
        [
            crew.follows  |> stringify
            crew.precedes |> stringify
        ]
        |> String.concat ","
    $"FollowPrecedeCrew({prototype},{ps})"

let recurItemCoresCrew (crew:ItemCoresCrew) =
    let prototype = recurFollowPrecedeCrew crew
    let ps =
        [
            crew.itemCoreCrews  |> stringify
        ]
        |> String.concat ","
    $"ItemCoresCrew({prototype},{ps})"

let recurLALRCollectionCrew (crew:LALRCollectionCrew) =
    let prototype = recurItemCoresCrew crew
    let ps =
        [
            crew.kernels  |> stringify
            crew.closures |> stringify
            crew.GOTOs    |> stringify
        ]
        |> String.concat ","
    $"LALRCollectionCrew({prototype},{ps})"

let recurAmbiguousCollectionCrew (crew:AmbiguousCollectionCrew) =
    let prototype = recurLALRCollectionCrew crew
    let ps =
        [
            crew.conflictedItemCores  |> stringify
        ]
        |> String.concat ","
    $"AmbiguousCollectionCrew({prototype},{ps})"







