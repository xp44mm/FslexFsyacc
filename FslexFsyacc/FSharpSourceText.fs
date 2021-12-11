module FslexFsyacc.FSharpSourceText

open FSharp.Idioms
open System.Text.RegularExpressions

let tryWhiteSpace =
    Regex @"^[\s-[\n]]+"
    |> tryRegexMatch

let tryLineTerminator =
    Regex @"^\r?\n"
    |> tryRegexMatch

let trySingleLineComment =
    Regex @"^//.*"
    |> tryRegexMatch

let tryMultiLineComment =
    Regex @"^\(\*(?!\s*\))[\s\S]*?\*\)"
    |> tryRegexMatch

let tryDoubleTick =
    Regex @"^``[ \S]+?``"
    |> tryRegexMatch

let tryTypeParameter =
    Regex @"^'\w+(?!')"
    |> tryRegexMatch

let tryChar =
    Regex @"^'(\\.|[^\\'])+'" // 转义斜杠后面紧跟着的一个字符作为整体看待。
    |> tryRegexMatch

let trySingleQuoteString =
    Regex @"^""(\\.|[^\\""])*"""
    |> tryRegexMatch

let tryVerbatimString =
    Regex @"^@""(""""|[^""])*"""
    |> tryRegexMatch

let tryTripleQuoteString =
    Regex @"^""""""(?!"")[\s\S]*?(?<!"")""""""(?!"")"
    |> tryRegexMatch

let tryWord =
    Regex @"^\w+"
    |> tryRegexMatch

let tries = 
    [
        tryWhiteSpace
        tryLineTerminator
        trySingleLineComment
        tryMultiLineComment
        tryDoubleTick
        tryTypeParameter
        tryChar
        trySingleQuoteString
        tryVerbatimString
        tryTripleQuoteString
        tryWord
        Regex @"^\S" |> tryRegexMatch
    ]
    |> List.map(fun f -> f >> Option.map(fun(x,rest)-> 0,x,rest))

let tryPercentRBrace = tryPrefix "%}" >> Option.map(fun(x,rest)-> 1,x,rest)
let tryHeaderTokens = tryPercentRBrace :: tries

let getHeaderLength (inp:string) =
    let rec loop len seed =
        if seed = "" then failwithf "%A" (len,inp)
        let i,x,rest =
            tryHeaderTokens
            |> List.pick(fun tryToken ->
                tryToken seed
                )
        let len = len+x.Length
        if i = 0 then
            loop len rest
        else
            len
    loop 0 inp

let tryLBrace = tryFirstChar '{' >> Option.map(fun rest -> -1,"{",rest)
let tryRBrace = tryFirstChar '}' >> Option.map(fun rest -> 1,"}",rest)
let tryActionTokens = tryLBrace :: tryRBrace :: tries

let getNestedActionLength(inp:string) =
    let rec loop depth len seed =
        if seed = "" then failwithf "%A" (depth,len,inp)
        let i,x,rest =
            tryActionTokens
            |> List.pick(fun tryToken ->
                tryToken seed
                )
        let depth = depth + i
        let len = len + x.Length
        if depth = 0 then
            len
        else
            loop depth len rest
    loop -1 0 inp

let tryHeader(inp:string) =
    let start = "%{"
    inp 
    |> tryStartWith(start)
    |> Option.map(fun rest ->
        let len = getHeaderLength rest
        let hdr = start + rest.[0..len-1]
        hdr,rest.[len..]
    )

let tryAction(inp:string) =
    let start = "{"
    inp 
    |> tryStartWith(start)
    |> Option.map(fun rest ->
        let len = getNestedActionLength rest
        let hdr = start + rest.[0..len-1]
        hdr,rest.[len..]
    )
