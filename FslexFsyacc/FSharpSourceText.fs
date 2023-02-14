module FslexFsyacc.FSharpSourceText

open System
open FSharp.Idioms
open FSharp.Idioms.StringOps
open FSharp.Idioms.RegularExpressions

open System.Text.RegularExpressions

let tryWS =
    Regex @"^\s+"
    |> tryMatch

let trySingleLineComment =
    Regex @"^//.*"
    |> tryMatch

let tryMultiLineComment =
    Regex @"^\(\*(?!\s*\))[\s\S]*?\*\)"
    |> tryMatch

let tryDoubleTick =
    Regex @"^``[ \S]+?``"
    |> tryMatch

let tryTypeParameter =
    Regex @"^'\w+(?!')"
    |> tryMatch

let tryChar =
    Regex @"^'(\\.|[^\\'])+'" // 转义斜杠后面紧跟着的一个字符作为整体看待。
    |> tryMatch

let trySingleQuoteString =
    Regex @"^""(\\.|[^\\""])*"""
    |> tryMatch

let tryVerbatimString =
    Regex @"^@""(""""|[^""])*"""
    |> tryMatch

let tryTripleQuoteString =
    Regex @"^""""""(?!"")[\s\S]*?(?<!"")""""""(?!"")"
    |> tryMatch

let tryWord =
    Regex @"^\w+"
    |> tryMatch

// 不终止循环的消费者
let tries = 
    [
        tryWS
        trySingleLineComment
        tryMultiLineComment
        tryDoubleTick
        tryTypeParameter
        tryChar
        trySingleQuoteString
        tryVerbatimString
        tryTripleQuoteString
        tryWord
        Regex @"^\S" |> tryMatch
    ]
    |> List.map(fun f -> 
        f 
        >> Option.map(fun(x,rest)-> 
            // 0代表不终止循环标记，%}出现的次数
            0,x,rest)
        )

let tryPercentRBrace = 
    tryStart "%}" 
    // 1代表终止循环标记，%}出现的次数
    >> Option.map(fun rest -> 1,"%}",rest)

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

let tryLBrace = tryFirst '{' >> Option.map(fun rest -> -1,"{",rest)

let tryRBrace = tryFirst '}' >> Option.map(fun rest -> 1,"}",rest)

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
    |> tryStart(start)
    |> Option.map(fun rest ->
        let len = getHeaderLength rest
        let hdr = start + rest.[0..len-1]
        hdr,rest.[len..]
    )

let trySemantic(inp:string) =
    let start = "{"
    inp 
    |> tryStart(start)
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
    | Rgx @"^[^\n]*\n" m ->
        let x = m.Value
        let rest = inp.Substring(x.Length)
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

// spaceCount是code前面填补的空格数
// code 不带开括号，也不带闭括号
let formatNestedCode (spaceCount:int) (code:string) =
    let lines =
        space spaceCount + code //补齐首行
        |> Line.splitLines //分行
        |> Seq.map(fun (i,line) -> line.TrimEnd()) //去掉行尾空格
        |> Seq.filter(fun line -> line > "") // 删除空行
        |> Seq.toList

    //行首空格数
    let spaces = Line.startSpaces lines

    lines
    |> List.map(fun line -> line.[spaces..])
    |> String.concat Environment.NewLine
