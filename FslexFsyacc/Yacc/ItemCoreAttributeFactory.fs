[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.ItemCoreAttributeFactory

/// 
let make (nonterminals:Set<string>) (nullables:Set<string>) (firsts:Map<string,Set<string>>) (itemCores:Set<ItemCore>) =
    let nullable = NullableFactory.nullable nullables
    let first = FirstFactory.first nullables firsts

    itemCores
    |> Seq.choose(fun itemCore ->
        if itemCore.gone then
            None
        elif nonterminals.Contains itemCore.nextSymbol then
            let propagatable = nullable itemCore.beta
            let spontaneous = first itemCore.beta
            Some(itemCore,(propagatable,spontaneous))
        else None
    )
    |> Map.ofSeq

