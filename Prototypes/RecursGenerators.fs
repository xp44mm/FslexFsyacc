module Prototypes.RecursGenerators
open FSharp.Idioms
open FSharp.Idioms.Line

let printFirstRecurLines (crew:CrewInfo) =
    [
        $"let recur{crew.typeName} (crew:{crew.typeName}) ="
        "let ps =" |> space4 1
        "[" |> space4 2
        for (nm,tp) in crew.fields do
            $"crew.{nm} |> stringify" |> space4 3
        "]" |> space4 2
        "|> String.concat \",\"" |> space4 2
        $"$\"{crew.typeName}({{ps}})\"" |> space4 1
    ]

let printDerivedRecurLines (crew:CrewInfo) =
    [
        $"let recur{crew.typeName} (crew:{crew.typeName}) ="
        $"let prototype = recur{crew.prototype.Value} crew" |> space4 1
        $"let ps =" |> space4 1
        "[" |> space4 2
        for (nm,tp) in crew.fields do
            $"crew.{nm} |> stringify" |> space4 3
        "]" |> space4 2
        "|> String.concat \",\"" |> space4 2
        $"$\"{crew.typeName}({{prototype}},{{ps}})\"" |> space4 1
    ]        

let printPrototypeChainsRecurs (crews:list<CrewInfo>) =
    match crews with
    | [] | [_] -> failwith "chain error: at least 2"
    | first:: crews ->
        [
            yield! 
                if first.prototype.IsNone then
                    printFirstRecurLines first
                else failwith "chain error"
            for crew in crews do
            yield!
                if crew.prototype.IsSome then
                    printDerivedRecurLines crew
                else failwith "chain error"
        ]
