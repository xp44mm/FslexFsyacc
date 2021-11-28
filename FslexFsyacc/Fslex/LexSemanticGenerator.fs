module FslexFsyacc.Fslex.LexSemanticGenerator

open System

let tab i = 
    let space i = String.replicate i " "
    space (4*i)

let decorateSemantic (semantic:string) =
    [
        $"{tab 1}fun (lexbuf:_ list) ->"
        $"{tab 2}{semantic}"
    ] |> String.concat Environment.NewLine

