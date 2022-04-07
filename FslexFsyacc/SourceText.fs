module FslexFsyacc.SourceText

open FSharp.Idioms
open System.Text.RegularExpressions

let tryWhiteSpace =
    Regex @"^[\s-[\n]]+"
    |> tryMatch

let tryLineTerminator =
    Regex @"^(\r?\n|\r)"
    |> tryMatch

let trySingleLineComment =
    Regex @"^//.*"
    |> tryMatch

let tryMultiLineComment =
    Regex @"^/\*[\s\S]*?\*/"
    |> tryMatch

//An identifier must start with $, _, or any character in the Unicode categories
//“Uppercase letter (Lu)”, “Lowercase letter (Ll)”, “Titlecase letter (Lt)”, “Modifier letter (Lm)”, “Other letter (Lo)”, or
//“Letter number (Nl)”.

//The rest of the string can contain the same characters, plus any
//U+200C zero width non-joiner characters,
//U+200D zero width joiner characters, and characters in the Unicode categories
//“Non-spacing mark (Mn)”, “Spacing combining mark (Mc)”, “Decimal digit number (Nd)”, or “Connector punctuation (Pc)”.

let tryIdentifierName =
    Regex @"^[$_\p{L}\p{Nl}][$_\p{L}\p{Mn}\p{Mc}\p{Nl}\p{Nd}\p{Pc}\u200C\u200D]*"
    |> tryMatch

let tryOptionalChainingPunctuator =
    Regex @"^\?\.(?!\d)"
    |> tryMatch

let tryDivPunctuator = Regex @"^/=?" |> tryMatch

let tryRightBracePunctuator = tryFirst '}'

let illegalNumberSep (input: string) = Regex.IsMatch(input, "(^_|_$|\D_|_\D)")

let tryBinaryIntegerLiteral =
    Regex @"^0[bB][01_]+n?\b"
    |> tryMatch

let tryOctalIntegerLiteral =
    Regex @"^0[oO][0-7_]+n?\b"
    |> tryMatch

let tryHexIntegerLiteral =
    Regex @"^0[xX][0-9a-fA-F_]+n?\b"
    |> tryMatch

let tryDecimalIntegerLiteral =
    Regex @"^\d[\d_]*n?\b"
    |> tryMatch

let tryDecimalLiteral =
    Regex @"^(?!_)([\d_]*\.[\d_]+|[\d_]+\.[\d_]*)([eE][-+]?[\d_]+)?"
    |> tryMatch

let trySingleStringLiteral =
    Regex @"^'(\\\\|\\'|[^'])*'"
    |> tryMatch

let tryDoubleStringLiteral =
    Regex """^"(\\\\|\\"|[^"])*(")"""
    |> tryMatch

//这是简易模板，要求内部的注释，字符串字面量等不能再包含`反引号。
let tryTemplate =
    Regex @"^`(\\\\|\\`|[^`])*`"
    |> tryMatch

let tryRegularExpressionLiteral =
    Regex @"^/(\\\\|\\/|[^/])+/[gimsuy]*"
    |> tryMatch

