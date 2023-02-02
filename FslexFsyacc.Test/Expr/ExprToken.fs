namespace Expr

//"(" ")" "*" "+" "-" "/" "NUMBER"
type ExprToken =
    | NUMBER of float
    | LPAREN
    | RPAREN
    | PLUS
    | MINUS
    | STAR
    | DIV
    | EOF
open System
open System.Text.RegularExpressions

open FslexFsyacc.Runtime
open FSharp.Idioms


module ExprToken =
    let ops = Map [
           "(", LPAREN
           ")", RPAREN
           "+", PLUS  
           "-", MINUS 
           "*", STAR  
           "/", DIV   
    ]

    let ops_inverse = 
        ops 
        |> Map.inverse 
        |> Map.map(fun k v -> Seq.exactlyOne v)

    let getTag (token:Position<ExprToken>) = 
        match token.value with
        | x when ops_inverse.ContainsKey x -> ops_inverse.[x]
        | NUMBER _ -> "NUMBER"
        | EOF -> ""
        | _ -> null

    let getLexeme (token:Position<ExprToken>) = 
        match token.value with
        | NUMBER n -> box n
        | _   -> null

    let rec tokenize pos (inp:string) =
        seq {
            match inp with
            | "" -> () // yield {index=pos;length=0;value=EOF}

            | On(tryMatch(Regex @"^\s+")) (x, rest) ->
                let len = x.Length
                let pos = pos + len
                yield! tokenize pos rest

            | On(tryMatch(Regex @"^\d+(\.\d+)?")) (x, rest) ->
                let tok =
                    {
                        index = pos
                        length = x.Length
                        value = NUMBER(Double.Parse(x))
                    }
                yield tok
                yield! tokenize tok.nextIndex rest

            | On(tryLongestPrefix (Map.keys ops)) (x, rest) ->
                let tok =
                    {
                        index = pos
                        length = x.Length
                        value = ops.[x]
                    }
                yield tok
                yield! tokenize tok.nextIndex rest

            | never -> failwith never
        }

