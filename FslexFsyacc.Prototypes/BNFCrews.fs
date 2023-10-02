namespace FslexFsyacc.Prototypes
open FslexFsyacc.Runtime

type ProductionsCrew = {
    mainProductions:Set<string list> 
    augmentedProductions:Set<string list>
    }

type NullableCrew = {
    prototype:ProductionsCrew
    symbols:Set<string>
    nonterminals:Set<string>
    terminals:Set<string>
    nullables:Set<string>
    }

type FirstLastCrew = {
    prototype:NullableCrew
    firsts:Map<string,Set<string>>
    lasts:Map<string,Set<string>>
    }

type FollowPrecedeCrew = {
    prototype:FirstLastCrew
    follows:Map<string,Set<string>>
    precedes:Map<string,Set<string>>
    }

type ItemCoresCrew = {
    prototype:FollowPrecedeCrew
    itemCoreCrews:Map<ItemCore,ItemCoreCrew>
    }

type LALRCollectionCrew = {
    prototype:ItemCoresCrew
    kernels : Set<Set<ItemCore>> // (kernel:Set<ItemCore>)
    closures: Map<int,Set<string*ItemCore>> // index -> lookahead * action

    /// state -> (lookahead/leftside) -> kernel
    GOTOs: Map<int,Map<string,Set<ItemCore>>>
    }

type AmbiguousCollectionCrew = {
    prototype:LALRCollectionCrew
    /// state -> (lookahead/leftside) -> conflicts
    conflictedItemCores: Map<int,Map<string,Set<ItemCore>>>
    }

type ActionParseTableCrew = {
    prototype: AmbiguousCollectionCrew
    unambiguousItemCores: Map<int,Map<string,Set<ItemCore>>>
    actions: Map<int,Map<string,Action>>
    resolvedClosures: Map<int,Map<ItemCore,Set<string>>>
    }

type EncodedParseTableCrew = {
    prototype: ActionParseTableCrew
    encodedActions: list<list<string*int>>
    encodedClosures: list<list<int*int*string list>>
    }


