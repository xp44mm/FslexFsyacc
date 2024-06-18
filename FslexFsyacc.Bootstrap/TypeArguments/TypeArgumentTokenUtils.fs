module FslexFsyacc.TypeArguments.TypeArgumentTokenUtils

open FslexFsyacc.TypeArguments.FSharpTokenScratch
open FslexFsyacc.SourceText
open FslexFsyacc

open FSharp.Idioms

open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.StringOps
open FSharp.Idioms.Literal

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
    "<",LANGLE;
    ">",RANGLE;
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

let getTag (token:Position<TypeArgumentToken>) =
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

let getLexeme (token:Position<TypeArgumentToken>) =
    match token.value with
    | IDENT             x -> box x
    | QTYPAR            x -> box x
    | HTYPAR            x -> box x
    | WHITESPACE        x -> box x
    | COMMENT           x -> box x
    | ARRAY_TYPE_SUFFIX x -> box x
    | _ -> null

let tokenize offset (input:string) =
    let rec loop pos rest =
        seq {
            match rest with
            | "" -> ()

            | Rgx @"^\s+" m ->
                yield! loop (pos+m.Length) rest.[m.Length..]

            | Rgx @"^\[\s*(,\s*)*\]" m ->
                let rank = m.Groups.[1].Captures.Count+1
                let tok = {
                    index = pos
                    length = m.Length
                    value = ARRAY_TYPE_SUFFIX rank
                }
                yield tok
                yield! loop tok.nextIndex rest.[tok.length..]            

            | On tryFSharpIdent x ->
                let tok =
                    {
                        index = pos
                        length = x.Length
                        value =
                            if kws.ContainsKey x.Value
                            then kws.[x.Value]
                            else IDENT x.Value
                    }
                yield tok
                yield! loop tok.nextIndex rest.[tok.length..]

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

            | LongestPrefix (Map.keys ops) x ->
                let tok = {
                    index = pos
                    length = x.Length
                    value = ops.[x]
                }
                yield tok
                yield! loop tok.nextIndex rest.[tok.length..]

            | rest -> failwith $"unimpl tokenize case{stringify(pos,rest)}"
        }

    loop offset input
