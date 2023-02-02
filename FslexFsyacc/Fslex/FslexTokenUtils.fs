module FslexFsyacc.Fslex.FslexTokenUtils

open FSharp.Idioms
open System.Text.RegularExpressions
open FslexFsyacc.FSharpSourceText
open FslexFsyacc.Runtime

let ops = Map [
    "%%",PERCENT;
    "&",AMP;
    "(",LPAREN;
    ")",RPAREN;
    "*",STAR;
    "+",PLUS;
    "/",SLASH;
    "=",EQUALS;
    "?",QMARK;
    "[",LBRACK;
    "]",RBRACK;
    "|",BAR
    ]
let ops_inverse = 
    ops 
    |> Map.inverse 
    |> Map.map(fun k v -> Seq.exactlyOne v)

let getTag (token:_ Position) = 
    match token.value with
    | x when ops_inverse.ContainsKey x -> ops_inverse.[x]

    | HEADER   _ -> "HEADER"
    | ID       _ -> "ID"
    | CAP      _ -> "CAP"
    | LITERAL  _ -> "LITERAL"
    | SEMANTIC _ -> "SEMANTIC"
    | HOLE     _ -> "HOLE"
    | _ -> failwith "getTag Wild"

let getLexeme (token:_ Position) = 
    match token.value with
    | HEADER   x -> box x
    | ID       x -> box x
    | CAP      x -> box x
    | LITERAL  x -> box x
    | SEMANTIC x -> box x
    | HOLE     x -> box x
    | _ -> null

let tryHole =
    Regex @"^\<\w+\>"
    |> tryMatch

let tokenize index inp =
    let rec loop (lpos:int,linp:string)(pos:int,inp:string) =
        seq {
            match inp with
            | "" -> ()
            | On tryWS (x, rest) ->
                let len = x.Length
                yield! loop (lpos,linp) (pos+len,rest)

            | On trySingleLineComment (x, rest) ->
                let len = x.Length
                yield! loop (lpos,linp) (pos+len,rest)

            | On tryMultiLineComment (x, rest) ->
                let len = x.Length
                yield! loop (lpos,linp) (pos+len,rest)

            | On tryWord (x, rest) ->
                let len = x.Length
                let v = if Regex.IsMatch(rest,@"^\s*=") then CAP x else ID x
                yield Position<_>.from(pos, len, v)
                yield! loop (lpos,linp) (pos+len,rest)

            | On trySingleQuoteString (x, rest) ->
                let len = x.Length
                yield Position<_>.from(pos,len,LITERAL(Quotation.unquote x))
                yield! loop (lpos,linp) (pos+len,rest)

            | On tryHole (x, rest) ->
                let len = x.Length
                yield Position<_>.from(pos,len,HOLE x.[1..len-2])
                yield! loop (lpos,linp) (pos+len,rest)

            | On trySemantic (x, rest) ->
                let len = x.Length
                let code = x.[1..len-2]

                let nlpos,nlinp,fcode =
                    if System.String.IsNullOrWhiteSpace(code) then
                        lpos,linp,""
                    else
                        let col,nlpos,nlinp = getColumnAndRest (lpos,linp) (pos+1)
                        let fcode = formatNestedCode col code
                        nlpos,nlinp,fcode

                yield Position<_>.from(pos, len, SEMANTIC fcode)
                yield! loop (nlpos,nlinp) (pos+len,rest)

            | On tryHeader (x, rest) ->
                let len = x.Length
                let code = x.[2..len-3]

                let nlpos,nlinp,fcode =
                    if System.String.IsNullOrWhiteSpace(code) then
                        lpos,linp,""
                    else
                        let col,nlpos,nlinp = getColumnAndRest (lpos,linp) (pos+2)
                        let fcode = formatNestedCode col code
                        nlpos,nlinp,fcode

                yield Position<_>.from(pos,len,HEADER fcode)
                yield! loop (nlpos,nlinp) (pos+len,rest)
            
            | On(tryMatch(Regex @"^%%+")) (x, rest) ->
                let len = x.Length
                yield Position<_>.from(pos,len,PERCENT)
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryLongestPrefix (Map.keys ops)) (x, rest) ->
                let len = x.Length
                yield Position<_>.from(pos,len,ops.[x])
                let nextPos = pos+len
                yield! loop (lpos,linp) (nextPos,rest)
            
            | _ -> failwith $""
        }
    
    loop (index,inp) (index,inp)

let appendAMP (lexbuf:FslexToken Position list) =
    let last = 
        lexbuf
        |> List.exactlyOne
    [last;Position<_>.from(last.nextIndex,0,AMP)]
