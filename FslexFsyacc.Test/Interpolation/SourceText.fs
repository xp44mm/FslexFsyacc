module Interpolation.SourceText

open FSharp.Idioms
open System.Text.RegularExpressions

let tryWhiteSpace =
    Regex @"^[\u0009\u000b\u000c\ufeff\p{Zs}]+"
    |> tryMatch

let tryLineTerminatorSequence =
    Regex @"^(\r?\n|[\r\u2028\u2029])"
    |> tryMatch

let trySingleLineComment =
    Regex @"^//.*"
    |> tryMatch

//如果存在换行，则是一个换行
let tryMultiLineComment =
    Regex @"^/\*[\s\S]*?\*/"
    |> tryMatch

let tryIdentifierName =
//An identifier must start with $, _, or any character in the Unicode categories
//“Uppercase letter (Lu)”, 
//“Lowercase letter (Ll)”, 
//“Titlecase letter (Lt)”, 
//“Modifier letter (Lm)”, 
//“Other letter (Lo)”, or
//“Letter number (Nl)”.

//The rest of the string can contain the same characters, plus any
//U+200C zero width non-joiner characters,
//U+200D zero width joiner characters, and characters in the Unicode categories
//“Non-spacing mark (Mn)”, 
//“Spacing combining mark (Mc)”, 
//“Decimal digit number (Nd)”, or 
//“Connector punctuation (Pc)”.
// the underline `_` is included in \p{Pc}.
//    Regex @"^[$_\p{ID_Start}][$\p{ID_Continue}\u200C\u200D]*"

    let id_start = @"[$\p{L}\p{Nl}_]"
    let id_conti = @"[$\p{L}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\u200C\u200D]"
    Regex $"^{id_start}{id_conti}*"
    |> tryMatch

let reservedWords = 
    "await break case catch class const continue debugger default delete do else enum export extends false finally for function if import in instanceof new null return super switch this throw true try typeof var void while with yield"
    |> fun x -> x.Split(' ')

let Punctuators = set [
    "!"
    "!="
    "!=="
    "%"
    "%="
    "&"
    "&&"
    "&&="
    "&="
    "("
    ")"
    "*"
    "**"
    "**="
    "*="
    "+"
    "++"
    "+="
    ","
    "-"
    "--"
    "-="
    "."
    "..."
    ":"
    ";"
    "<"
    "<<"
    "<<="
    "<="
    "="
    "=="
    "==="
    "=>"
    ">"
    ">="
    ">>"
    ">>="
    ">>>"
    ">>>="
    "?"
    "??"
    "??="
    "["
    "]"
    "^"
    "^="
    "{"
    "|"
    "|="
    "||"
    "||="
    "~"

    "}"
    "/"
    "/="
    ]

let tryOptionalChainingPunctuator =
    Regex @"^\?\.(?!\d)"
    |> tryMatch

let tryDivPunctuator = Regex @"^/=?" |> tryMatch


//仅DecimalLiteral代表NumericLiteral
let tryNumericLiteral =
    Regex @"^(\.\d+|\d+\.\d*|\d+)([eE][-+]?\d+)?(?!\w)"
    |> tryMatch

let trySingleStringLiteral =
    Regex @"^'(\\.|[^\\'])*'"
    |> tryMatch

let tryDoubleStringLiteral =
    Regex """^"(\\.|[^\\"])*(?:")"""
    |> tryMatch

let tryRegularExpressionLiteral =
    Regex @"^/(\\\\|\\/|[^/\r\n\u2028\u2029])+/[gimsuy]*"
    |> tryMatch

let tryTemplateCharacters =
    Regex @"^(\\[\s\S]|[^\\`$]|[$](?![{]))*"
    |> tryMatch



