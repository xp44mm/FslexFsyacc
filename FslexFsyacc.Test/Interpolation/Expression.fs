namespace Interpolation

open FSharp.Idioms
open FSharp.Idioms.StringOps
open System.Text.RegularExpressions

type Expression =
    | GroupingExpression of Expression
    | PrefixExpression  of string * Expression
    | PostfixExpression  of Expression*string
    | BinaryExpression of Expression*string*Expression
    | TernaryExpression of  Expression*string*Expression*string*Expression
    | Number of float