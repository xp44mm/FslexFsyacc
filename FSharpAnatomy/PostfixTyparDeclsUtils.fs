module FSharpAnatomy.PostfixTyparDeclsUtils

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

let rec tokenize index (inp:string) =
    seq {
        match inp with
        | "" -> yield {index=index;length=0;value=EOF}
        | On(tryMatch(Regex @"^\s+")) (x, rest) ->
            let len = x.Length
            let index = index + len
            yield! tokenize index rest
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
        | On tryIdent (x, rest) ->
            let tok =
                {
                    index = index
                    length = x.Length
                    value =
                        if kws.ContainsKey x then
                            kws.[x]
                        else IDENT x
                }
            yield tok
            yield! tokenize tok.nextIndex rest

        | On(tryStart "(*)") rest ->
            let tok = {
                index = index
                length = 3
                value = OPERATOR_NAME "*"
            }
            yield tok
            yield! tokenize tok.nextIndex rest
        | On tryOperatorName (x,rest) ->
            let op = x.[1..x.Length-2].Trim()
            let tok = {
                index = index
                length = x.Length
                value = OPERATOR_NAME op
            }
            yield tok
            yield! tokenize tok.nextIndex rest

        | On(tryLongestPrefix (Map.keys ops)) (x,rest) ->
            let tok = {
                index = index
                length = x.Length
                value = ops.[x]
            }
            yield tok
            yield! tokenize tok.nextIndex rest

        | _ -> failwith "unimpl tokenize case"
    }


let getTag (token:Position<FSharpToken>) =
    match token.value with
    // ops
    | HASH -> "#"
    | LPAREN -> "("
    | RPAREN -> ")"
    | STAR -> "*"
    | COMMA -> ","
    | RARROW -> "->"
    | DOT -> "."
    | COLON -> ":"
    | COLON_GREATER -> ":>"
    | SEMICOLON -> ";"
    | LESS -> "<"
    | GREATER -> ">"
    | LBRACK -> "["
    | RBRACK -> "]"
    | LBRACE_BAR -> "{|"
    | BAR_RBRACE -> "|}"
    // kws
    | UNDERSCORE -> "_"
    | AND -> "and"
    | COMPARISON -> "comparison"
    | DELEGATE -> "delegate"
    | ENUM -> "enum"
    | EQUALITY -> "equality"
    | MEMBER -> "member"
    | NEW -> "new"
    | NOT -> "not"
    | NULL -> "null"
    | OR -> "or"
    | STATIC -> "static"
    | STRUCT -> "struct"
    | UNMANAGED -> "unmanaged"
    | WHEN -> "when"
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
    //| x -> failwith $"{stringify x}"

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

//let ops = PostfixTyparDeclsUtils.ops
//let kws = PostfixTyparDeclsUtils.kws

//let s =
//    let ops =
//        ops
//        |> Map.toList
//        |> List.map(fun(a,b)-> $"| {stringify b} -> {stringify a}")

//    let kws =
//        kws
//        |> Map.toList
//        |> List.map(fun(a,b)-> $"| {stringify b} -> {stringify a}")

//    [
//        "// ops"
//        yield! ops
//        "// kws"
//        yield! kws
//    ] |> String.concat "\r\n"
