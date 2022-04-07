module FslexFsyacc.Fslex.FslexTokenUtils
open FSharp.Idioms
open System.Text.RegularExpressions
open FslexFsyacc.FSharpSourceText

let getTag(pos,len,token) = 
    match token with
    | HEADER   _ -> "HEADER"
    | ID       _ -> "ID"
    | CAP      _ -> "CAP"
    | QUOTE    _ -> "QUOTE"
    | SEMANTIC _ -> "SEMANTIC"
    | HOLE     _ -> "HOLE"
    | EQUALS    -> "="
    | LPAREN    -> "("
    | RPAREN    -> ")"
    | LBRACK    -> "["
    | RBRACK    -> "]"
    | PLUS      -> "+"
    | STAR      -> "*"
    | SLASH     -> "/"
    | BAR       -> "|"
    | QMARK     -> "?"
    | AMP       -> "&"
    | PERCENT   -> "%%"

let getLexeme (pos,len,token) = 
    match token with
    | HEADER x -> box x
    | ID x -> box x
    | CAP x -> box x
    | QUOTE x -> box x
    | SEMANTIC x -> box x
    | HOLE x -> box x
    | _ -> null

let tryHole =
    Regex @"^\<\w+\>"
    |> tryMatch

let tokenize inp =
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
                if Regex.IsMatch(rest,@"^\s*=") then
                    yield pos, len, CAP x
                else
                    yield pos, len, ID x
                yield! loop (lpos,linp) (pos+len,rest)

            | On trySingleQuoteString (x, rest) ->
                let len = x.Length
                yield pos,len,QUOTE(Quotation.unquote x)
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryMatch(Regex @"^%%+")) (x, rest) ->
                let len = x.Length
                yield pos,len,PERCENT
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryFirst '=') rest ->
                let len = 1
                yield pos,len,EQUALS
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryFirst '(') rest ->
                let len = 1
                yield pos,len,LPAREN
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryFirst ')') rest ->
                let len = 1
                yield pos,len,RPAREN
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryFirst '[') rest ->
                let len = 1
                yield pos,len,LBRACK
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryFirst ']') rest ->
                let len = 1
                yield pos,len,RBRACK
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryFirst '+') rest ->
                let len = 1
                yield pos,len,PLUS
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryFirst '*') rest ->
                let len = 1
                yield pos,len,STAR
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryFirst '/') rest ->
                let len = 1
                yield pos,len,SLASH
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryFirst '|') rest ->
                let len = 1
                yield pos,len,BAR
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryFirst '?') rest ->
                let len = 1
                yield pos,len,QMARK
                yield! loop (lpos,linp) (pos+len,rest)

            | On tryHole (x, rest) ->
                let len = x.Length
                yield pos,len,HOLE x.[1..len-2]
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

                yield pos, len, SEMANTIC fcode
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

                yield pos,len,HEADER fcode
                yield! loop (nlpos,nlinp) (pos+len,rest)

            | never -> failwith never
        }
    
    loop (0,inp) (0,inp)

let appendAMP (lexbuf:(int*int*_)list) =
    let last = 
        lexbuf
        |> List.exactlyOne
    let pos,len,_ = last
    [last;pos + len,0,AMP]