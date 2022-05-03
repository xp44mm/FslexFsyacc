namespace Interpolation

open FSharp.Idioms
open FSharp.Idioms.StringOps
open System.Text.RegularExpressions

type Token =
    | WhiteSpace of string
    | LineTerminatorSequence of string
    | SingleLineComment of string
    | MultiLineComment of string
    | IdentifierName of string
    | Punctuator of string
    | NumericLiteral of string
    | SingleStringLiteral of string
    | DoubleStringLiteral of string
    | ExpressionToken of Expression
