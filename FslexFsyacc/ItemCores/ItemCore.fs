namespace FslexFsyacc.ItemCores

/// 4.6 Introduction to LR Parsing: Simple LR; 4.6.2 Items and the LR(0) Automaton
type ItemCore = 
    {
    production : string list
    dot        : int
    }
    static member just(production,dot) = { production = production; dot = dot }

    static member left(production) = ItemCore.just(production,0)

    member this.advance() = 
        if this.dotmax then
            invalidOp "ItemCore.dotmax is true"
        else
            ItemCore.just(this.production, this.dot+1)

    member this.backwards =
        let row = ItemCoreRowUtils.getRow this.production this.dot
        row.backwards

    member this.forwards =
        let row = ItemCoreRowUtils.getRow this.production this.dot
        row.forwards

    /// 点号在最右，所有符号之后
    member this.dotmax =
        let row = ItemCoreRowUtils.getRow this.production this.dot
        row.dotmax

    /// closure中某项是否为kernel项
    member this.isKernel =
        let row = ItemCoreRowUtils.getRow this.production this.dot
        row.isKernel

    /// 点号紧左侧的符号，终结符，或者非终结符
    member this.prevSymbol =
        let row = ItemCoreRowUtils.getRow this.production this.dot
        row.backwards
        |> List.head

    /// 点号紧右侧的符号，终结符，或者非终结符
    member this.nextSymbol =
        let row = ItemCoreRowUtils.getRow this.production this.dot
        row.forwards
        |> List.head

    // 来源于4.7.2, 非终结符 B 右侧的串，A -> alpha @ B beta
    member this.beta =
        let row = ItemCoreRowUtils.getRow this.production this.dot
        row.forwards
        |> List.tail

    static member spread (production:list<string>) =
        let body = production.Tail
        [0..body.Length]
        |> Seq.map(fun dot -> { production = production; dot = dot })




