module FSharpAnatomy.FSharpTokenUtils

open FSharpAnatomy.FSharpTokenScratch
open FslexFsyacc.Runtime

open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.StringOps

open System.Text.RegularExpressions

let tokenize(inp:string) =
    let rec loop index (inp:string) =
        seq {
            match inp with
            | "" -> ()

            | On(tryMatch(Regex @"^\s+")) (x, rest) ->
                let len = x.Length
                let index = index + len
                yield! loop index rest

            | On tryIdent (x, rest) ->
                let pos =
                    {
                        index = index
                        length = 1
                        value =
                            match x with
                            | "_" -> UNDERSCORE
                            | "struct" -> STRUCT
                            | _ -> IDENT x
                    }
                yield pos
                yield! loop pos.nextIndex rest

            | On tryTypar (x, rest) ->
                let pos = {
                    index = index
                    length = x.Length
                    value = TYPAR x.[1..]
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On tryInlineTypar (x, rest) ->
                let pos = {
                    index = index
                    length = x.Length
                    value = INLINE_TYPAR x.[1..]
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryFirst '#') rest ->
                let pos = {
                    index = index
                    length = 1
                    value = HASH
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryFirst '(') rest ->
                let pos = {
                    index = index
                    length = 1
                    value = LPAREN
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryFirst ')') rest ->
                let pos = {
                    index = index
                    length = 1
                    value = RPAREN
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryFirst '[') rest ->
                let pos = {
                    index = index
                    length = 1
                    value = LBRACK
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryFirst ']') rest ->
                let pos = {
                    index = index
                    length = 1
                    value = RBRACK
                }
                yield pos
                yield! loop pos.nextIndex rest





            | On(tryFirst '*') rest ->
                let pos = {
                    index = index
                    length = 1
                    value = STAR
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryFirst ',') rest ->
                let pos = {
                    index = index
                    length = 1
                    value = COMMA
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryFirst '.') rest ->
                let pos = {
                    index = index
                    length = 1
                    value = DOT
                }
                yield pos
                yield! loop pos.nextIndex rest
            | On(tryFirst ':') rest ->
                let pos = {
                    index = index
                    length = 1
                    value = COLON
                }
                yield pos
                yield! loop pos.nextIndex rest
            | On(tryFirst ';') rest ->
                let pos = {
                    index = index
                    length = 1
                    value = SEMICOLON
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryFirst '<') rest ->
                let pos = {
                    index = index
                    length = 1
                    value = LESS
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryFirst '>') rest ->
                let pos = {
                    index = index
                    length = 1
                    value = GREATER
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryStart "->") rest ->
                let pos = {
                    index = index
                    length = 1
                    value = RARROW
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryStart ":>") rest ->
                let pos = {
                    index = index
                    length = 1
                    value = COLON_GREATER
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryStart "{|") rest ->
                let pos = {
                    index = index
                    length = 1
                    value = LBRACE_BAR
                }
                yield pos
                yield! loop pos.nextIndex rest

            | On(tryStart "|}") rest ->
                let pos = {
                    index = index
                    length = 1
                    value = BAR_RBRACE
                }
                yield pos
                yield! loop pos.nextIndex rest
            | _ -> failwith "unimpl tokenize case"
        }

    loop 0 inp

let getTag (token:Position<FSharpToken>) =
    match token.value with
    | HASH -> "#"
    | LBRACK -> "["
    | RBRACK -> "]"
    | LPAREN -> "("
    | RPAREN -> ")"
    | LBRACE_BAR -> "{|"
    | BAR_RBRACE -> "|}"
    | STAR -> "*"
    | COMMA -> ","
    | RARROW -> "->"
    | DOT -> "."
    | COLON -> ":"
    | COLON_GREATER -> ":>"
    | SEMICOLON -> ";"
    | LESS -> "<"
    | GREATER -> ">"
    | UNDERSCORE -> "_"
    | IDENT _ -> "IDENT"
    | TYPAR _ -> "TYPAR"
    | INLINE_TYPAR _ -> "INLINE_TYPAR"
    | STRUCT -> "struct"
    | WHITESPACE _ -> "WHITESPACE"
    | COMMENT _ -> "COMMENT"
    | ARRAY_TYPE_SUFFIX _ -> "ARRAY_TYPE_SUFFIX"

let getLexeme (token:Position<FSharpToken>) =
    match token.value with
    | HASH   
    | LBRACK 
    | RBRACK 
    | LPAREN 
    | RPAREN 
    | LBRACE_BAR
    | BAR_RBRACE
    | STAR   
    | COMMA  
    | RARROW 
    | DOT
    | COLON
    | COLON_GREATER
    | SEMICOLON
    | LESS
    | GREATER
    | UNDERSCORE
    | STRUCT
        -> null
    | IDENT             x -> box x
    | TYPAR             x -> box x
    | INLINE_TYPAR      x -> box x
    | WHITESPACE        x -> box x
    | COMMENT           x -> box x
    | ARRAY_TYPE_SUFFIX x -> box x
