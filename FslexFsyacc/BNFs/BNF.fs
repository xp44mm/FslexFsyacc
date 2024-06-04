namespace FslexFsyacc.BNFs
open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FslexFsyacc.Precedences

type BNF =
    {
        /// augmented Productions
        productions: Set<list<string>>
    }

    static member just(productions) = { productions = productions }

    //从文法生成增广文法
    static member from (input:list<list<string>>) =
        input
        |> ProductionUtils.augment
        |> BNF.just

    member this.kernels =
        let row = BNFRowUtils.getRow this.productions
        row.kernels

    member this.kernelSymbols =
        let row = BNFRowUtils.getRow this.productions
        row.kernelSymbols

    /// to be removed
    member this.closures =
        let row = BNFRowUtils.getRow this.productions
        row.closures

    member this.actions =
        let row = BNFRowUtils.getRow this.productions
        row.actions

    /// to be removed
    member this.conflictedItemCores =
        let row = BNFRowUtils.getRow this.productions
        row.conflictedItemCores

    //下面是Grammar的成员
    member this.grammar =
        let row = BNFRowUtils.getRow this.productions
        row.grammar

    member this.symbols =
        let row = GrammarRowUtils.getRow this.productions
        row.symbols

    member this.nonterminals =
        let row = GrammarRowUtils.getRow this.productions
        row.nonterminals

    member this.terminals =
        let row = GrammarRowUtils.getRow this.productions
        row.terminals

    member this.nullables =
        let row = GrammarRowUtils.getRow this.productions
        row.nullables

    member this.firsts =
        let row = GrammarRowUtils.getRow this.productions
        row.firsts

    member this.lasts =
        let row = GrammarRowUtils.getRow this.productions
        row.lasts

    member this.follows =
        let row = GrammarRowUtils.getRow this.productions
        row.follows

    member this.precedes =
        let row = GrammarRowUtils.getRow this.productions
        row.precedes

    member this.nullable (alpha:string list) =
        NullableUtils.nullable this.nullables alpha

    member this.leftmost (alpha:string list) =        
        NullableUtils.leftmost this.nullables alpha

    member this.rightmost (alpha:string list) =
        NullableUtils.rightmost this.nullables alpha

    member this.first (alpha:string list) =
        FirstUtils.first this.nullables this.firsts alpha

    member this.getProperConflictActions() =
        [
            for KeyValue(_,mp) in this.actions do
            for KeyValue(_,acts) in mp do
            if acts.Count > 1 then List.ofSeq acts
        ]

    member bnf.getConflictedProductions() =
        [
        for acts in bnf.getProperConflictActions() do
        for act in acts do
        yield! act.getProductions()
        ]
        |> Set.ofList
