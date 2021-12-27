module FslexFsyacc.Fslex.LexSemanticGenerator

open System
open FSharp.Idioms

let decorateSemantic (semantic:string) =
    [
        $"{indent 1}fun (lexbuf:(int*int*_)list) ->"
        (2*4,semantic) ||> Line.indentCodeBlock 
    ] |> String.concat Environment.NewLine

