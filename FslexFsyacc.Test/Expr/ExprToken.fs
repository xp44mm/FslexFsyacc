namespace FslexFsyacc.Expr

open FSharp.Idioms
open FSharp.Idioms.StringOps
open FSharp.Idioms.RegularExpressions
open FSharp.Idioms.ActivePatterns

open System
open System.Text.RegularExpressions

open FslexFsyacc.Runtime


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

    let tokenize offset (input:string) =
        let rec loop pos rest =
            seq {
                match rest with
                | "" -> ()

                | Rgx @"^\s+" m ->
                    yield! loop (pos + m.Length) rest.[m.Length..]

                | Rgx @"^\d+(\.\d+)?" m ->
                    let tok =
                        {
                            index = pos
                            length = m.Length
                            value = NUMBER(Double.Parse(m.Value))
                        }
                    yield tok
                    yield! loop tok.nextIndex rest.[m.Length..]

                | LongestPrefix (Map.keys ops) x ->
                    let tok =
                        {
                            index = pos
                            length = x.Length
                            value = ops.[x]
                        }
                    yield tok
                    yield! loop tok.nextIndex rest.[x.Length..]

                | never -> failwith never
            }
        loop offset input
