namespace FslexFsyacc.Prototypes.Yacc.AmbiguousCollectionCrews
open FslexFsyacc.Prototypes.Yacc.ItemCoreCrews
open FslexFsyacc.Runtime

/// todo:only augmentedProductions
type ProductionsCrew = {
    inputProductionList:list<list<string>>
    startSymbol:string
    mainProductions:Set<list<string>>
    augmentedProductions:Set<list<string>>
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
    // flatClosures
    closures: Map<int,Set<string*ItemCore>> // kernel index -> lookahead * action

    /// kernel index -> (lookahead/leftside) -> kernel
    GOTOs: Map<int,Map<string,Set<ItemCore>>>
    }

type AmbiguousCollectionCrew = {
    prototype:LALRCollectionCrew
    /// kernel index -> (lookahead/leftside) -> conflicts
    conflictedItemCores: Map<int,Map<string,Set<ItemCore>>>
    }


