namespace FslexFsyacc.Yacc

/// SLR && LALR
type Action = 
    /// 这个shift包括龙书中的 *shift* lookahead and *goto* nonterminal
    | Shift of kernel: Set<ItemCore>
    | Reduce of production: string list
    //| Nonassoc when actions.isEmpty

    static member from (conflicts:Set<ItemCore>) =
        let reduces,shifts =
            conflicts
            |> Set.partition(fun ic->ic.dotmax)

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
                    |> Set.map(fun ic -> ic.dotIncr())
                Set.singleton (Shift nextKernel) // all to 1

        let actions = reduces + shifts
        actions

