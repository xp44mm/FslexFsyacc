module FslexFsyacc.Fslex.LexSemanticGenerator

open System
open FSharp.Idioms

let decorateSemantic (semantic:string) =
    [
        $"{space 4}fun (lexbuf:token list) ->"
        semantic |> Line.indentCodeBlock (2*4)
    ] |> String.concat Environment.NewLine

