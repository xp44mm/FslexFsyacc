namespace FslexFsyacc.Prototypes.Fsyacc.FlatedFsyaccFileCrews
open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc

type RawFsyaccFileCrew = {
    inputText:string
    tokens: list<Position<FsyaccToken>>
    header:string
    // inputRuleList
    rules:(string*((string list*string*string)list))list
    // precedenceLines
    precedences:(string*string list)list
    // declarationLines
    declarations:(string*string list)list
}

type FlatedFsyaccFileCrew = {
    prototype: RawFsyaccFileCrew
    startSymbol:string
    //mainProductions:Set<list<string>>
    dummyTokens:Map<list<string>,string>
    //semantics:Map<list<string>,string>

    //~~
    flatedRules:list<string list*string*string> 

    // precedences
    flatedPrecedences:Map<string,int> // symbol -> prec level
    // declarations
    flatedDeclarations:Map<string,string> // symbol,type

    //~~
    mainProductionList:list<list<string>>
    //~~
    semanticList:list<list<string>*string>

}
