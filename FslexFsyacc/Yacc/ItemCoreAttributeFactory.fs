[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.ItemCoreAttributeFactory
open FslexFsyacc.Runtime

type LookaheadType =
    | NoBeta
    | Beta of propagatable:bool*spontaneous:Set<string>

/// 所有ItemCore的propagatable,spontaneous属性
[<System.Obsolete("getSpontaneous")>]
let make 
    (nonterminals:Set<string>) (nullables:Set<string>) (firsts:Map<string,Set<string>>) 
    (itemCores:Set<ItemCore>) =

    let nullable = NullableFactory.nullable nullables
    let first = FirstFactory.first nullables firsts

    itemCores
    |> Seq.choose(fun itemCore ->
        if ItemCoreUtils.dotmax itemCore then
            None // lookaheads将始终为空
        elif nonterminals.Contains (ItemCoreUtils.nextSymbol itemCore) then
            //原有的lookaheads是否可传播
            let propagatable = nullable (ItemCoreUtils.beta itemCore)
            //新发现的lookaheads集合
            let spontaneous = first (ItemCoreUtils.beta itemCore)
            Some(itemCore,(propagatable,spontaneous))
        else None
    )
    |> Map.ofSeq
