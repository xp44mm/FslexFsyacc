module FslexFsyacc.Runtime.ItemCores.DummyToken

open FSharp.Idioms
open FSharp.Idioms.Literal
open FslexFsyacc.Runtime

/// Normally, the precedence of a production is taken to be the same as
/// that of its rightmost terminal.
/// 过滤终结符，并反向保存在list中。
let lastTerminal (terminals:Set<string>) (production:string list) =
    production.Tail
    |> List.indexed
    |> List.filter(fun (i,s) -> terminals.Contains s)
    |> List.tryLast
