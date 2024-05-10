module FslexFsyacc.Runtime.ParseTables.Conflict
open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

/// 过滤掉假冲突，保留真冲突
let filterProperConflicts (conflicts:Map<Set<ItemCore>,Map<string,Set<ItemCore>>>) =
    conflicts
    |> Map.map(fun state conflicts ->
        conflicts
        |> Map.filter(fun la items ->
            SLR.just(items).isConflict()
            //AmbiguousCollectionUtils.isConflict items
        )
    )
    |> Map.filter(fun state conflicts ->
        //内层空，外层也排除
        not conflicts.IsEmpty
    )

/// 从冲突汇总产生式，以此得知哪些产生式必须指定优先级，以排除歧义。
let collectConflictedProductions (conflicts:Map<Set<ItemCore>,Map<string,Set<ItemCore>>>) =
    set [
        for KeyValue(state,cnflcts) in conflicts do
        for KeyValue(sym,tgt) in cnflcts do
        for icore in tgt do
        icore.production
    ]

///// 去重，即冲突项目仅保留一个，但是grammar,kernels,gotos仍然保持去重前的数值
//let getUnambiguousItemCores
//    (dummyTokens:Map<string list,string>)
//    (precedences:Map<string,int>)
//    (terminals:Set<string>)
//    (conflicts:Map<_,Map<string,Set<ItemCore>>>)
//    =

//    let eliminator: AmbiguityEliminator =
//        {
//            terminals = terminals
//            dummyTokens = dummyTokens
//            precedences = precedences
//        }

//    conflicts
//    |> Map.map(fun i closure ->
//        closure
//        |> Map.map(fun sym itemcores ->
//            if SLR.just(itemcores).isConflict() then
//                eliminator.disambiguate(itemcores)
//            else
//                itemcores
//        )
//        |> Map.filter(fun sym itemcores ->
//            // empty is nonassoc, will be error
//            not itemcores.IsEmpty
//            )
//    )
