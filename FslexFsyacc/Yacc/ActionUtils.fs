module FslexFsyacc.Yacc.ActionUtils
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.LALRs

let from (conflicts:Set<ItemCore>) =
    let reduces,shifts =
        conflicts
        |> Set.partition(ItemCoreUtils.dotmax)

    let reduces =
        reduces
        |> Set.map(fun icore -> // 1 to 1
            Reduce icore.production
        )
    let shifts =
        if shifts.IsEmpty then
            Set.empty
        else
            let nextKernel =
                shifts
                |> Set.map(ItemCoreUtils.dotIncr)
            Set.singleton (Shift nextKernel) // all to 1

    let actions = reduces + shifts
    actions

