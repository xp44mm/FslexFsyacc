module FslexFsyacc.Runtime.ParseTables.ProductionUtils
open FSharp.Idioms

/// Normally, the precedence of a production is taken to be the same as
/// that of its rightmost terminal.
/// 过滤终结符，并反向保存在list中。
let revTerminalsOfProduction (terminals:Set<string>) (production:string list) =
    let rec loop (revls:string list) ls =
        match ls with
        | [] -> revls
        | h :: t -> 
            if terminals.Contains h then
                loop (h::revls) t
            else loop revls t
    loop [] production.Tail
