namespace FslexFsyacc.Expr

open FSharp.Idioms
open FSharp.Idioms.StringOps
open FSharp.Idioms.RegularExpressions
open FSharp.Idioms.ActivePatterns

open System
open System.Text.RegularExpressions

open FslexFsyacc


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

    let getTag (token:PositionWith<ExprToken>) = 
        match token.value with
        | x when ops_inverse.ContainsKey x -> ops_inverse.[x]
        | NUMBER _ -> "NUMBER"
        | EOF -> ""
        | _ -> null

    let getLexeme (token:PositionWith<ExprToken>) = 
        match token.value with
        | NUMBER n -> box n
        | _   -> null

    let tokenize (sourceText:SourceText) = // offset (input:string) =
        let rec loop (src:SourceText) =
            seq {
                match src.text with
                | "" -> ()

                | Rgx @"^\s+" m ->
                    let src = src.skip(m.Length)
                    yield! loop src

                | Rgx @"^\d+(\.\d+)?" m ->
                    let tok =
                        {
                            index = src.index
                            length = m.Length
                            value = NUMBER(Double.Parse(m.Value))
                        }
                    yield tok
                    yield! loop <| src.skip(tok.length)

                | LongestPrefix (Map.keys ops) x ->
                    let tok =
                        {
                            index = src.index
                            length = x.Length
                            value = ops.[x]
                        }
                    yield tok
                    yield! loop <| src.skip(tok.length) // tok.nextIndex rest.[x.Length..]

                | never -> failwith never
            }
        //SourceText.just(offset, input)
        loop sourceText
