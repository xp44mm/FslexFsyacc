[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.ItemCoreAttributeFactory

/// 所有ItemCore的propagatable,spontaneous属性
let make 
    (nonterminals:Set<string>) (nullables:Set<string>) (firsts:Map<string,Set<string>>) 
    (itemCores:Set<ItemCore>) =

    let nullable = NullableFactory.nullable nullables
    let first = FirstFactory.first nullables firsts

    itemCores
    |> Seq.choose(fun itemCore ->
        if itemCore.dotmax then
            None
        elif nonterminals.Contains itemCore.nextSymbol then
            let propagatable = nullable itemCore.beta
            let spontaneous = first itemCore.beta
            Some(itemCore,(propagatable,spontaneous))
        else None
    )
    |> Map.ofSeq

