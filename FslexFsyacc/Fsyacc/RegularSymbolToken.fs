namespace FslexFsyacc.Fsyacc

type RegularSymbolToken =
    | ID of string
    | LITERAL of string

    | QMARK
    | PLUS
    | STAR

    | LBRACE | RBRACE
    | LBRACK | RBRACK
    | LPAREN | RPAREN

module RegularSymbolToken =
    /// the tag of token
    let getTag(pos,len,token) =
        match token with
        | ID      _ -> "ID"
        | LITERAL _ -> "LITERAL"
        | QMARK -> "?"
        | PLUS  -> "+"
        | STAR  -> "*"
        | LBRACE -> "{"
        | RBRACE -> "}"
        | LBRACK -> "["
        | RBRACK -> "]"
        | LPAREN -> "("
        | RPAREN -> ")"

    /// 获取token携带的语义信息§
    let getLexeme(pos,len,token) =
        match token with
        | ID      x -> box x
        | LITERAL x -> box x
        | _ -> null

    open FSharp.Idioms
    open FslexFsyacc.FSharpSourceText

    let tokenize inp =
        let rec loop (pos:int)(inp:string) =
            seq {
                match inp with
                | "" -> ()
                | On tryWS (x, rest) ->
                    let len = x.Length
                    yield! loop (pos+len) rest

                | On tryWord (x, rest) ->
                    let len = x.Length
                    yield pos, len, ID x
                    yield! loop (pos+len) rest

                | On trySingleQuoteString (x, rest) ->
                    let len = x.Length
                    yield pos,len,LITERAL(Quotation.unquote x)
                    yield! loop (pos+len) rest

                | On (tryFirst '?') rest ->
                    yield pos,1,QMARK
                    yield! loop (pos+1) rest

                | On (tryFirst '+') rest ->
                    yield pos,1,PLUS
                    yield! loop (pos+1) rest

                | On (tryFirst '*') rest ->
                    yield pos,1,STAR
                    yield! loop (pos+1) rest

                | On (tryFirst '{') rest ->
                    yield pos,1,LBRACE
                    yield! loop (pos+1) rest

                | On (tryFirst '}') rest ->
                    yield pos,1,RBRACE
                    yield! loop (pos+1) rest

                | On (tryFirst '[') rest ->
                    yield pos,1,LBRACK
                    yield! loop (pos+1) rest

                | On (tryFirst ']') rest ->
                    yield pos,1,RBRACK
                    yield! loop (pos+1) rest

                | On (tryFirst '(') rest ->
                    yield pos,1,LPAREN
                    yield! loop (pos+1) rest

                | On (tryFirst ')') rest ->
                    yield pos,1,RPAREN
                    yield! loop (pos+1) rest

                | never -> failwithf "%A" never
            }
        loop 0 inp
