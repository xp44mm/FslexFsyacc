module FslexFsyacc.Fsyacc.RegularSymbolUtils

/// the tag of token
let getTag(token) = FsyaccTokenUtils.getTag(0,0,token)

/// 获取token携带的语义信息§
let getLexeme(token) = FsyaccTokenUtils.getLexeme(0,0,token)

open FSharp.Idioms
open FslexFsyacc.FSharpSourceText

let tokenize inp =
    let rec loop (inp:string) =
        seq {
            match inp with
            | "" -> ()
            | On tryWS (_, rest) ->
                yield! loop rest

            | On tryWord (x, rest) ->
                yield ID x
                yield! loop rest

            | On trySingleQuoteString (x, rest) ->
                yield LITERAL(Quotation.unquote x)
                yield! loop rest

            | On (tryFirst '?') rest ->
                yield QMARK
                yield! loop rest

            | On (tryFirst '+') rest ->
                yield PLUS
                yield! loop rest

            | On (tryFirst '*') rest ->
                yield STAR
                yield! loop rest

            | On (tryFirst '[') rest ->
                yield LBRACK
                yield! loop rest

            | On (tryFirst ']') rest ->
                yield RBRACK
                yield! loop rest

            | On (tryFirst '(') rest ->
                yield LPAREN
                yield! loop rest

            | On (tryFirst ')') rest ->
                yield RPAREN
                yield! loop rest

            | never -> failwithf "%A" never
        }
    loop inp

