namespace FslexFsyacc.Prototypes.Fsyacc.FlatedFsyaccFileCrews
open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc

type RawFsyaccFileCrew = {
    inputText:string
    tokens: list<Position<FsyaccToken>>
    header:string
    inputRuleList:(string*((string list*string*string)list))list
    precedenceLines:list<string*list<string>>
    declarationLines:list<string*list<string>>
}

type FlatedFsyaccFileCrew = {
    prototype: RawFsyaccFileCrew
    augmentedRules:Map<string list,string*string>
    precedences:Map<string,int> // symbol -> prec level
    declarations:Map<string,string> // symbol,type
}
