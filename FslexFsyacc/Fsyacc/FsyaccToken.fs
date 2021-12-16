namespace FslexFsyacc.Fsyacc

type FsyaccToken =
    | HEADER of string
    | IDENTIFIER of string
    | QUOTE of string
    | SEMANTIC of string
    | COLON
    | SEMICOLON
    | BAR
    | PERCENT
    | LEFT
    | RIGHT
    | NONASSOC
    | PREC
    | BOF
    | EOF

//let tokenize inp =
//    let rec loop (pos:int) (inp: string) =
//        seq {
//            match inp with
//            | "" -> ()
//            | On tryWhiteSpace (x, rest) ->
//                let len = x.Length
//                yield! loop (pos+len) rest
//            | On tryLineTerminator (x, rest) ->
//                let len = x.Length
//                yield! loop (pos+len) rest

//            | On trySingleLineComment (x, rest) ->
//                let len = x.Length
//                yield! loop (pos+len) rest

//            | On tryMultiLineComment (x, rest) ->
//                let len = x.Length
//                yield! loop (pos+len) rest

//            | On tryWord (x, rest) ->
//                let len = x.Length
//                yield pos, len, IDENTIFIER x
//                yield! loop (pos+len) rest

//            | On trySingleQuoteString (x, rest) ->
//                let len = x.Length
//                yield pos,len,QUOTE(unquote x)
//                yield! loop (pos+len) rest

//            | On (tryFirstChar ':') rest ->
//                yield pos, 1, COLON
//                yield! loop (pos+1) rest

//            | On (tryFirstChar '|') rest ->
//                yield pos,1,BAR
//                yield! loop (pos+1) rest

//            | On (tryFirstChar ';') rest ->
//                yield pos,1,SEMICOLON
//                yield! loop (pos+1) rest

//            | Prefix "%%+" (x, rest) ->
//                let len = x.Length
//                yield pos,len,PERCENT
//                yield! loop (pos+len) rest

//            | Prefix "%[a-z]+" (x, rest) ->
//                let tok =
//                    match x with
//                    | "%left" -> LEFT
//                    | "%right" -> RIGHT
//                    | "%nonassoc" -> NONASSOC
//                    | "%prec" -> PREC
//                    | never -> failwith ""
//                let len = x.Length
//                yield pos,len,tok
//                yield! loop (pos+len) rest

//            | On trySemantic (x, rest) ->
//                let len = x.Length
//                let code = x.[1..len-2]
//                yield pos, len, SEMANTIC(code.Trim())
//                yield! loop (pos+len) rest

//            | On tryHeader (x, rest) ->
//                let len = x.Length
//                let code = x.[2..len-3]
//                yield pos,len,HEADER(code.Trim())
//                yield! loop (pos+len) rest

//            | never -> failwithf "%A" never
//        }
//    loop 0 inp

