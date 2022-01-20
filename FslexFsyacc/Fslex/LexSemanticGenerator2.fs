module FslexFsyacc.Fslex.LexSemanticGenerator2

open System
open FSharp.Idioms

let decorateSemantic (semantic:string) =
    [
        $"fun (lexbuf:token list) ->"
        semantic |> Line.indentCodeBlock 4
    ] |> String.concat Environment.NewLine

