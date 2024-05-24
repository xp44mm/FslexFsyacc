module FslexFsyacc.Runtime.Precedences.Conflict
open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores

/// 删除冲突的项目
let disambiguate (tryGetPrecedence: string list -> option<int*Associativity>) (actions: ParseTableAction Set) =
    match Seq.toList actions with
    | [] -> failwith "never"
    | [x] -> Some x
    | [Shift k as s; Reduce p as r] ->
        //shift的优先级
        let sprec =
            (Seq.head k).production
            |> tryGetPrecedence

        //reduce的优先级
        let rprec = tryGetPrecedence p

        match sprec,rprec with
        | _,None | None,_ -> Some s
        | Some (sprec,sassoc), Some (rprec,rassoc) ->
        // 如果没有取得iprec则用shift
        if rprec > sprec then
            Some r
        elif rprec < sprec then
            Some s
        else
            if sassoc <> rassoc then failwith "never"
            //优先级相等，在同一行，有相同结合性
            match sassoc with
            | NonAssoc -> None
            | RightAssoc -> Some s
            | LeftAssoc -> Some r
    | ls -> failwith $"{ls}"

    
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
///// 从冲突汇总产生式，以此得知哪些产生式必须指定优先级，以排除歧义。
//let collectConflictedProductions (conflicts:Map<Set<ItemCore>,Map<string,Set<ItemCore>>>) =
//    set [
//        for KeyValue(state,cnflcts) in conflicts do
//        for KeyValue(sym,tgt) in cnflcts do
//        for icore in tgt do
//        icore.production
//    ]
