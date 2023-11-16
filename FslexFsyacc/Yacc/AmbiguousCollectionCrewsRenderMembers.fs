module FslexFsyacc.Yacc.AmbiguousCollectionCrewsMembersRender

open FslexFsyacc.Runtime
open FSharp.Idioms
open FSharp.Idioms.Literal

let renderProductionsCrewMembers (crew:ProductionsCrew) =
    let a =
        [
            "inputProductionList";"startSymbol";"mainProductions";"augmentedProductions"
        ]
    
    [
        crew.inputProductionList |> stringify
        crew.startSymbol |> stringify
        crew.mainProductions |> stringify
        crew.augmentedProductions |> stringify
    ]
    |> List.zip a
    |> List.map(fun(a,b) -> $"let {a} = {b}")

let renderNullableCrewMembers (crew:NullableCrew) =
    let a =
        [
            "symbols"
            "nonterminals"
            "terminals"
            "nullables"
        ]
    let b =
        [
            crew.symbols      |> stringify
            crew.nonterminals |> stringify
            crew.terminals    |> stringify
            crew.nullables    |> stringify
        ]
    [
        yield! renderProductionsCrewMembers crew
        yield!
            List.zip a b
            |> List.map(fun(a,b) -> $"let {a} = {b}")
    ]

let renderFirstLastCrewMembers (crew:FirstLastCrew) =
    let a =
        [
            "firsts"
            "lasts"
        ]
    let b =
        [
            crew.firsts |> stringify
            crew.lasts |> stringify
        ]
    [
        yield! renderNullableCrewMembers crew
        yield!
            List.zip a b
            |> List.map(fun(a,b) -> $"let {a} = {b}")
    ]

let renderFollowPrecedeCrewMembers (crew:FollowPrecedeCrew) =
    let a =
        [
            "follows"
            "precedes"
        ]
    let b =
        [
            crew.follows |> stringify
            crew.precedes |> stringify
        ]
    [
        yield! renderFirstLastCrewMembers crew
        yield!
            List.zip a b
            |> List.map(fun(a,b) -> $"let {a} = {b}")
    ]

let renderItemCoresCrewMembers (crew:ItemCoresCrew) =
    let a =
        [
            "itemCoreCrews"
        ]
    let b =
        [
            crew.itemCoreCrews
            |> Map.map(fun x y -> ItemCoreCrewRecurs.recurItemCoreCrew y)
            |> Seq.map(fun(KeyValue(x,y))-> $"{stringify x},{y}")
            |> String.concat ";"
            |> sprintf "Map [%s]"
        ]
    [
        yield! renderFollowPrecedeCrewMembers crew
        yield!
            List.zip a b
            |> List.map(fun(a,b) -> $"let {a} = {b}")
    ]

let renderLALRCollectionCrewMembers (crew:LALRCollectionCrew) =
    let a =
        [
            "kernels"
            "closures"
            "GOTOs"

        ]
    let b =
        [
            crew.kernels |> stringify
            crew.closures |> stringify
            crew.GOTOs |> stringify

        ]
    [
        yield! renderItemCoresCrewMembers crew
        yield!
            List.zip a b
            |> List.map(fun(a,b) -> $"let {a} = {b}")
    ]


let renderAmbiguousCollectionCrewMembers (crew:AmbiguousCollectionCrew) =
    let a =
        [
            "conflictedItemCores"
        ]
    let b =
        [
            crew.conflictedItemCores |> stringify
        ]
    [
        yield! renderLALRCollectionCrewMembers crew
        yield!
            List.zip a b
            |> List.map(fun(a,b) -> $"let {a} = {b}")
    ]
