namespace FslexFsyacc.Prototypes.Yacc.SemanticParseTableCrews

open FslexFsyacc.Prototypes.Yacc.AmbiguousCollectionCrews
open FslexFsyacc.Runtime

type ActionParseTableCrew = {
    prototype: AmbiguousCollectionCrew
    dummyTokens:Map<string list,string>
    precedences:Map<string,int>
    unambiguousItemCores: Map<int,Map<string,Set<ItemCore>>>
    actions: Map<int,Map<string,Action>>
    resolvedClosures: Map<int,Map<ItemCore,Set<string>>>
    }

type EncodedParseTableCrew = {
    prototype: ActionParseTableCrew
    encodedActions: list<list<string*int>>
    encodedClosures: list<list<int*int*string list>>
    }

type SemanticParseTableCrew = {
    prototype: EncodedParseTableCrew

    header: string
    rules:(string list*string)list
    declarations:Map<string,string>

    }


