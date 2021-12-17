module FslexFsyacc.Fsyacc.FsyaccTokenUtils

/// the tag of token
let getTag(pos,len,token) =
    match token with
    | HEADER _ -> "HEADER"
    | ID _ -> "ID"
    | QUOTE _    -> "QUOTE"
    | SEMANTIC _ -> "SEMANTIC"
    | COLON        -> ":"
    | SEMICOLON    -> ";"
    | BAR          -> "|"
    | PERCENT      -> "%%"
    | LEFT         -> "%left"
    | RIGHT        -> "%right"
    | NONASSOC     -> "%nonassoc"
    | PREC         -> "%prec"
    //| BOF          -> "BOF"
    //| EOF          -> "EOF"

/// 获取token携带的语义信息
let getLexeme(pos,len,token) =
    match token with
    | HEADER x -> box x
    | ID x -> box x
    | QUOTE x    -> box x
    | SEMANTIC x -> box x
    | _        -> null

open FSharp.Idioms
open FslexFsyacc.FSharpSourceText

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
                yield pos,len,QUOTE(unquote x)
                yield! loop (lpos,linp) (pos+len,rest)

            | Prefix "%%+" (x, rest) ->
                let len = x.Length
                yield pos,len,PERCENT
                yield! loop (lpos,linp) (pos+len,rest)

            | On (tryFirstChar ':') rest ->
                yield pos, 1, COLON
                yield! loop (lpos,linp) (pos+1,rest)

            | On (tryFirstChar '|') rest ->
                yield pos,1,BAR
                yield! loop (lpos,linp) (pos+1,rest)

            | On (tryFirstChar ';') rest ->
                yield pos,1,SEMICOLON
                yield! loop (lpos,linp) (pos+1,rest)

            | Prefix @"%[a-z]+" (x, rest) ->
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
                let col,nlpos,nlinp = getColumnAndRest (lpos,linp) (pos+1)
                let fcode = formatNestedCode col code

                yield pos, len, SEMANTIC fcode
                yield! loop (nlpos,nlinp) (pos+len,rest)

            | On tryHeader (x, rest) ->
                let len = x.Length
                let code = x.[2..len-3]
                let col,nlpos,nlinp = getColumnAndRest (lpos,linp) (pos+2)
                let fcode = formatNestedCode col code

                yield pos,len,HEADER fcode
                yield! loop (nlpos,nlinp) (pos+len,rest)

            | never -> failwithf "%A" never
        }
    loop (0,inp) (0,inp)
