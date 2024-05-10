namespace FslexFsyacc.Yacc
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.ParseTables
open FslexFsyacc.Runtime.BNFs

// ActionParseTableCrew(prototype,dummyTokens,precedences,unambiguousItemCores,actions,resolvedClosures)
type ActionParseTableCrew(prototype:AmbiguousCollectionCrew,dummyTokens:Map<list<string>,string>,precedences:Map<string,int>,unambiguousItemCores:Map<int,Map<string,Set<ItemCore>>>,actions:Map<int,Map<string,Action>>,resolvedClosures:Map<int,Map<ItemCore,Set<string>>>) =
    inherit AmbiguousCollectionCrew(LALRCollectionCrew(ItemCoresCrew(FollowPrecedeCrew(FirstLastCrew(NullableCrew(ProductionsCrew(prototype.inputProductionList,prototype.startSymbol,prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts),prototype.follows,prototype.precedes),prototype.itemCoreCrews),prototype.kernels,prototype.closures,prototype.GOTOs),prototype.conflictedItemCores)
    member _.dummyTokens = dummyTokens
    member _.precedences = precedences
    member _.unambiguousItemCores = unambiguousItemCores
    member _.actions = actions
    member _.resolvedClosures = resolvedClosures

// EncodedParseTableCrew(prototype,encodedActions,encodedClosures)
type EncodedParseTableCrew(prototype:ActionParseTableCrew,encodedActions:list<list<string*int>>,encodedClosures:list<list<int*int*list<string>>>) =
    inherit ActionParseTableCrew(AmbiguousCollectionCrew(LALRCollectionCrew(ItemCoresCrew(FollowPrecedeCrew(FirstLastCrew(NullableCrew(ProductionsCrew(prototype.inputProductionList,prototype.startSymbol,prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts),prototype.follows,prototype.precedes),prototype.itemCoreCrews),prototype.kernels,prototype.closures,prototype.GOTOs),prototype.conflictedItemCores),prototype.dummyTokens,prototype.precedences,prototype.unambiguousItemCores,prototype.actions,prototype.resolvedClosures)
    member _.encodedActions = encodedActions
    member _.encodedClosures = encodedClosures

// SemanticParseTableCrew(prototype,header,rules,declarations)
type SemanticParseTableCrew(prototype:EncodedParseTableCrew,header:string,rules:list<list<string>*string>,declarations:Map<string,string>) =
    inherit EncodedParseTableCrew(ActionParseTableCrew(AmbiguousCollectionCrew(LALRCollectionCrew(ItemCoresCrew(FollowPrecedeCrew(FirstLastCrew(NullableCrew(ProductionsCrew(prototype.inputProductionList,prototype.startSymbol,prototype.mainProductions,prototype.augmentedProductions),prototype.symbols,prototype.nonterminals,prototype.terminals,prototype.nullables),prototype.firsts,prototype.lasts),prototype.follows,prototype.precedes),prototype.itemCoreCrews),prototype.kernels,prototype.closures,prototype.GOTOs),prototype.conflictedItemCores),prototype.dummyTokens,prototype.precedences,prototype.unambiguousItemCores,prototype.actions,prototype.resolvedClosures),prototype.encodedActions,prototype.encodedClosures)
    member _.header = header
    member _.rules = rules
    member _.declarations = declarations
