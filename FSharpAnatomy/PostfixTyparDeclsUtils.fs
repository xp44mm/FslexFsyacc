module FSharpAnatomy.PostfixTyparDeclsUtils

open FSharpAnatomy.FSharpTokenScratch
open FslexFsyacc.Runtime

open FSharp.Idioms

open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.StringOps
open FSharp.Idioms.Literal

open System.Text.RegularExpressions

let ops = Map [
    "#",HASH;
    "(",LPAREN;
    ")",RPAREN;
    "*",STAR;
    ",",COMMA;
    "->",RARROW;
    ".",DOT;
    ":",COLON;
    ":>",COLON_GREATER;
    ";",SEMICOLON;
    "<",LESS;
    ">",GREATER;
    "[",LBRACK
    "]",RBRACK
    "{|",LBRACE_BAR;
    "|}",BAR_RBRACE
]

let kws = Map [
     "_"         , UNDERSCORE
     "and"       , AND
     "comparison", COMPARISON
     "delegate"  , DELEGATE
     "enum"      , ENUM
     "equality"  , EQUALITY
     "member"    , MEMBER
     "new"       , NEW
     "not"       , NOT
     "null"      , NULL
     "or"        , OR
     "static"    , STATIC
     "struct"    , STRUCT
     "unmanaged" , UNMANAGED
     "when" , WHEN
]
let ops_inverse = 
    ops 
    |> Map.inverse 
    |> Map.map(fun k v -> Seq.exactlyOne v)

let kws_inverse = 
    kws 
    |> Map.inverse 
    |> Map.map(fun k v -> Seq.exactlyOne v)

let getTag (token:Position<FSharpToken>) =
    match token.value with
    | x when ops_inverse.ContainsKey x -> ops_inverse.[x]
    | x when kws_inverse.ContainsKey x -> kws_inverse.[x]

    //with params
    | WHITESPACE _ -> "WHITESPACE"
    | COMMENT _ -> "COMMENT"
    | IDENT _ -> "IDENT"
    | HTYPAR _ -> "HTYPAR"
    | QTYPAR _ -> "QTYPAR"
    | OPERATOR_NAME _ -> "OPERATOR_NAME"
    | ARRAY_TYPE_SUFFIX _ -> "ARRAY_TYPE_SUFFIX"
    | TYPE_ARGUMENT _ ->"TYPE_ARGUMENT"
    | EOF -> "EOF"
    | x -> failwith $"getTag:{stringify x}"

let getLexeme (token:Position<FSharpToken>) =
    match token.value with

    | WHITESPACE x -> box x 
    | COMMENT x -> box x 

    | IDENT x -> box x 
    | HTYPAR x -> box x 
    | QTYPAR x -> box x 
    | OPERATOR_NAME x -> box x
    | TYPE_ARGUMENT x -> box x
    | _ -> null

let tokenize offset (input:string) =
    let rec loop pos rest =
        seq {
            match rest with // input.[offset+pos..] 
            | "" -> ()
            | Rgx @"^\s+" m ->
                yield! loop (pos+m.Length) rest.[m.Length..]
            | On tryQTypar x ->
                let tok = {
                    index = pos
                    length = x.Length
                    value = QTYPAR x.Value.[1..]
                }
                yield tok
                yield! loop tok.nextIndex rest.[tok.length..]
            | On tryHTypar x ->
                let tok = {
                    index = pos
                    length = x.Length
                    value = HTYPAR x.Value.[1..]
                }
                yield tok
                yield! loop tok.nextIndex rest.[tok.length..]
            | On tryIdent x ->
                let tok =
                    {
                        index = pos
                        length = x.Length
                        value =
                            if kws.ContainsKey x.Value then
                                kws.[x.Value]
                            else IDENT x.Value
                    }
                yield tok
                yield! loop tok.nextIndex rest.[tok.length..]

            | StartsWith "(*)" rest ->
                let tok = {
                    index = pos
                    length = 3
                    value = OPERATOR_NAME "*"
                }
                yield tok
                yield! loop tok.nextIndex rest.[tok.length..]
            | On tryOperatorName x ->
                let op = x.Value.[1..x.Length-2].Trim()
                let tok = {
                    index = pos
                    length = x.Length
                    value = OPERATOR_NAME op
                }
                yield tok
                yield! loop tok.nextIndex rest.[tok.length..]

            | LongestPrefix (Map.keys ops) x ->
                let tok = {
                    index = pos
                    length = x.Length
                    value = ops.[x]
                }
                yield tok
                yield! loop tok.nextIndex rest.[tok.length..]

            | _ -> failwith "unimpl tokenize case"
        }
    loop offset input

