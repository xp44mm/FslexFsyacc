namespace FslexFsyacc.Runtime.BNFs
open FslexFsyacc.Runtime.Grammars

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

    member this.grammar =
        let row = BNFRowUtils.getRow this.productions
        row.grammar

    member this.kernels =
        let row = BNFRowUtils.getRow this.productions
        row.kernels

    member this.closures =
        let row = BNFRowUtils.getRow this.productions
        row.closures

    member this.actions =
        let row = BNFRowUtils.getRow this.productions
        row.actions

    member this.conflictedItemCores =
        let row = BNFRowUtils.getRow this.productions
        row.conflictedItemCores

