namespace FslexFsyacc.Yacc
// ProductionCrew(production,leftside,body)
type ProductionCrew(production:list<string>,leftside:string,body:list<string>) =
    member _.production = production
    member _.leftside = leftside
    member _.body = body
// ItemCoreCrew(prototype,dot,backwards,forwards,dotmax,isKernel)
type ItemCoreCrew(prototype:ProductionCrew,dot:int,backwards:list<string>,forwards:list<string>,dotmax:bool,isKernel:bool) =
    inherit ProductionCrew(prototype.production,prototype.leftside,prototype.body)
    member _.dot = dot
    member _.backwards = backwards
    member _.forwards = forwards
    member _.dotmax = dotmax
    member _.isKernel = isKernel
// ProductionsCrew(mainProductions,augmentedProductions)
type ProductionsCrew(mainProductions:Set<list<string>>,augmentedProductions:Set<list<string>>) =
    member _.mainProductions = mainProductions
    member _.augmentedProductions = augmentedProductions
// NullableCrew(prototype,symbols,nonterminals,terminals,nullables)
type NullableCrew(prototype:ProductionsCrew,symbols:Set<string>,nonterminals:Set<string>,terminals:Set<string>,nullables:Set<string>) =
    inherit ProductionsCrew(prototype.mainProductions,prototype.augmentedProductions)
    member _.symbols = symbols
    member _.nonterminals = nonterminals
    member _.terminals = terminals
    member _.nullables = nullables
// FirstLastCrew(prototype,firsts,lasts)
type FirstLastCrew(prototype:NullableCrew,firsts:Map<string,Set<string>>,lasts:Map<string,Set<string>>) =
    inherit NullableCrew(ProductionsCrew(prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables)
    member _.firsts = firsts
    member _.lasts = lasts
// FollowPrecedeCrew(prototype,follows,precedes)
type FollowPrecedeCrew(prototype:FirstLastCrew,follows:Map<string,Set<string>>,precedes:Map<string,Set<string>>) =
    inherit FirstLastCrew(NullableCrew(ProductionsCrew(prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts)
    member _.follows = follows
    member _.precedes = precedes
// ItemCoresCrew(prototype,itemCoreCrews)
type ItemCoresCrew(prototype:FollowPrecedeCrew,itemCoreCrews:Map<ItemCore,ItemCoreCrew>) =
    inherit FollowPrecedeCrew(FirstLastCrew(NullableCrew(ProductionsCrew(prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts),prototype.follows,prototype.precedes)
    member _.itemCoreCrews = itemCoreCrews
// LALRCollectionCrew(prototype,kernels,closures,GOTOs)
type LALRCollectionCrew(prototype:ItemCoresCrew,kernels:Set<Set<ItemCore>>,closures:Map<int,Set<string*ItemCore>>,GOTOs:Map<int,Map<string,Set<ItemCore>>>) =
    inherit ItemCoresCrew(FollowPrecedeCrew(FirstLastCrew(NullableCrew(ProductionsCrew(prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts),prototype.follows,prototype.precedes),prototype.itemCoreCrews)
    member _.kernels = kernels
    member _.closures = closures
    member _.GOTOs = GOTOs
// AmbiguousCollectionCrew(prototype,conflicts)
type AmbiguousCollectionCrew(prototype:LALRCollectionCrew,conflicts:Map<int,Map<string,Set<ItemCore>>>) =
    inherit LALRCollectionCrew(ItemCoresCrew(FollowPrecedeCrew(FirstLastCrew(NullableCrew(ProductionsCrew(prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts),prototype.follows,prototype.precedes),prototype.itemCoreCrews),prototype.kernels,prototype.closures,prototype.GOTOs)
    member _.conflicts = conflicts
// ActionParseTableCrew(prototype,actions,resolvedClosures)
type ActionParseTableCrew(prototype:AmbiguousCollectionCrew,actions:Map<int,Map<string,Action>>,resolvedClosures:Map<int,Map<ItemCore,Set<string>>>) =
    inherit AmbiguousCollectionCrew(LALRCollectionCrew(ItemCoresCrew(FollowPrecedeCrew(FirstLastCrew(NullableCrew(ProductionsCrew(prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts),prototype.follows,prototype.precedes),prototype.itemCoreCrews),prototype.kernels,prototype.closures,prototype.GOTOs),prototype.conflicts)
    member _.actions = actions
    member _.resolvedClosures = resolvedClosures
// EncodedParseTableCrew(prototype,encodedActions,encodedClosures)
type EncodedParseTableCrew(prototype:ActionParseTableCrew,encodedActions:list<list<string*int>>,encodedClosures:list<list<int*int*list<string>>>) =
    inherit ActionParseTableCrew(AmbiguousCollectionCrew(LALRCollectionCrew(ItemCoresCrew(FollowPrecedeCrew(FirstLastCrew(NullableCrew(ProductionsCrew(prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts),prototype.follows,prototype.precedes),prototype.itemCoreCrews),prototype.kernels,prototype.closures,prototype.GOTOs),prototype.conflicts),prototype.actions,prototype.resolvedClosures)
    member _.encodedActions = encodedActions
    member _.encodedClosures = encodedClosures