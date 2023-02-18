module FslexFsyacc.Fsyacc.FsyaccTokenUtils

open FslexFsyacc.FSharpSourceText
open FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Idioms.StringOps
open FSharp.Idioms.RegularExpressions
open FSharp.Idioms.ActivePatterns

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
let getTag(token:Position<_>) =
    match token.value with
    | x when ops_inverse.ContainsKey x -> ops_inverse.[x]

    | HEADER   _ -> "HEADER"
    | ID       _ -> "ID"
    | LITERAL  _ -> "LITERAL"
    | SEMANTIC _ -> "SEMANTIC"
    | LEFT     -> "%left"
    | RIGHT    -> "%right"
    | NONASSOC -> "%nonassoc"
    | PREC     -> "%prec"
    | _ -> failwith $"getTag"

/// 获取token携带的语义信息§
let getLexeme(token:Position<_>) =
    match token.value with
    | HEADER   x -> box x
    | ID       x -> box x
    | LITERAL  x -> box x
    | SEMANTIC x -> box x
    | _ -> null

let tokenize (index:int) (txt:string) =
    
    /// lpos:行首的索引
    /// linp:从行首开始，到inp结束的字符串
    /// pos: inp开始的字符串
    let rec loop (lpos:int) (pos:int) = //,linp:string,inp:string
        seq {
            match txt.[index+pos..] with
            | "" -> ()
            | On tryWS x ->
                let len = x.Length
                yield! loop (lpos) (pos+len) //,linp,rest

            | On trySingleLineComment x ->
                let len = x.Length
                yield! loop (lpos) (pos+len) //,linp,rest

            | On tryMultiLineComment x ->
                let len = x.Length
                yield! loop (lpos) (pos+len) //,linp,rest

            | On tryWord x ->
                let len = x.Length
                yield Position<_>.from(pos, len, ID x.Value)
                yield! loop (lpos) (pos+len) //,linp,rest

            | On trySingleQuoteString x ->
                let len = x.Length
                yield Position<_>.from(pos,len,LITERAL(JsonString.unquote x.Value))
                yield! loop (lpos) (pos+len) //,linp,rest

            | On trySemantic x ->
                let len = x.Length
                let code = x.[1..len-2]

                let nlpos,fcode = //,nlinp
                    if System.String.IsNullOrWhiteSpace(code) then
                        lpos,"" //,linp
                    else
                        let linp = txt.[index+lpos..]
                        let col,nlpos = Line.getColumnAndLpos (lpos,linp) (pos+1)
                        let fcode = formatNestedCode col code
                        nlpos,fcode //,nlinp

                yield Position<_>.from(pos,len,SEMANTIC fcode)
                yield! loop (nlpos) (pos+len) //,nlinp,rest

            | On tryHeader x ->
                let len = x.Length
                let code = x.[2..len-3]

                let nlpos,fcode = //,nlinp
                    if System.String.IsNullOrWhiteSpace(code) then
                        lpos,"" //,linp
                    else
                        let linp = txt.[index+lpos..]
                        let col,nlpos = Line.getColumnAndLpos (lpos,linp) (pos+2)
                        let fcode = formatNestedCode col code
                        nlpos,fcode //,nlinp

                yield Position<_>.from(pos,len,HEADER fcode)
                yield! loop (nlpos) (pos+len) //,nlinp,rest
            
            | Rgx @"^%[a-z]+" m ->
                let x = m.Value
                //let rest = inp.Substring(x.Length)

                let tok =
                    match x with
                    | "%left" -> LEFT
                    | "%right" -> RIGHT
                    | "%nonassoc" -> NONASSOC
                    | "%prec" -> PREC
                    | never -> failwith ""
                let len = x.Length
                yield Position<_>.from(pos,len,tok)
                yield! loop (lpos) (pos+len) //,linp,rest

            | Rgx @"^%%+" m ->
                //let x = m.Value
                //let rest = inp.Substring(x.Length)

                let len = m.Length
                yield Position<_>.from(pos,len,PERCENT)
                yield! loop (lpos) (pos+len) //,linp,rest

            | LongestPrefix (Map.keys ops) x ->
                let len = x.Length
                let nextPos = pos+len
                yield Position<_>.from(pos,len,ops.[x])
                yield! loop (lpos) (nextPos) //,linp,rest

            | never -> failwith $"tokenize:{never}" 
        }
    loop (index) (index)



