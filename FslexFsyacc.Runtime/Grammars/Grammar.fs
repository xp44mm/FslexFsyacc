namespace FslexFsyacc.Runtime.Grammars

type Grammar =
    {
        /// augmented Productions
        productions: Set<list<string>>
    }
    static member just(productions) = { productions = productions }

    //从文法生成增广文法
    static member from (input:list<list<string>>) =
        input
        |> ProductionUtils.augment
        |> Grammar.just

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

