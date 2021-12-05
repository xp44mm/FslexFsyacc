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
let getTag = function
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
let getLexeme = function
    | HEADER x -> box x
    | IDENTIFIER x -> box x
    | QUOTE x    -> box x
    | SEMANTIC x -> box x
    | _        -> null

open FSharp.Idioms
open System.Text.RegularExpressions
open FslexFsyacc.SourceText

let tokenize inp =
    let rec loop (inp: string) =
        seq {
            match inp with
            | "" -> ()
            | On tryWhiteSpace        (_, rest) -> yield! loop rest
            | On tryLineTerminator    (_, rest) -> yield! loop rest
            | On trySingleLineComment (_, rest) -> yield! loop rest
            | On tryFsMultiLineComment  (_, rest) -> yield! loop rest
            | Prefix @"\w+" (lexeme, rest) ->
                yield IDENTIFIER lexeme
                yield! loop rest

            | On tryDoubleStringLiteral (lexeme, rest) ->
                yield QUOTE <| unquote lexeme
                yield! loop rest

            | On (tryFirstChar ':') rest ->
                yield COLON
                yield! loop rest

            | On (tryFirstChar '|') rest ->
                yield BAR
                yield! loop rest

            | On (tryFirstChar ';') rest ->
                yield SEMICOLON
                yield! loop rest

            | Prefix "%%+" (_, rest) ->
                yield PERCENT
                yield! loop rest

            | Prefix "%[a-z]+" (lexeme, rest) ->
                yield
                    match lexeme with
                    | "%left" -> LEFT
                    | "%right" -> RIGHT
                    | "%nonassoc" -> NONASSOC
                    | "%prec" -> PREC
                    | never -> failwith ""
                yield! loop rest

            | On trySemanticAction (x, rest) ->
                let x = x.[1..x.Length-2].Trim()
                yield SEMANTIC x
                yield! loop rest
            | On tryHeader (y, rest) ->
                let x = 
                    y.[2..y.Length-3].Trim()
                yield HEADER x
                yield! loop rest
            | never -> failwithf "%A" never
        }

    seq {
        yield BOF
        yield! loop inp
        yield EOF
    }
