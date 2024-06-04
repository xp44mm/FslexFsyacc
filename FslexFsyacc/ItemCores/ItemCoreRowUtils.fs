module FslexFsyacc.Runtime.ItemCores.ItemCoreRowUtils
open System.Collections.Generic
open System.Collections.Concurrent

let rows = ConcurrentDictionary<string list,ItemCoreRow[]>(HashIdentity.Structural)

let getRow (production: string list) (dot: int) =
    let arr = rows.GetOrAdd(
        production, fun production ->
            production
            |> ItemCoreRow.spread
            |> Array.ofList
        )
    if 0 <= dot && dot <= production.Tail.Length then
        arr.[dot]
    else 
        failwith $"dot = {dot} should in [0..{production.Tail.Length}]"
