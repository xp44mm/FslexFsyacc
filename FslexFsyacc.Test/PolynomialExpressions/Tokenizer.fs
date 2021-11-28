module PolynomialExpressions.Tokenizer

type Token =
    | ID of string
    | INT of int
    | HAT
    | PLUS
    | MINUS

let getTag = function
    | ID _ -> "ID"
    | INT _ -> "INT"
    | HAT   -> "**"
    | PLUS  -> "+"
    | MINUS -> "-"

let getLexeme = function
    | ID x -> box x
    | INT x -> box x
    | _ -> null


let regex s = new System.Text.RegularExpressions.Regex(s)

let tokenR = regex @"((?<token>(\d+|\w+|\*\*|\+|-))\s*)*"

let tokenize (s : string) =
    [for x in tokenR.Match(s).Groups.["token"].Captures do
        let token =
            match x.Value with
            | "**" -> HAT
            | "-" -> MINUS
            | "+" -> PLUS
            | s when System.Char.IsDigit s.[0] -> INT (int s)
            | s -> ID s
        yield token]

open PolynomialExpressions

let toConst = function
    | [      INT n] 
    | [PLUS ;INT n] ->  Const n
    | [MINUS;INT n] ->  Const -n
    | tokens -> failwithf "%A" tokens
    
let toTerm = function
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
