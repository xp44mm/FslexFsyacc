namespace FslexFsyacc.Runtime.Precedences

open FslexFsyacc.Runtime.ItemCores

/// SLR && LALR
type ParseTableAction = 
    /// 这个shift包括龙书中的 *shift* lookahead and *goto* nonterminal
    | Shift of kernel: Set<ItemCore>
    | Reduce of production: string list

    /// 
    member this.toItemCores(sym:string) =
        match this with
        | Reduce p -> set [ItemCore.just(p,p.Tail.Length),Some sym]
        | Shift k -> //当shift时候符号没有用处，舍去。
            k
            |> Set.map(fun ic -> ic.dotDecr(),None)

    member this.getProductions() =
        match this with
        | Reduce p -> Set.singleton p
        | Shift k ->
            k
            |> Set.map(fun ic -> ic.production)

