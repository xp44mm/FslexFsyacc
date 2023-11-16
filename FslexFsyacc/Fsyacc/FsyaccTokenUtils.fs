module FslexFsyacc.Fsyacc.FsyaccTokenUtils

open FslexFsyacc.VanillaFSharp.FSharpSourceText

open FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Idioms.StringOps
open FSharp.Idioms.RegularExpressions
open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.Literal

open System.Text.RegularExpressions

let ops = Map [
    "%%",PERCENT;
    "(",LPAREN;
    ")",RPAREN;
    "*",STAR;
    "+",PLUS;
    ":",COLON;
    "?",QMARK;
    "[",LBRACK;
    "]",RBRACK;
    "|",BAR;
    ]

let ops_inverse = 
    ops 
    |> Map.inverse 
    |> Map.map(fun k v -> Seq.exactlyOne v)

/// the tag of token
let getTag(token:Position<FsyaccToken>) =
    match token.value with
    | x when ops_inverse.ContainsKey x -> ops_inverse.[x]

    | HEADER   _ -> "HEADER"
    | ID       _ -> "ID"
    | LITERAL  _ -> "LITERAL"
    | SEMANTIC _ -> "SEMANTIC"
    | TYPE_ARGUMENT _ -> "TYPE_ARGUMENT"
    | LEFT     -> "%left"
    | RIGHT    -> "%right"
    | NONASSOC -> "%nonassoc"
    | PREC     -> "%prec"
    | TYPE     -> "%type"
    | tok -> failwith $"getTag:{stringify tok}"

/// 获取token携带的语义信息§
let getLexeme(token:Position<_>) =
    match token.value with
    | HEADER   x -> box x
    | ID       x -> box x
    | LITERAL  x -> box x
    | SEMANTIC x -> box x
    | TYPE_ARGUMENT x -> box x

    | _ -> null

let tokenize (offset:int) (input:string) =
    
    /// lpos:行首的索引，上一次缓存的
    /// lrest = input.[lpos-offset..]
    ///  rest = input.[ pos-offset..]
    let rec loop (lpos:int,lrest:string) (pos:int,rest:string) =
        seq {
            match rest with
            | "" -> ()
            | On tryWS x ->
                let len = x.Length
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | On trySingleLineComment x ->
                let len = x.Length
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | On tryMultiLineComment x ->
                let len = x.Length
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | On tryWord x ->
                let len = x.Length
                yield Position.from(pos, len, ID x.Value)
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | On trySingleQuoteString x ->
                let len = x.Length
                yield Position.from(pos,len,LITERAL(JsonString.unquote x.Value))
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | On trySemantic x ->
                let len = x.Length
                let code = x.[1..len-2]

                let nlpos,nlinp,fcode =
                    if System.String.IsNullOrWhiteSpace(code) then
                        lpos,lrest,""
                    else
                        let col,nlpos = Line.getColumnAndLpos (lpos,lrest) (pos+1)
                        let nlinp = input.[offset+nlpos..]
                        let fcode = formatNestedCode col code
                        nlpos,nlinp,fcode

                yield Position.from(pos,len,SEMANTIC fcode)
                yield! loop (nlpos,nlinp) (pos+len,rest.[len..])

            | On tryHeader x ->
                let len = x.Length
                let code = x.[2..len-3]

                let nlpos,nlinp,fcode =
                    if System.String.IsNullOrWhiteSpace(code) then
                        lpos,lrest,""
                    else
                        let col,nlpos = Line.getColumnAndLpos (lpos,lrest) (pos+2)
                        let nlinp = input.[offset+nlpos..]
                        let fcode = formatNestedCode col code
                        nlpos,nlinp,fcode

                yield Position.from(pos,len,HEADER fcode)
                yield! loop (nlpos,nlinp) (pos+len,rest.[len..])
            
            | Rgx @"^%[a-z]+" m ->
                let tok =
                    match m.Value with
                    | "%left" -> LEFT
                    | "%right" -> RIGHT
                    | "%nonassoc" -> NONASSOC
                    | "%prec" -> PREC
                    | "%type" -> TYPE
                    | never -> failwith ""
                let len = m.Length
                yield Position.from(pos,len,tok)
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | Rgx @"^%%+" m ->
                let len = m.Length
                yield Position.from(pos,len,PERCENT)
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | LongestPrefix (Map.keys ops) x ->
                let len = x.Length
                yield Position.from(pos,len,ops.[x])
                yield! loop (lpos,lrest) (pos+len,rest.[len..])
            | First '<' _ ->
                let postok = 
                    let rg = FslexFsyacc.VanillaFSharp.TypeArgumentAngleCompiler.getRange pos rest 
                    {
                        index = rg.index
                        length = rg.length
                        value = TYPE_ARGUMENT rg.value
                    }

                yield postok
                yield! loop (lpos,lrest) (postok.nextIndex, rest.[postok.length..])

            | _ -> failwith $"tokenize:{rest}"
        }
    loop (offset,input) (offset,input)



