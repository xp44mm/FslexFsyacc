module FslexFsyacc.Fsyacc.FsyaccToken

type FsyaccToken =
    | HEADER of string
    | IDENTIFIER of string
    | QUOTE of string
    | SEMANTIC of string
    | COLON
    | SEMICOLON
    | BAR
    | PERCENT
    | LEFT
    | RIGHT
    | NONASSOC
    | PREC
    | BOF
    | EOF

/// the tag of token
let getTag(pos,token) = 
    match token with
    | HEADER _ -> "HEADER"
    | IDENTIFIER _ -> "IDENTIFIER"
    | QUOTE _    -> "QUOTE"
    | SEMANTIC _ -> "SEMANTIC"
    | COLON        -> ":"
    | SEMICOLON    -> ";"
    | BAR          -> "|"
    | PERCENT      -> "%%"
    | LEFT         -> "%left"
    | RIGHT        -> "%right"
    | NONASSOC     -> "%nonassoc"
    | PREC         -> "%prec"
    | BOF          -> "BOF"
    | EOF          -> "EOF"

///用于求解的栈
let getLexeme(pos,token) = 
    match token with
    | HEADER x -> box x
    | IDENTIFIER x -> box x
    | QUOTE x    -> box x
    | SEMANTIC x -> box x
    | _        -> null

open FSharp.Idioms
open System.Text.RegularExpressions
open FslexFsyacc.SourceText

let tokenize inp =
    let rec loop (pos:int) (inp: string) =
        seq {
            match inp with
            | "" -> ()
            | On tryWhiteSpace         (x, rest) -> 
                let pos = pos + x.Length
                yield! loop pos rest
            | On tryLineTerminator     (x, rest) ->
                let pos = pos + x.Length
                yield! loop pos rest
            | On trySingleLineComment  (x, rest) ->
                let pos = pos + x.Length
                yield! loop pos rest
            | On tryFsMultiLineComment (x, rest) ->
                let pos = pos + x.Length
                yield! loop pos rest

            | Prefix @"\w+" (x, rest) ->
                yield pos, IDENTIFIER x
                let pos = pos + x.Length
                yield! loop pos rest

            | On tryDoubleStringLiteral (x, rest) ->
                yield pos,QUOTE <| unquote x
                let pos = pos + x.Length
                yield! loop pos rest

            | On (tryFirstChar ':') rest ->
                yield pos, COLON
                yield! loop (pos+1) rest

            | On (tryFirstChar '|') rest ->
                yield pos,BAR
                yield! loop (pos+1) rest

            | On (tryFirstChar ';') rest ->
                yield pos,SEMICOLON
                yield! loop (pos+1) rest

            | Prefix "%%+" (x, rest) ->
                yield pos,PERCENT
                let pos = pos + x.Length
                yield! loop pos rest

            | Prefix "%[a-z]+" (x, rest) ->
                let tok =
                    match x with
                    | "%left" -> LEFT
                    | "%right" -> RIGHT
                    | "%nonassoc" -> NONASSOC
                    | "%prec" -> PREC
                    | never -> failwith ""
                yield pos,tok
                let pos = pos + x.Length
                yield! loop pos rest

            | On trySemanticAction (x, rest) ->
                let y = x.[1..x.Length-2].Trim()
                yield pos, SEMANTIC y
                let pos = pos + x.Length
                yield! loop pos rest

            | On tryHeader (x, rest) ->
                let z = x.[2..x.Length-3].Trim()
                yield pos,HEADER z
                let pos = pos + x.Length
                yield! loop pos rest
            | never -> failwithf "%A" never
        }

    seq {
        //yield (0,BOF)
        yield! loop 0 inp
        yield (inp.Length,EOF)
    }

let isPercent(pos,token) = 
    match token with
    | PERCENT -> true
    | _       -> false
