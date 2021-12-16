module FslexFsyacc.Fslex.LexSemanticGenerator

open System
open FSharp.Idioms.StringOps

let decorateSemantic (semantic:string) =
    [
        $"{indent 1}fun (lexbuf:(int*int*_)list) ->"
        (2*4,semantic) ||> indentCodeBlock 
    ] |> String.concat Environment.NewLine

