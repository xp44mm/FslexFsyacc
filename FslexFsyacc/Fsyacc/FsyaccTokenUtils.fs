module FslexFsyacc.Fsyacc.FsyaccTokenUtils

/// the tag of token
let getTag(pos,len,token) =
    match token with
    | HEADER _ -> "HEADER"
    | ID _ -> "ID"
    | LITERAL _    -> "LITERAL"
    | SEMANTIC _ -> "SEMANTIC"
    | COLON        -> ":"
    | SEMICOLON    -> ";"
    | BAR          -> "|"
    | PERCENT      -> "%%"
    | LEFT         -> "%left"
    | RIGHT        -> "%right"
    | NONASSOC     -> "%nonassoc"
    | PREC         -> "%prec"

/// 获取token携带的语义信息
let getLexeme(pos,len,token) =
    match token with
    | HEADER x -> box x
    | ID x -> box x
    | LITERAL x    -> box x
    | SEMANTIC x -> box x
    | _        -> null

open FSharp.Idioms
open FslexFsyacc.FSharpSourceText
open System.Text.RegularExpressions

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
                yield pos, len, ID x
                yield! loop (lpos,linp) (pos+len,rest)

            | On trySingleQuoteString (x, rest) ->
                let len = x.Length
                yield pos,len,LITERAL(Quotation.unquote x)
                yield! loop (lpos,linp) (pos+len,rest)

            | On(tryMatch(Regex @"^%%+")) (x, rest) ->
                let len = x.Length
                yield pos,len,PERCENT
                yield! loop (lpos,linp) (pos+len,rest)

            | On (tryFirst ':') rest ->
                yield pos, 1, COLON
                yield! loop (lpos,linp) (pos+1,rest)

            | On (tryFirst '|') rest ->
                yield pos,1,BAR
                yield! loop (lpos,linp) (pos+1,rest)

            | On (tryFirst ';') rest ->
                yield pos,1,SEMICOLON
                yield! loop (lpos,linp) (pos+1,rest)

            | On(tryMatch(Regex @"^%[a-z]+")) (x, rest) ->
                let tok =
                    match x with
                    | "%left" -> LEFT
                    | "%right" -> RIGHT
                    | "%nonassoc" -> NONASSOC
                    | "%prec" -> PREC
                    | never -> failwith ""
                let len = x.Length
                yield pos,len,tok
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

            | never -> failwithf "%A" never
        }
    loop (0,inp) (0,inp)
