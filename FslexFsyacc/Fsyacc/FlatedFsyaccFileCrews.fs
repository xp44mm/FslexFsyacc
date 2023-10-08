namespace FslexFsyacc.Fsyacc
open FslexFsyacc.Runtime

// RawFsyaccFileCrew(inputText,tokens,header,rules,precedences,declarations)
type RawFsyaccFileCrew(inputText:string,tokens:list<Position<FsyaccToken>>,header:string,rules:list<string*list<list<string>*string*string>>,precedences:list<string*list<string>>,declarations:list<string*list<string>>) =
    member _.inputText = inputText
    member _.tokens = tokens
    member _.header = header
    member _.rules = rules
    member _.precedences = precedences
    member _.declarations = declarations

// FlatedFsyaccFileCrew(prototype,startSymbol,dummyTokens,flatedRules,flatedPrecedences,flatedDeclarations,mainProductionList,semanticList)
type FlatedFsyaccFileCrew(prototype:RawFsyaccFileCrew,startSymbol:string,dummyTokens:Map<list<string>,string>,flatedRules:list<list<string>*string*string>,flatedPrecedences:Map<string,int>,flatedDeclarations:Map<string,string>,mainProductionList:list<list<string>>,semanticList:list<list<string>*string>) =
    inherit RawFsyaccFileCrew(prototype.inputText,prototype.tokens,prototype.header,prototype.rules,prototype.precedences,prototype.declarations)
    member _.startSymbol = startSymbol
    member _.dummyTokens = dummyTokens
    member _.flatedRules = flatedRules
    member _.flatedPrecedences = flatedPrecedences
    member _.flatedDeclarations = flatedDeclarations
    member _.mainProductionList = mainProductionList
    member _.semanticList = semanticList
