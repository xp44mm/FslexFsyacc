module FslexFsyacc.Fslex.FslexTokenUtils

open FSharp.Idioms
open FSharp.Idioms.StringOps
open FSharp.Idioms.RegularExpressions
open FSharp.Idioms.ActivePatterns
open FSharp.Literals.Literal

open System.Text.RegularExpressions
open FslexFsyacc.FSharpSourceText
open FslexFsyacc.Runtime
open System

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

let tokenize (offset:int) (input:string) =
    let rec loop (lpos:int,lrest:string) (pos:int,rest:string) =
        seq {
            match rest with
            | "" -> ()
            | On tryWS m ->
                let len = m.Length
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | On trySingleLineComment m ->
                let len = m.Length
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | On tryMultiLineComment m ->
                let len = m.Length
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | Rgx @"^\w+" m ->
                yield Position<_>.from(pos,m.Length,ID m.Value)
                yield! loop (lpos,lrest) (pos+m.Length,rest.[m.Length..])

            | Rgx @"^<(\w+)>\s*(=)?" m ->
                let g1 = m.Groups.[1]
                let tok =
                    if m.Groups.[2].Success then
                        CAP g1.Value
                    else
                        HOLE g1.Value
                let len = g1.Length+2 // '<(\w+)>' 的长度
                yield Position<_>.from(pos, len, tok)
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            //| Rgx @"^<(\w+)>" m -> //todo: @"^<(\w+)>\s*(=)?"
            //    let len = m.Length
            //    yield Position<_>.from(pos,len,HOLE m.Groups.[1].Value)
            //    yield! loop (lpos,lrest) (pos+len,rest.[len..])

            //| Rgx @"^(\w+)\s*(=)?" m -> //todo: @"^\w+"
            //    let g1 = m.Groups.[1]
            //    let tok =
            //        if m.Groups.[2].Success then
            //            CAP g1.Value
            //        else
            //            ID g1.Value
            //    yield Position<_>.from(pos, g1.Length, tok)
            //    yield! loop (lpos,lrest) (pos+g1.Length,rest.[g1.Length..])


            | On trySingleQuoteString m ->
                let len = m.Length
                yield Position<_>.from(pos,len,LITERAL(JsonString.unquote m.Value))
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | On trySemantic capt ->
                let len = capt.Length
                let code = capt.[1..len-2]

                let nlpos,nlinp,fcode =
                    if System.String.IsNullOrWhiteSpace(code) then
                        lpos,lrest,""
                    else
                        let col,nli = Line.getColumnAndLpos (lpos,lrest) (pos+1)
                        let nlrest = input.[offset+nli..]
                        let fcode = formatNestedCode col code
                        nli,nlrest,fcode

                yield Position<_>.from(pos, len, SEMANTIC fcode)
                yield! loop (nlpos,nlinp) (pos+len,rest.[len..])

            | On tryHeader x ->
                let len = x.Length
                let code = x.[2..len-3]

                let nli,nlrest,fcode =
                    if System.String.IsNullOrWhiteSpace(code) then
                        lpos,lrest,""
                    else
                        let col,nli = Line.getColumnAndLpos (lpos,lrest) (pos+2)
                        let nlrest = input.[offset+nli..]
                        let fcode = formatNestedCode col code
                        nli,nlrest,fcode

                yield Position<_>.from(pos,len,HEADER fcode)
                yield! loop (nli,nlrest) (pos+len,rest.[len..])

            | Rgx @"^%%+" m ->
                let x = m.Value
                yield Position<_>.from(pos,x.Length,PERCENT)
                yield! loop (lpos,lrest) (pos+x.Length,rest.[m.Length..])

            | LongestPrefix (Map.keys ops) x ->
                let len = x.Length
                yield Position<_>.from(pos,len,ops.[x])
                yield! loop (lpos,lrest) (pos+len,rest.[x.Length..])

            | rest -> failwith $"tokenize:{rest}"
        }

    loop (offset,input) (offset,input)

let appendAMP (lexbuf: Position<FslexToken> list) =
    let last =
        lexbuf
        |> List.exactlyOne
    [last;Position<_>.from(last.nextIndex,0,AMP)]
