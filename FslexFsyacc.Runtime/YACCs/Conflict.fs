module FslexFsyacc.Runtime.YACCs.Conflict
open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

///// 过滤掉假冲突，保留真冲突
//let filterProperConflicts (conflicts:Map<Set<ItemCore>,Map<string,Set<ItemCore>>>) =
//    conflicts
//    |> Map.map(fun state conflicts ->
//        conflicts
//        |> Map.filter(fun la items ->
//            SLR.just(items).isConflict()
//            //AmbiguousCollectionUtils.isConflict items
//        )
//    )
//    |> Map.filter(fun state conflicts ->
//        //内层空，外层也排除
//        not conflicts.IsEmpty
//    )

/// 从冲突汇总产生式，以此得知哪些产生式必须指定优先级，以排除歧义。
let collectConflictedProductions (conflicts:Map<Set<ItemCore>,Map<string,Set<ItemCore>>>) =
    set [
        for KeyValue(state,cnflcts) in conflicts do
        for KeyValue(sym,tgt) in cnflcts do
        for icore in tgt do
        icore.production
    ]
