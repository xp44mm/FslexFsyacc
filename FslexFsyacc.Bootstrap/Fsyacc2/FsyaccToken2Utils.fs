module FslexFsyacc.Fsyacc.FsyaccToken2Utils

open FslexFsyacc.VanillaFSharp.FSharpSourceText
open FslexFsyacc.SourceTextTry

open FslexFsyacc

open FSharp.Idioms
open FSharp.Idioms.StringOps
open FSharp.Idioms.RegularExpressions
open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.Literal
open FSharp.Idioms.PointFree

open System
open System.Diagnostics
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
    "<",LANGLE;
    ">",RANGLE;
    ]

let ops_inverse = 
    ops 
    |> Map.inverse 
    |> Map.map(fun k v -> Seq.exactlyOne v)

/// the tag of token
let getTag(token:PositionWith<FsyaccToken2>) =
    match token.value with
    | x when ops_inverse.ContainsKey x -> ops_inverse.[x]

    | HEADER   _ -> "HEADER"
    | ID       _ -> "ID"
    | LITERAL  _ -> "LITERAL"
    | REDUCER _ -> "REDUCER"
    | TYPE_ARGUMENT _ -> "TYPE_ARGUMENT"
    | LEFTASSOC     -> "%left"
    | RIGHTASSOC    -> "%right"
    | NONASSOC -> "%nonassoc"
    | PREC     -> "%prec"
    | TYPE     -> "%type"
    | tok -> failwith (stringify tok)

/// 获取token携带的语义信息§
let getLexeme(token:PositionWith<FsyaccToken2>) =
    match token.value with
    | HEADER   x -> box x
    | ID       x -> box x
    | LITERAL  x -> box x
    | REDUCER x -> box x
    | TYPE_ARGUMENT x -> box x

    | _ -> null

let tokenize (sourceText:SourceText) = // (offset:int) (input:string) =
    
    /// lpos:行首的索引，上一次缓存的
    /// lrest = input.[lpos-offset..]
    ///  rest = input.[ pos-offset..]
    let rec loop (lineSrc:SourceText) (src:SourceText) = // (lpos:int,lrest:string) (pos:int,rest:string) =
        seq {
            match src.text with
            | "" -> ()
            | On tryWS x ->
                let len = x.Length
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | On trySingleLineComment x ->
                let len = x.Length
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | On tryMultiLineComment x ->
                let len = x.Length
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | On tryWord x ->
                let len = x.Length
                yield PositionWith<_>.just(src.index, len, ID x.Value)
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | On trySingleQuoteString x ->
                let len = x.Length
                yield PositionWith<_>.just(src.index,len,LITERAL(Json.unquote x.Value))
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | On tryReducer x ->
                let len = x.Length
                let code = x.[1..len-2]

                let lineSrc,fcode =
                    if System.String.IsNullOrWhiteSpace(code) then
                        lineSrc,""
                    else
                        let col,nli = lineSrc.getColumnAndNextLine (src.index+1)
                        let lineSrc = lineSrc.jump(nli) // input.[offset+nli..]
                        let fcode = formatNestedCode col code
                        lineSrc,fcode

                yield PositionWith<_>.just(src.index,len,REDUCER fcode)
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
            
            | Rgx @"^%[a-z]+" m ->
                let tok =
                    match m.Value with
                    | "%left" -> LEFTASSOC
                    | "%right" -> RIGHTASSOC
                    | "%nonassoc" -> NONASSOC
                    | "%prec" -> PREC
                    | "%type" -> TYPE
                    | _ -> failwith "never"
                let len = m.Length
                yield PositionWith<_>.just(src.index,len,tok)
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | Rgx @"^%%+" m ->
                let len = m.Length
                yield PositionWith<_>.just(src.index,len,PERCENT)
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | First '<' m ->
                //let src = SourceText.just(pos, rest)
                let langle = PositionWith<_>.just(src.index, 1, LANGLE)
                yield langle
                let src1 = src.skip langle.length
                //let pos = langle.nextIndex
                //let rest = rest.[langle.length .. ]
                                
                let targ, length = 
                    let exit (x:string) = Regex.IsMatch(x, @"^\s*\>")
                    TypeArguments.TypeArgumentCompiler.compile exit src1

                let tta = PositionWith<_>.just(src1.index, length, TYPE_ARGUMENT targ)

                yield tta
                let src2 = src1.skip tta.length
                yield! loop lineSrc src2 // (src2.index, src2.text)

            | LongestPrefix (Map.keys ops) x ->
                let len = x.Length
                yield PositionWith<_>.just(src.index,len,ops.[x])
                let src = src.skip len
                yield! loop lineSrc src // (pos+len,rest.[len..])

            | _ -> failwith $"tokenize:{src}"
        }

    twice loop sourceText // (offset,input)



