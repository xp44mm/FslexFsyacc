module FslexFsyacc.Fsyacc.FsyaccToken2Utils

open FslexFsyacc.VanillaFSharp.FSharpSourceText
open FslexFsyacc.SourceText

open FslexFsyacc

open FSharp.Idioms
open FSharp.Idioms.StringOps
open FSharp.Idioms.RegularExpressions
open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.Literal

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
let getTag(token:Position<FsyaccToken2>) =
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
let getLexeme(token:Position<FsyaccToken2>) =
    match token.value with
    | HEADER   x -> box x
    | ID       x -> box x
    | LITERAL  x -> box x
    | REDUCER x -> box x
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
                yield Position.from(pos,len,LITERAL(Json.unquote x.Value))
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

                yield Position.from(pos,len,REDUCER fcode)
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
                    | "%left" -> LEFTASSOC
                    | "%right" -> RIGHTASSOC
                    | "%nonassoc" -> NONASSOC
                    | "%prec" -> PREC
                    | "%type" -> TYPE
                    | _ -> failwith "never"
                let len = m.Length
                yield Position.from(pos,len,tok)
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | Rgx @"^%%+" m ->
                let len = m.Length
                yield Position.from(pos,len,PERCENT)
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | First '<' m ->
                let langle = Position.from(pos, 1, LANGLE)
                yield langle

                let pos = langle.nextIndex
                let rest = rest.[langle.length .. ]

                Console.WriteLine(stringify (pos,rest))
                                
                let targ, epos, erest = 
                    let exit (x:string) = Regex.IsMatch(x, @"^\s*\>")                   
                    TypeArguments.TypeArgumentCompiler.compile exit pos rest

                //let tas = rest.[0..epos-pos-1]
                let targ = FslexFsyacc.TypeArguments.TypeArgumentUtils.uniform targ
                let tta = Position.from(pos, epos-pos+1, TYPE_ARGUMENT targ)

                yield tta
                yield! loop (lpos,lrest) (epos, erest)

            | LongestPrefix (Map.keys ops) x ->
                let len = x.Length
                yield Position.from(pos,len,ops.[x])
                yield! loop (lpos,lrest) (pos+len,rest.[len..])

            | _ -> failwith $"tokenize:{rest}"
        }
    loop (offset,input) (offset,input)



