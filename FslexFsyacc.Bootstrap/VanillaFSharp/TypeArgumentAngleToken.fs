namespace FslexFsyacc.VanillaFSharp

type TypeArgumentAngleToken =
    | HASH
    | LPAREN 
    | RPAREN
    | STAR
    | COMMA
    | RARROW
    | DOT
    | COLON
    | COLON_GREATER
    | SEMICOLON
    | ARRAY_TYPE_SUFFIX of rank:int
    | HTYPAR of string
    | IDENT of string
    | QTYPAR of string
    | UNDERSCORE
    | STRUCT
    | LBRACE_BAR 
    | BAR_RBRACE
    | LANGLE
    | RANGLE
    | WHITESPACE of string
    | COMMENT of string

open FslexFsyacc.VanillaFSharp.FSharpSourceText
open FslexFsyacc
open FSharp.Idioms

open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.StringOps
open FSharp.Idioms.Literal

open System
open System.Text.RegularExpressions

module TypeArgumentAngleToken =
    let ops = Map [
        "#" ,HASH
        "(" ,LPAREN
        ")" ,RPAREN
        "*" ,STAR
        "," ,COMMA
        "->",RARROW
        "." ,DOT
        ":" ,COLON
        ":>",COLON_GREATER
        ";" ,SEMICOLON
        "<" ,LANGLE
        ">" ,RANGLE
        "{|",LBRACE_BAR
        "|}",BAR_RBRACE
        ]

    let kws = Map [
        "_"     ,UNDERSCORE
        "struct",STRUCT
        ]

    let ops_inverse = 
        ops 
        |> Map.inverse 
        |> Map.map(fun k v -> Seq.exactlyOne v)

    let kws_inverse = 
        kws 
        |> Map.inverse 
        |> Map.map(fun k v -> Seq.exactlyOne v)

    let render (token:TypeArgumentAngleToken) =
        match token with
        | x when ops_inverse.ContainsKey x -> ops_inverse.[x]
        | x when kws_inverse.ContainsKey x -> kws_inverse.[x]
        | ARRAY_TYPE_SUFFIX rank -> $"[{String.replicate (rank-1) (string ',') }]"
        | HTYPAR id -> $"^{id}"
        | QTYPAR id -> $"'{id}"
        | IDENT id -> id
        | WHITESPACE sp -> stringify sp
        | COMMENT ct -> stringify ct
        | _ -> failwith ""

    let getTag (token:PositionWith<TypeArgumentAngleToken>) =
        match token.value with
        | LANGLE -> "LEFT"
        | RANGLE -> "RIGHT"
        | _ -> "TICK"

    let getLexeme (token:PositionWith<TypeArgumentAngleToken>) =
        match token.value with
        | LANGLE -> box token.index
        | RANGLE -> box token.index
        | tok -> box (render tok)

    let tokenize (sourceText:SourceText) = // offset (input:string) =
        //无限循环
        let rec loop (src:SourceText) = // pos rest =
            seq {
                match src.text with
                | Rgx @"^\s+" m ->
                    let tok =
                        {
                            index = src.index
                            length = m.Length
                            value = WHITESPACE m.Value
                        }
                    yield tok
                    let src = src.skip tok.length
                    yield! loop src

                | Rgx @"^//[^\r\n]*" m ->
                    let tok =
                        {
                            index = src.index
                            length = m.Length
                            value = COMMENT m.Value
                        }
                    yield tok
                    let src = src.skip tok.length
                    yield! loop src

                | Rgx @"^\(\*[\s\S]+\*\)" m ->
                    let tok =
                        {
                            index = src.index
                            length = m.Length
                            value = COMMENT m.Value
                        }
                    yield tok
                    let src = src.skip tok.length
                    yield! loop src

                | On tryIdent x ->
                    let tok =
                        {
                            index = src.index
                            length = x.Length
                            value =
                                if kws.ContainsKey x.Value
                                then kws.[x.Value]
                                else IDENT x.Value
                        }
                    yield tok
                    let src = src.skip tok.length
                    yield! loop src

                | Rgx @"^'(\w+)(?!')" m ->
                    let tok = {
                        index = src.index
                        length = m.Length
                        value = QTYPAR m.Groups.[1].Value
                    }
                    yield tok
                    let src = src.skip tok.length
                    yield! loop src

                | Rgx @"^\^(\w+)" m ->
                    let tok = {
                        index = src.index
                        length = m.Length
                        value = HTYPAR m.Groups.[1].Value
                    }
                    yield tok
                    let src = src.skip tok.length
                    yield! loop src

                | Rgx @"^\[\s*(,\s*)*\]" m ->
                    let tok = {
                        index = src.index
                        length = m.Length
                        value = 
                            let rank = m.Groups.[1].Captures.Count-1
                            ARRAY_TYPE_SUFFIX rank
                    }
                    yield tok
                    let src = src.skip tok.length
                    yield! loop src

                | LongestPrefix (Map.keys ops) capt ->
                    let tok = {
                        index = src.index
                        length = capt.Length
                        value = ops.[capt]
                    }
                    yield tok
                    let src = src.skip tok.length
                    yield! loop src
            
                | _ -> failwith $"unimpl case: {stringify src}"
            }
        loop sourceText



