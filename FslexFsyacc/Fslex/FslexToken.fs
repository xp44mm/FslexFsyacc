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

let getTag = function
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

let getLexeme = function
    | HEADER x -> box x
    | ID x -> box x
    | QUOTE x -> box x
    | SEMANTIC x -> box x
    | HOLE x -> box x
    | _ -> null

open FSharp.Idioms
open System.Text.RegularExpressions
open FslexFsyacc.SourceText

let tryHole =
    Regex @"^\<\w+\>"
    |> tryRegexMatch

let tokenize inp =
    let rec loop (inp:string) =
        seq {
            match inp with
            | "" -> ()

            | Prefix @"[\s-[\n]]+" (_,rest)
                -> yield! loop rest

            | On trySingleLineComment (_,rest)
                -> yield! loop rest

            | On tryFsMultiLineComment (_,rest)
                -> yield! loop rest

            | PrefixChar '\n' rest ->
                yield LF
                yield! loop rest

            | Prefix @"\w+" (lexeme,rest) ->
                yield ID lexeme
                yield! loop rest

            | Prefix """(?:"(\\[/'"bfnrt\\]|\\u[0-9a-fA-F]{4}|[^\\"])*")""" (lexeme,rest) ->
                yield QUOTE <| unquote lexeme
                yield! loop rest

            | Prefix @"%%+" (_,rest) ->
                yield PERCENT
                yield! loop rest

            | PrefixChar '=' rest ->
                yield EQUALS
                yield! loop rest

            | PrefixChar '(' rest ->
                yield LPAREN
                yield! loop rest

            | PrefixChar ')' rest ->
                yield RPAREN
                yield! loop rest

            | PrefixChar '[' rest ->
                yield LBRACK
                yield! loop rest

            | PrefixChar ']' rest ->
                yield RBRACK
                yield! loop rest

            | PrefixChar '+' rest ->
                yield PLUS
                yield! loop rest

            | PrefixChar '*' rest ->
                yield STAR
                yield! loop rest

            | PrefixChar '/' rest ->
                yield SLASH
                yield! loop rest

            | PrefixChar '|' rest ->
                yield BAR
                yield! loop rest

            | PrefixChar '?' rest ->
                yield QMARK
                yield! loop rest

            | On tryHole (x, rest) ->
                let x = x.[1..x.Length-2]
                yield HOLE x
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

            | never -> failwith never
        }

    seq {
        yield BOF
        yield! loop inp
        yield EOF
    }
