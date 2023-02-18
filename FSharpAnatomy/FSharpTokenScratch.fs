module FSharpAnatomy.FSharpTokenScratch

open System
open FSharp.Idioms
open FSharp.Idioms.StringOps
open FSharp.Idioms.RegularExpressions
open FSharp.Idioms.ActivePatterns

open System.Text.RegularExpressions

let tryWS =
    Regex @"^\s+"
    |> trySearch

let tryWord =
    Regex @"^\w+"
    |> trySearch

let trySingleLineComment =
    Regex @"^//.*"
    |> trySearch

let tryMultiLineComment =
    Regex @"^\(\*(?!\s*\))[\s\S]*?\*\)"
    |> trySearch

let tryDoubleTick =
    Regex @"^``[ \S]+?``"
    |> trySearch

let tryQTypar =
    Regex @"^'\w+(?!')"
    |> trySearch

let tryHTypar =
    Regex @"^\^\w+(?!')"
    |> trySearch

let tryIdent =
    Regex @"^[_\p{L}\p{Nl}][\p{L}\p{Mn}\p{Mc}\p{Nl}\p{Nd}\p{Pc}\p{Cf}']*"
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

let tryOperatorName =
    Regex @"^\(\s*[!$%&*+-./:<=>?@^|~]+\s*\)"
    |> trySearch
