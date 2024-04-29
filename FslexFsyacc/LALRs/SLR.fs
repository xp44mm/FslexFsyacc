namespace FslexFsyacc.LALRs

open FslexFsyacc.Runtime
open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FSharp.Idioms

type SLR = 
    {
    items: Set<ItemCore>
    }

    static member just(items) = { items = items }

    /// shift/reduce
    member this.isSRConflict () =
        if this.items.Count > 1 then
            let reduces =
                this.items
                |> Set.filter(fun i -> i.dotmax)
            reduces.Count = 1
        else false

    member this.isConflict () =
        // 无需限定冲突符号必须是终结符号，
        // 冲突一定存在reduce，reduce的lookahead一定是终结符号
        if this.items.Count > 1 then
            this.items
            |> Set.exists(fun i -> i.dotmax)
        else false

    member conflicts.toActions () =
        let reduces,shifts =
            conflicts.items
            |> Set.partition(fun i -> i.dotmax)

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
                    |> Set.map(fun i -> i.advance())
                    |> Set.map(fun i -> id<FslexFsyacc.Runtime.ItemCore> { production = i.production; dot = i.dot })
                Set.singleton (Shift nextKernel) // all to 1

        let actions = reduces + shifts
        actions


