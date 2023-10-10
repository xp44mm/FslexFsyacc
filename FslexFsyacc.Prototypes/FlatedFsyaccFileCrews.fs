namespace FslexFsyacc.Prototypes.Fsyacc.FlatedFsyaccFileCrews
open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc

type RawFsyaccFileCrew = {
    inputText:string
    tokens: list<Position<FsyaccToken>>
    header:string
    rules:(string*((string list*string*string)list))list
    precedenceLines:list<string*list<string>>
    declarationLines:list<string*list<string>>
}

type FlatedFsyaccFileCrew = {
    prototype: RawFsyaccFileCrew

    flatedRules:Map<string list,string*string>
    flatedPrecedences:Map<string,int> // symbol -> prec level
    flatedDeclarations:Map<string,string> // symbol,type
}
