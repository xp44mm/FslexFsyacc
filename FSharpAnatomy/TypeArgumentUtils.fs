module FSharpAnatomy.TypeArgumentUtils

open FSharpAnatomy.FSharpTokenScratch
open FslexFsyacc.Runtime
open FSharp.Idioms

open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.StringOps
open FSharp.Literals.Literal

open System
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

let tokenize index (inp:string) =
    let rec loop i =
        seq {
            match inp.[index+i..] with
            | "" -> ()

            | Rgx @"^\s+" m ->
                yield! loop (i+m.Length)

            | On tryIdent (x, _) ->
                let tok =
                    {
                        index = i
                        length = x.Length
                        value =
                            if kws.ContainsKey x 
                            then kws.[x]
                            else IDENT x
                    }
                yield tok
                yield! loop tok.nextIndex

            | On tryQTypar (x, _) ->
                let tok = {
                    index = i
                    length = x.Length
                    value = QTYPAR x.[1..]
                }
                yield tok
                yield! loop tok.nextIndex

            | On tryHTypar (x, _) ->
                let tok = {
                    index = i
                    length = x.Length
                    value = HTYPAR x.[1..]
                }
                yield tok
                yield! loop tok.nextIndex

            | LongestPrefix (Map.keys ops) (x, _) ->
                let tok = {
                    index = i
                    length = x.Length
                    value = ops.[x]
                }
                yield tok
                yield! loop tok.nextIndex
            
            | rest -> failwith $"unimpl tokenize case{stringify(i,rest)}"
        }

    loop index
