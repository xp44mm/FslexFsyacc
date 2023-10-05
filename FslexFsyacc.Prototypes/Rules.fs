namespace FslexFsyacc.Prototypes.Fsyacc.FlatedFsyaccFileCrews
open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc

type FsyaccFileCrew = {
    inputText:string
    tokens: list<Position<FsyaccToken>>
    header:string
    rules:(string*((string list*string*string)list))list
    precedences:(string*string list)list
    declarations:(string*string list)list
}

type FlatedFsyaccFileCrew = {
    prototype: FsyaccFileCrew

    flatedRules:list<string list*string*string>
    flatedPrecedences:Map<string,int> // symbol -> prec level
    flatedDeclarations:list<string*string> // symbol,type
}

type FsyaccParseTableFileCrew = {
    prototype: FlatedFsyaccFileCrew

    tblRules:(string list*string)list
    tblActions:(string*int)list list
    tblClosures:(int*int*string list)list list
    tblDeclarations:(string*string)list
}
