﻿module FslexFsyacc.FSharpSourceText

open System
open FSharp.Idioms
open System.Text.RegularExpressions

let tryWS =
    Regex @"^\s+"
    |> tryRegexMatch

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
let trySemanticTokens = tryLBrace :: tryRBrace :: tries

let getSemanticLength(inp:string) =
    let rec loop depth len seed =
        if seed = "" then failwithf "%A" (depth,len,inp)
        let i,x,rest =
            trySemanticTokens
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

let trySemantic(inp:string) =
    let start = "{"
    inp 
    |> tryStartWith(start)
    |> Option.map(fun rest ->
        let len = getSemanticLength rest
        let hdr = start + rest.[0..len-1]
        hdr,rest.[len..]
    )

/// pos是x第一个字符的位置
let rec getColumnAndRest (start:int, inp:string) (pos:int) =
    match inp with
    | "" -> 
        failwithf "length:%d < pos:%d" start pos
    | On (tryPrefix @"[^\n]*\n") (x, rest) ->
        let nextStart = start + x.Length
        if pos < nextStart then
            let col = pos - start
            col, nextStart, rest
        else
            getColumnAndRest (nextStart, rest) pos
    | _ ->
        let nextStart = start + inp.Length
        if pos < nextStart then
            let col = pos - start
            col,nextStart, ""
        else
            failwithf "length:%d < pos:%d" nextStart pos

//let col,nextstart,rest =
//    getColumnAndRest start inp pos

// spaceCount是code前面填补的空格数
// code 不带开括号，也不带闭括号
let formatNestedCode (spaceCount:int) (code:string) =
    let lines =
        space spaceCount + code //补齐首行
        |> splitLines //分行
        |> Seq.map(fun (i,line) -> line.TrimEnd()) //去掉行尾空格
        |> Seq.filter(fun line -> line > "") // 删除空行
        |> Seq.toArray

    //行首空格数
    let spaces =
        lines
        |> Array.map(fun line -> Regex.Match(line,"^ *").Length)
        |> Array.min

    lines
    |> Array.map(fun line -> line.[spaces..])
    |> String.concat Environment.NewLine