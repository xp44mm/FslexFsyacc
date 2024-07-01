module FslexFsyacc.Fslex.FslexTokenUtils

open FSharp.Idioms
open FSharp.Idioms.StringOps

open FSharp.Idioms.RegularExpressions
open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.Literal
open FSharp.Idioms.PointFree

open System
open System.Text.RegularExpressions
open FslexFsyacc.VanillaFSharp.FSharpSourceText
open FslexFsyacc
open FslexFsyacc.SourceTextTry

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

let getTag (token: _ PositionWith) =
    match token.value with
    | x when ops_inverse.ContainsKey x -> ops_inverse.[x]

    | HEADER   _ -> "HEADER"
    | ID       _ -> "ID"
    | CAP      _ -> "CAP"
    | LITERAL  _ -> "LITERAL"
    | REDUCER _ -> "REDUCER"
    | HOLE     _ -> "HOLE"
    | _ -> failwith "getTag Wild"

let getLexeme (token: _ PositionWith) =
    match token.value with
    | HEADER   x -> box x
    | ID       x -> box x
    | CAP      x -> box x
    | LITERAL  x -> box x
    | REDUCER x -> box x
    | HOLE    x -> box x
    | _ -> null

let tokenize (sourceText:SourceText) = // (offset:int) (input:string) =
    let rec loop (lineSrc:SourceText) (src:SourceText) = // (lpos:int,lrest:string) (pos:int,rest:string) =
        seq {
            match src.text with
            | "" -> ()
            | On tryWS m ->
                let len = m.Length
                let src = src.skip len
                yield! loop lineSrc src //(lpos,lrest) (pos+len,rest.[len..])

            | On trySingleLineComment m ->
                let len = m.Length
                let src = src.skip len
                yield! loop lineSrc src // (lpos,lrest) (pos+len,rest.[len..])

            | On tryMultiLineComment m ->
                let len = m.Length
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | Rgx @"^\w+" m ->
                yield PositionWith<_>.just(src.index, m.Length, ID m.Value)
                let src = src.skip m.Length
                yield! loop lineSrc src // (pos+m.Length,rest.[m.Length..])

            | Rgx @"^<(\w+)>\s*(=)?" m ->
                let g1 = m.Groups.[1]
                let tok =
                    if m.Groups.[2].Success then
                        CAP g1.Value
                    else
                        HOLE g1.Value
                let len = g1.Length+2 // '<(\w+)>' 的长度
                yield PositionWith<_>.just(src.index, len, tok)
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | On trySingleQuoteString m ->
                let len = m.Length
                yield PositionWith<_>.just(src.index, len,LITERAL(Json.unquote m.Value))
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | On tryReducer capt ->
                let len = capt.Length
                let code = capt.[1..len-2]

                let lineSrc,fcode =
                    if System.String.IsNullOrWhiteSpace(code) then
                        lineSrc,""
                    else
                        let col,nli = lineSrc.getColumnAndNextLine (src.index+1)
                        let lineSrc = lineSrc.jump(nli) // input.[offset+nli..]
                        let fcode = formatNestedCode col code
                        lineSrc,fcode

                yield PositionWith<_>.just(src.index, len, REDUCER fcode)
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | On tryHeader x ->
                let len = x.Length
                let code = x.[2..len-3]

                let lineSrc,fcode =
                    if System.String.IsNullOrWhiteSpace(code) then
                        lineSrc,""
                    else
                        let col,nli = lineSrc.getColumnAndNextLine (src.index+2)
                        let lineSrc = lineSrc.jump(nli) // input.[offset+nli..]
                        let fcode = formatNestedCode col code
                        lineSrc,fcode

                yield PositionWith<_>.just(src.index,len,HEADER fcode)
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | Rgx @"^%%+" m ->
                //let x = m.Value
                yield PositionWith<_>.just(src.index,m.Length,PERCENT)
                let src = src.skip m.Length
                yield! loop lineSrc src // (pos+m.Length,rest.[m.Length..])

            | LongestPrefix (Map.keys ops) x ->
                let len = x.Length
                yield PositionWith<_>.just(src.index,len,ops.[x])
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[x.Length..])

            | _ -> failwith $"tokenize:{src}"
        }

    twice loop sourceText //(offset,input)

let appendAMP (lexbuf: PositionWith<FslexToken> list) =
    let last =
        lexbuf
        |> List.exactlyOne
    [last;PositionWith<_>.just(last.index + last.length,0,AMP)]
