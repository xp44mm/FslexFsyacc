module FSharpAnatomy.FSharpTokenScratch

open System
open FSharp.Idioms
open System.Text.RegularExpressions
open FSharp.Idioms.RegularExpressions

let tryWS =
    Regex @"^\s+"
    |> tryMatch

let tryWord =
    Regex @"^\w+"
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

let tryQTypar =
    Regex @"^'\w+(?!')"
    |> tryMatch

let tryHTypar =
    Regex @"^\^\w+(?!')"
    |> tryMatch

let tryIdent =
    Regex @"^[_\p{L}\p{Nl}][\p{L}\p{Mn}\p{Mc}\p{Nl}\p{Nd}\p{Pc}\p{Cf}']*"
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

let tryOperatorName =
    Regex @"^\(\s*[!$%&*+-./:<=>?@^|~]+\s*\)"
    |> tryMatch
