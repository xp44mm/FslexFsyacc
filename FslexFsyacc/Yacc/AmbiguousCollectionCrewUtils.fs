module FslexFsyacc.Yacc.AmbiguousCollectionCrewUtils
open FslexFsyacc.Runtime

let getConflictsOfClosure (closure:Set<string*ItemCore>) =
    closure
    |> Seq.groupBy fst
    |> Map.ofSeq
    |> Map.map(fun _ sq ->
        sq
        |> Seq.map snd // itemcore
        |> Set.ofSeq
    )

///此函数的结果数据是焦点，交通要道。
let getAmbiguousCollectionCrew (crew:LALRCollectionCrew) =
    let conflicts =
        crew.closures
        |> Map.map(fun i closure ->
            closure
            |> getConflictsOfClosure
        )
    AmbiguousCollectionCrew(crew,conflicts)

/// 过滤掉假冲突，保留真冲突
let filterProperConflicts (this:AmbiguousCollectionCrew) =
    this.conflicts
    |> Map.map(fun state conflicts ->
        conflicts
        |> Map.filter(fun la items ->
            AmbiguousCollectionUtils.isConflict items
        )
    )
    |> Map.filter(fun state conflicts ->
        //内层空，外层也排除
        not conflicts.IsEmpty
    )

/// 从冲突汇总产生式，以此得知哪些产生式必须指定优先级，以排除歧义。
let collectConflictedProductions (this:AmbiguousCollectionCrew) =
    set [
        for KeyValue(state,cnflcts) in filterProperConflicts this do
        for KeyValue(sym,st) in cnflcts do
        for icore in st do
        icore.production
    ]

/// 去重，即冲突项目仅保留一个，但是grammar,kernels,gotos仍然保持去重前的数值
let toUnambiguousCollection
    (prodTokens:Map<string list,string>)
    (precedences:Map<string,int>) 
    (this:AmbiguousCollectionCrew)
    =

    let eliminator =
        {
            terminals = this.terminals
            prodTokens = prodTokens
            precedences = precedences
        }:AmbiguityEliminator

    let unambiguousClosures =
        this.conflicts
        |> Map.map(fun i closure ->
            closure
            |> Map.map(fun sym itemcores ->
                if AmbiguousCollectionUtils.isSRConflict(itemcores) then
                    eliminator.disambiguate(itemcores)
                else
                    itemcores
            )
            |> Map.filter(fun sym itemcores -> 
                // empty is nonassoc, will be error
                not itemcores.IsEmpty
                )
        )
    AmbiguousCollectionCrew(this,unambiguousClosures)

