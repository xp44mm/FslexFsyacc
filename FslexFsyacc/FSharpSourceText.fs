module FslexFsyacc.FSharpSourceText

open System
open FSharp.Idioms
open FSharp.Idioms.StringOps
open FSharp.Idioms.RegularExpressions

open System.Text.RegularExpressions
open FSharp.Literals.Literal

let tryWS =
    Regex @"^\s+"
    |> trySearch

let trySingleLineComment =
    Regex @"^//[^\r\n]*"
    |> trySearch

let tryMultiLineComment =
    Regex @"^\(\*(?!\s*\))[\s\S]*?\*\)"
    |> trySearch

let tryDoubleTick =
    Regex @"^``[ \S]+?``"
    |> trySearch

let tryTypeParameter =
    Regex @"^'\w+(?!')"
    |> trySearch

let tryChar =
    Regex @"^'(\\.|[^\\'])+'" // 转义斜杠后面紧跟着的一个字符作为整体看待。
    |> trySearch

let trySingleQuoteString =
    Regex @"^""(\\.|[^\\""])*"""
    |> trySearch

let tryVerbatimString =
    Regex @"^@""(""""|[^""])*"""
    |> trySearch

let tryTripleQuoteString =
    Regex @"^""""""(?!"")[\s\S]*?(?<!"")""""""(?!"")"
    |> trySearch

let tryWord =
    Regex @"^\w+"
    |> trySearch

// 不终止循环的消费者 fsharpCodeCommonTries
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
        Regex @"^\S" |> trySearch
    ]
    |> List.map(fun f -> 
        f 
        >> Option.map(fun mat -> 
            // 0代表不终止循环标记，%}出现的次数
            0,mat.Value)
        )

let tryPercentRBrace = 
    tryStartsWith "%}" 
    // 1代表终止循环标记，%}出现的次数
    >> Option.map(fun capt -> 1,capt) // capt = "%}"

let tryHeaderTokens = tryPercentRBrace :: tries

//匹配%{%}
let getHeaderLength (inp:string) =
    let rec loop len =
        if len > inp.Length-1 then failwith $"{len}"
        let bal,capt =
            tryHeaderTokens
            |> List.pick(fun tryToken -> tryToken inp.[len..])
        let len = len+capt.Length
        if bal = 0 then
            loop len
        else
            len
    loop 0

let tryLBrace = tryFirst '{' >> Option.map(fun capt -> -1,string capt)

let tryRBrace = tryFirst '}' >> Option.map(fun capt -> 1,string capt)

let trySemanticTokens = tryLBrace :: tryRBrace :: tries

//匹配{}
let getSemanticLength(inp:string) =
    let rec loop depth len =
        if len > inp.Length-1 then 
            failwith $"depth:{depth};len:{len}"
        let d,capt =
            trySemanticTokens
            |> List.pick(fun tryToken ->
                tryToken inp.[len..]
                )
        let depth = depth + d
        let len = len + capt.Length
        if depth = 0 then
            len
        else
            loop depth len
    loop -1 0

let tryHeader(inp:string) =
    inp 
    |> tryStartsWith "%{"
    |> Option.map(fun capt ->
        let rest = inp.[capt.Length..]
        let len = capt.Length+getHeaderLength rest
        let hdr = inp.Substring(0,len)
        hdr
    )

let trySemantic(inp:string) =
    inp 
    |> tryStartsWith "{"
    |> Option.map(fun capt ->
        let rest = inp.[capt.Length..]
        let len = capt.Length+getSemanticLength rest
        let hdr = inp.Substring(0,len)
        hdr//,capt.[len..]
    )

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
