module FslexFsyacc.Fslex.LexSemanticGenerator

open System
open FSharp.Idioms
open FSharp.Literals

let decorateSemantic (semantic:string) =
    [
        "fun (lexbuf:token list) ->"
        semantic |> Line.indentCodeBlock 4
    ] |> String.concat Environment.NewLine

let renderRule (finals:uint32[]) (lexemes:uint32[]) (fn:string) =
    let f = Literal.stringify finals
    let l = Literal.stringify lexemes
    $"{f},{l},{fn}"
