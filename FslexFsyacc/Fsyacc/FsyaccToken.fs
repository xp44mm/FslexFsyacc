﻿module FslexFsyacc.Fsyacc.FsyaccToken

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
let getTag(pos,len,token) = 
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
let getLexeme(pos,len,token) = 
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
            | On tryWhiteSpace (x, rest) -> 
                let len = x.Length
                yield! loop (pos+len) rest
            | On tryLineTerminator (x, rest) ->
                let len = x.Length
                yield! loop (pos+len) rest

            | On trySingleLineComment  (x, rest) ->
                let len = x.Length
                yield! loop (pos+len) rest

            | On tryFsMultiLineComment (x, rest) ->
                let len = x.Length
                yield! loop (pos+len) rest

            | Prefix @"\w+" (x, rest) ->
                let len = x.Length
                yield pos, len, IDENTIFIER x
                yield! loop (pos+len) rest

            | On tryDoubleStringLiteral (x, rest) ->
                let len = x.Length
                yield pos,len,QUOTE(unquote x)
                yield! loop (pos+len) rest

            | On (tryFirstChar ':') rest ->
                yield pos, 1, COLON
                yield! loop (pos+1) rest

            | On (tryFirstChar '|') rest ->
                yield pos,1,BAR
                yield! loop (pos+1) rest

            | On (tryFirstChar ';') rest ->
                yield pos,1,SEMICOLON
                yield! loop (pos+1) rest

            | Prefix "%%+" (x, rest) ->
                let len = x.Length
                yield pos,len,PERCENT
                yield! loop (pos+len) rest

            | Prefix "%[a-z]+" (x, rest) ->
                let tok =
                    match x with
                    | "%left" -> LEFT
                    | "%right" -> RIGHT
                    | "%nonassoc" -> NONASSOC
                    | "%prec" -> PREC
                    | never -> failwith ""
                let len = x.Length
                yield pos,len,tok
                yield! loop (pos+len) rest

            | On trySemanticAction (x, rest) ->
                let len = x.Length
                yield pos, len, SEMANTIC(x.[1..x.Length-2].Trim())
                yield! loop (pos+len) rest

            | On tryHeader (x, rest) ->
                let len = x.Length                
                yield pos,len,HEADER (x.[2..x.Length-3].Trim())
                yield! loop (pos+len) rest
            | never -> failwithf "%A" never
        }
    loop 0 inp

//let isPercent(pos,token) = 
//    match token with
//    | PERCENT -> true
//    | _       -> false
