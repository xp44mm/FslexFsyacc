module PolynomialExpressions.Tokenizer
open PolynomialExpressions

type Token =
    | ID of string
    | INT of int
    | HAT
    | PLUS
    | MINUS

let getTag(pos,len,token) = 
    match token with
    | ID _ -> "ID"
    | INT _ -> "INT"
    | HAT   -> "**"
    | PLUS  -> "+"
    | MINUS -> "-"

open System
open System.Text.RegularExpressions
open FSharp.Idioms

let regex s = new Regex(s)

let tokenR = regex @"((?<token>(\d+|\w+|\*\*|\+|-))\s*)*"

let tokenize (s: string) =
    seq {
        for x in tokenR.Match(s).Groups.["token"].Captures do
        let token =
            match x.Value with
            | "**" -> HAT
            | "-" -> MINUS
            | "+" -> PLUS
            | s when Char.IsDigit s.[0] -> INT (int s)
            | s -> ID s
        yield x.Index, x.Length, token
    }

let toConst (lexbuf:(int*int*Token)list) = 
    match lexbuf |> List.map Triple.last with
    | [      INT n] 
    | [PLUS ;INT n] ->  Const n
    | [MINUS;INT n] ->  Const -n
    | tokens -> failwithf "%A" tokens
    
let toTerm (lexbuf:(int*int*Token)list) = 
    match lexbuf |> List.map Triple.last with
    |[      ID x] -> Term(1,x,1)
    |[PLUS ;ID x] -> Term(1,x,1)
    |[MINUS;ID x] -> Term(-1,x,1)
    |[      ID x;HAT;INT i] -> Term(1,x,i)
    |[PLUS ;ID x;HAT;INT i] -> Term(1,x,i)
    |[MINUS;ID x;HAT;INT i] -> Term(-1,x,i)
    
    |[      INT n;ID x] -> Term(n,x,1)
    |[PLUS ;INT n;ID x] -> Term(n,x,1)
    |[MINUS;INT n;ID x] -> Term(-n,x,1)
    |[      INT n;ID x;HAT;INT i] -> Term(n,x,i)
    |[PLUS ;INT n;ID x;HAT;INT i] -> Term(n,x,i)
    |[MINUS;INT n;ID x;HAT;INT i] -> Term(-n,x,i)
    
    | tokens -> failwithf "%A" tokens
