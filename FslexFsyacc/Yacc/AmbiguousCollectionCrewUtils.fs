module FslexFsyacc.Yacc.AmbiguousCollectionCrewUtils
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs
open FSharp.Idioms.Literal

// ok
let getConflictsOfClosure (closure:Set<string*ItemCore>) =
    closure
    |> Seq.groupBy fst
    |> Map.ofSeq
    |> Map.map(fun _ sq ->
        sq
        |> Seq.map snd // itemcore
        |> Set.ofSeq
    )

// ok
///此函数的结果数据是焦点，交通要道。
let getAmbiguousCollectionCrew (crew:LALRCollectionCrew) =
    let conflicts =
        crew.closures
        |> Map.map(fun i closure ->
            closure
            |> getConflictsOfClosure
        )
    AmbiguousCollectionCrew(crew,conflicts)

let newAmbiguousCollectionCrew (mainProductions:list<list<string>>) =
    mainProductions
    |> ProductionsCrewUtils.getProductionsCrew
    |> GrammarCrewUtils.getNullableCrew
    |> GrammarCrewUtils.getFirstLastCrew
    |> GrammarCrewUtils.getFollowPrecedeCrew
    |> GrammarCrewUtils.getItemCoresCrew
    |> LALRCollectionCrewUtils.getLALRCollectionCrew
    |> getAmbiguousCollectionCrew

/// 过滤掉假冲突，保留真冲突
let filterProperConflicts (this:AmbiguousCollectionCrew) =
    this.conflictedItemCores
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

let renderMembers (collectionCrew:AmbiguousCollectionCrew) =
    [
        $"let mainProductions = {stringify collectionCrew.mainProductions}"
        $"let augmentedProductions = {stringify collectionCrew.augmentedProductions}"
        $"let symbols = {stringify collectionCrew.symbols}"
        $"let nonterminals = {stringify collectionCrew.nonterminals}"
        $"let terminals = {stringify collectionCrew.terminals}"
        $"let nullables = {stringify collectionCrew.nullables}"
        $"let firsts = {stringify collectionCrew.firsts}"
        $"let lasts = {stringify collectionCrew.lasts}"
        $"let follows = {stringify collectionCrew.follows}"
        $"let precedes = {stringify collectionCrew.precedes}"

        $"let kernels = {stringify collectionCrew.kernels}"
        $"let closures = {stringify collectionCrew.closures}"
        $"let GOTOs = {stringify collectionCrew.GOTOs}"
        $"let conflictedItemCores = {stringify collectionCrew.conflictedItemCores}"

    ]

