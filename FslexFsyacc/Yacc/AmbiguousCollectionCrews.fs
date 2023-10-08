namespace FslexFsyacc.Yacc
open FslexFsyacc.Runtime

// ProductionsCrew(inputProductionList,startSymbol,mainProductions,augmentedProductions)
type ProductionsCrew(inputProductionList:list<list<string>>,startSymbol:string,mainProductions:Set<list<string>>,augmentedProductions:Set<list<string>>) =
    member _.inputProductionList = inputProductionList
    member _.startSymbol = startSymbol
    member _.mainProductions = mainProductions
    member _.augmentedProductions = augmentedProductions

// NullableCrew(prototype,symbols,nonterminals,terminals,nullables)
type NullableCrew(prototype:ProductionsCrew,symbols:Set<string>,nonterminals:Set<string>,terminals:Set<string>,nullables:Set<string>) =
    inherit ProductionsCrew(prototype.inputProductionList,prototype.startSymbol,prototype.mainProductions,prototype.augmentedProductions)
    member _.symbols = symbols
    member _.nonterminals = nonterminals
    member _.terminals = terminals
    member _.nullables = nullables

// FirstLastCrew(prototype,firsts,lasts)
type FirstLastCrew(prototype:NullableCrew,firsts:Map<string,Set<string>>,lasts:Map<string,Set<string>>) =
    inherit NullableCrew(ProductionsCrew(prototype.inputProductionList,prototype.startSymbol,prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables)
    member _.firsts = firsts
    member _.lasts = lasts

// FollowPrecedeCrew(prototype,follows,precedes)
type FollowPrecedeCrew(prototype:FirstLastCrew,follows:Map<string,Set<string>>,precedes:Map<string,Set<string>>) =
    inherit FirstLastCrew(NullableCrew(ProductionsCrew(prototype.inputProductionList,prototype.startSymbol,prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts)
    member _.follows = follows
    member _.precedes = precedes

// ItemCoresCrew(prototype,itemCoreCrews)
type ItemCoresCrew(prototype:FollowPrecedeCrew,itemCoreCrews:Map<ItemCore,ItemCoreCrew>) =
    inherit FollowPrecedeCrew(FirstLastCrew(NullableCrew(ProductionsCrew(prototype.inputProductionList,prototype.startSymbol,prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts),prototype.follows,prototype.precedes)
    member _.itemCoreCrews = itemCoreCrews

// LALRCollectionCrew(prototype,kernels,closures,GOTOs)
type LALRCollectionCrew(prototype:ItemCoresCrew,kernels:Set<Set<ItemCore>>,closures:Map<int,Set<string*ItemCore>>,GOTOs:Map<int,Map<string,Set<ItemCore>>>) =
    inherit ItemCoresCrew(FollowPrecedeCrew(FirstLastCrew(NullableCrew(ProductionsCrew(prototype.inputProductionList,prototype.startSymbol,prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts),prototype.follows,prototype.precedes),prototype.itemCoreCrews)
    member _.kernels = kernels
    member _.closures = closures
    member _.GOTOs = GOTOs

// AmbiguousCollectionCrew(prototype,conflictedItemCores)
type AmbiguousCollectionCrew(prototype:LALRCollectionCrew,conflictedItemCores:Map<int,Map<string,Set<ItemCore>>>) =
    inherit LALRCollectionCrew(ItemCoresCrew(FollowPrecedeCrew(FirstLastCrew(NullableCrew(ProductionsCrew(prototype.inputProductionList,prototype.startSymbol,prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts),prototype.follows,prototype.precedes),prototype.itemCoreCrews),prototype.kernels,prototype.closures,prototype.GOTOs)
    member _.conflictedItemCores = conflictedItemCores
