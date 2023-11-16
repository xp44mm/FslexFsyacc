module Prototypes.RenderMembersGenerators

open FSharp.Idioms
open FSharp.Idioms.Line

let generateRenderMembersOf (crew:CrewInfo) =
    [
        "let ids ="
        "["
        for (nm,tp) in crew.fields do
            $"\"{nm}\"" |> space4 3
        "]"
    
        "let vs ="
        "["
        for (nm,tp) in crew.fields do
            // stringify 需要自定义crew
            $"crew.{nm} |> stringify" |> space4 3
        "]"
        "List.zip ids vs"
        "|> List.map(fun(a,b) -> $\"let {a} = {b}\")"
    ]
