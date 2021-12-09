module FslexFsyacc.Fslex.LexSemanticGenerator

open System

let indent i = 
    let space i = String.replicate i " "
    space (4*i)

let decorateSemantic (semantic:string) =
    [
        $"{indent 1}fun (lexbuf:(int*_) list) ->"
        $"{indent 2}{semantic}"
    ] |> String.concat Environment.NewLine

