module FslexFsyacc.Fslex.FslexToken

type FslexToken =
    | HEADER of string
    /// \w+
    | ID of string
    /// quote string literal(unquoted)
    | QUOTE of string

    | SEMANTIC of string

    | HOLE of string

    /// =
    | EQUALS
    /// line feed
    | LF
    /// (
    | LPAREN
    /// )
    | RPAREN
    /// [
    | LBRACK
    /// ]
    | RBRACK
    /// +
    | PLUS
    /// *
    | STAR
    /// /
    | SLASH
    /// |
    | BAR
    /// ?
    | QMARK
    /// &，concat运算符，连接两个正则表达式
    | AMP
    /// %%
    | PERCENT

    | BOF
    | EOF

open FSharp.Idioms
open System.Text.RegularExpressions
open FslexFsyacc.FSharpSourceText

let tryHole =
    Regex @"^\<\w+\>"
    |> tryRegexMatch

let getTag(pos,len,token) = 
    match token with
    | HEADER _ -> "HEADER"
    | ID _ -> "ID"
    | QUOTE _ -> "QUOTE"
    | SEMANTIC _ -> "SEMANTIC"
    | HOLE _ -> "HOLE"
    | EQUALS    -> "="
    | LF        -> "\n"
    | LPAREN    -> "("
    | RPAREN    -> ")"
    | LBRACK    -> "["
    | RBRACK    -> "]"
    | PLUS      -> "+"
    | STAR      -> "*"
    | SLASH     -> "/"
    | BAR       -> "|"
    | QMARK     -> "?"
    | AMP       -> "&"
    | PERCENT   -> "%%"
    | BOF       -> "BOF"
    | EOF       -> "EOF"

let getLexeme (pos,len,token) = 
    match token with
    | HEADER x -> box x
    | ID x -> box x
    | QUOTE x -> box x
    | SEMANTIC x -> box x
    | HOLE x -> box x
    | _ -> null

let tokenize inp =
    let rec loop pos (inp:string) =
        seq {
            match inp with
            | "" -> yield pos, 0, EOF

            | On tryWhiteSpace (x, rest) ->
                let len = x.Length
                yield! loop (pos+len) rest

            | On tryLineTerminator (x, rest) ->
                let len = x.Length
                yield pos,len,LF
                yield! loop (pos+len) rest

            | On trySingleLineComment (x,rest) ->
                let len = x.Length
                yield! loop (pos+len) rest

            | On tryMultiLineComment (x,rest) ->
                let len = x.Length
                yield! loop (pos+len) rest

            | On tryWord (x,rest) ->
                let len = x.Length
                yield pos,len,ID x
                yield! loop (pos+len) rest

            | On trySingleQuoteString (x,rest) ->
                let len = x.Length
                yield pos,len,QUOTE(unquote x)
                yield! loop (pos+len) rest

            | Prefix @"%%+" (x,rest) ->
                let len = x.Length
                yield pos,len,PERCENT
                yield! loop (pos+len) rest

            | PrefixChar '=' rest ->
                let len = 1
                yield pos,len,EQUALS
                yield! loop (pos+len) rest

            | PrefixChar '(' rest ->
                let len = 1
                yield pos,len,LPAREN
                yield! loop (pos+len) rest

            | PrefixChar ')' rest ->
                let len = 1
                yield pos,len,RPAREN
                yield! loop (pos+len) rest


            | PrefixChar '[' rest ->
                let len = 1
                yield pos,len,LBRACK
                yield! loop (pos+len) rest

            | PrefixChar ']' rest ->
                let len = 1
                yield pos,len,RBRACK
                yield! loop (pos+len) rest

            | PrefixChar '+' rest ->
                let len = 1
                yield pos,len,PLUS
                yield! loop (pos+len) rest

            | PrefixChar '*' rest ->
                let len = 1
                yield pos,len,STAR
                yield! loop (pos+len) rest

            | PrefixChar '/' rest ->
                let len = 1
                yield pos,len,SLASH
                yield! loop (pos+len) rest

            | PrefixChar '|' rest ->
                let len = 1
                yield pos,len,BAR
                yield! loop (pos+len) rest

            | PrefixChar '?' rest ->
                let len = 1
                yield pos,len,QMARK
                yield! loop (pos+len) rest

            | On tryHole (x, rest) ->
                let len = x.Length
                yield pos,len,HOLE x.[1..x.Length-2]
                yield! loop (pos+len) rest

            | On trySemantic (x, rest) ->
                let len = x.Length
                yield pos,len,SEMANTIC(x.[1..x.Length-2].Trim())
                yield! loop (pos+len) rest

            | On tryHeader (x, rest) ->
                let len = x.Length
                yield pos,len,HEADER(x.[2..x.Length-3].Trim())
                yield! loop (pos+len) rest

            | never -> failwith never
        }
    
    loop 0 inp

let appendAMP (lexbuf:(int*int*_)list) =
    let last = 
        lexbuf
        |> List.exactlyOne
    let pos,len,_ = last
    [last;pos + len,0,AMP]