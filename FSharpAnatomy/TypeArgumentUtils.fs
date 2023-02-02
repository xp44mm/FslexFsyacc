module FSharpAnatomy.TypeArgumentUtils

open FSharpAnatomy.FSharpTokenScratch
open FslexFsyacc.Runtime
open FSharp.Idioms

open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.StringOps
open FSharp.Literals.Literal

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
    "_",UNDERSCORE;
    "struct",STRUCT;]

let ops_inverse = 
    ops 
    |> Map.inverse 
    |> Map.map(fun k v -> Seq.exactlyOne v)

let kws_inverse = 
    kws 
    |> Map.inverse 
    |> Map.map(fun k v -> Seq.exactlyOne v)

let rec tokenize index (inp:string) =
    seq {
        match inp with
        | "" -> () // yield {index=index;length=0;value=EOF}

        | On(tryMatch(Regex @"^\s+")) (x, rest) ->
            let len = x.Length
            let index = index + len
            yield! tokenize index rest

        | On tryIdent (x, rest) ->
            let tok =
                {
                    index = index
                    length = x.Length
                    value =
                        if kws.ContainsKey x then kws.[x]
                        else IDENT x
                }
            yield tok
            yield! tokenize tok.nextIndex rest

        | On tryQTypar (x, rest) ->
            let tok = {
                index = index
                length = x.Length
                value = QTYPAR x.[1..]
            }
            yield tok
            yield! tokenize tok.nextIndex rest

        | On tryHTypar (x, rest) ->
            let tok = {
                index = index
                length = x.Length
                value = HTYPAR x.[1..]
            }
            yield tok
            yield! tokenize tok.nextIndex rest

        | On(tryLongestPrefix (Map.keys ops)) (x, rest) ->
            let tok = {
                index = index
                length = x.Length
                value = ops.[x]
            }
            yield tok
            yield! tokenize tok.nextIndex rest
            
        | _ -> failwith $"unimpl tokenize case{stringify(index,inp)}"
    }


let getTag (token:Position<FSharpToken>) =
    match token.value with
    | x when ops_inverse.ContainsKey x -> ops_inverse.[x]
    | x when kws_inverse.ContainsKey x -> kws_inverse.[x]

    | IDENT _ -> "IDENT"
    | QTYPAR _ -> "QTYPAR"
    | HTYPAR _ -> "HTYPAR"
    | WHITESPACE _ -> "WHITESPACE"
    | COMMENT _ -> "COMMENT"
    | ARRAY_TYPE_SUFFIX _ -> "ARRAY_TYPE_SUFFIX"
    | x -> failwith $"{stringify x}"

let getLexeme (token:Position<FSharpToken>) =
    match token.value with
    | IDENT             x -> box x
    | QTYPAR            x -> box x
    | HTYPAR            x -> box x
    | WHITESPACE        x -> box x
    | COMMENT           x -> box x
    | ARRAY_TYPE_SUFFIX x -> box x
    | _ -> null
