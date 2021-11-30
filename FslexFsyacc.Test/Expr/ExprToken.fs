module Expr.ExprToken

//"(" ")" "*" "+" "-" "/" "NUMBER"
type ExprToken =
| NUMBER of float
| LPAREN
| RPAREN
| PLUS
| MINUS
| STAR
| DIV

let getTag = function
| NUMBER _ -> "NUMBER"
| LPAREN   -> "("
| RPAREN   -> ")"
| PLUS     -> "+"
| MINUS   -> "-"
| STAR -> "*"
| DIV    -> "/"

let getLexeme = function
| NUMBER n -> box n
| _   -> null

open FSharp.Idioms.StringOps

let tokenize(inp:string) =
    let rec loop (inp:string) =
        seq {
            match inp with
            | "" -> ()

            | Prefix @"\s+" (_, rest) ->
                yield! loop rest

            | Prefix @"\d+(\.\d+)?" (lexeme, rest) ->
                yield NUMBER <| System.Double.Parse(lexeme)
                yield! loop rest

            | PrefixChar '(' rest ->
                yield LPAREN
                yield! loop rest

            | PrefixChar ')' rest ->
                yield RPAREN
                yield! loop rest

            | PrefixChar '+' rest ->
                yield PLUS
                yield! loop rest

            | PrefixChar '-' rest ->
                yield MINUS
                yield! loop rest

            | PrefixChar '*' rest ->
                yield STAR
                yield! loop rest

            | PrefixChar '/' rest ->
                yield DIV
                yield! loop rest
            | never -> failwith never
        }

    loop inp
