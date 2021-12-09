﻿module Expr.ExprToken

//"(" ")" "*" "+" "-" "/" "NUMBER"
type ExprToken =
    | NUMBER of float
    | LPAREN
    | RPAREN
    | PLUS
    | MINUS
    | STAR
    | DIV

let getTag(pos,len,token) = 
    match token with
    | NUMBER _ -> "NUMBER"
    | LPAREN   -> "("
    | RPAREN   -> ")"
    | PLUS     -> "+"
    | MINUS   -> "-"
    | STAR -> "*"
    | DIV    -> "/"

let getLexeme(pos,len,token) = 
    match token with
    | NUMBER n -> box n
    | _   -> null

open FSharp.Idioms.StringOps

let tokenize(inp:string) =
    let rec loop pos (inp:string) =
        seq {
            match inp with
            | "" -> ()

            | Prefix @"\s+" (x, rest) ->
                let len = x.Length
                let pos = pos + len
                yield! loop pos rest

            | Prefix @"\d+(\.\d+)?" (x, rest) ->
                let len = x.Length
                yield pos,len,NUMBER <| System.Double.Parse(x)
                let pos = pos + len
                yield! loop pos rest

            | PrefixChar '(' rest ->
                let len = 1
                yield pos,len, LPAREN
                let pos = pos + len
                yield! loop pos rest

            | PrefixChar ')' rest ->
                let len = 1
                yield pos,len, RPAREN
                let pos = pos + len
                yield! loop pos rest

            | PrefixChar '+' rest ->
                let len = 1
                yield pos, len, PLUS
                let pos = pos + len
                yield! loop pos rest

            | PrefixChar '-' rest ->
                let len = 1
                yield pos, len, MINUS
                let pos = pos + len
                yield! loop pos rest

            | PrefixChar '*' rest ->
                let len = 1
                yield pos, len, STAR
                let pos = pos + len
                yield! loop pos rest

            | PrefixChar '/' rest ->
                let len = 1
                yield pos, len, DIV
                let pos = pos + len
                yield! loop pos rest
            | never -> failwith never
        }

    loop 0 inp
