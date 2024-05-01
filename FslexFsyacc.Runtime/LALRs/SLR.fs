namespace FslexFsyacc.Runtime.LALRs

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FSharp.Idioms

type SLR = 
    {
    items: Set<ItemCore>
    }

    static member just(items) = { items = items }

    static member from(lalr: Set<ItemCore*Set<string>>) =
        lalr
        |> Set.filter(fun (i,_) -> i.isKernel )
        |> Set.map fst
        |> SLR.just

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



