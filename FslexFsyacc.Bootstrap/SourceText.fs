module FslexFsyacc.SourceText

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

let tryFSharpIdent =
    Regex @"^[_\p{L}\p{Nl}][\p{L}\p{Mn}\p{Mc}\p{Nl}\p{Nd}\p{Pc}\p{Cf}']*"
    |> trySearch
