namespace FslexFsyacc.Runtime.LALRs

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FSharp.Idioms

/// SLR && LALR
type Action = 
    /// 这个shift包括龙书中的 *shift* lookahead and *goto* nonterminal
    | Shift of kernel: Set<ItemCore>
    | Reduce of production: string list

    static member from (conflicts:Set<ItemCore>) =
        let reduces,shifts =
            conflicts
            |> Set.partition(fun i -> i.dotmax)

        let shifts =
            if shifts.IsEmpty then
                Set.empty
            else
                let nextKernel =
                    shifts
                    |> Set.map(fun i -> i.dotIncr())
                Set.singleton (Shift nextKernel) // all to 1

        let reduces =
            reduces
            |> Set.map(fun icore -> // 1 to 1
                Reduce icore.production
            )

        let actions = reduces + shifts
        actions
