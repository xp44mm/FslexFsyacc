namespace Prototypes.FslexFsyacc.Fsyacc.FlatedFsyaccFileCrews
open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc

type RawFsyaccFileCrew = {
    inputText:string
    tokens: list<Position<FsyaccToken>>
    header:string
    inputRuleList:list<string*list<list<string>*string*string>>
    precedenceLines:list<string*list<string>>
    declarationLines:list<string*list<string>>
}

type FlatedFsyaccFileCrew = {
    prototype: RawFsyaccFileCrew

    flatedRules:list<list<string>*string*string>
    augmentRules:Map<list<string>,string*string>
    flatedPrecedences:Map<string,int> // symbol -> prec level
    flatedDeclarations:Map<string,string> // symbol,type
}
