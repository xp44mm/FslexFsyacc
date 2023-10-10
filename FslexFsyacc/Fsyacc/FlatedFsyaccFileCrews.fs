namespace FslexFsyacc.Fsyacc
open FslexFsyacc.Runtime

// RawFsyaccFileCrew(inputText,tokens,header,rules,precedenceLines,declarationLines)
type RawFsyaccFileCrew(inputText:string,tokens:list<Position<FsyaccToken>>,header:string,rules:list<string*list<list<string>*string*string>>,precedenceLines:list<string*list<string>>,declarationLines:list<string*list<string>>) =
    member _.inputText = inputText
    member _.tokens = tokens
    member _.header = header
    member _.rules = rules
    member _.precedenceLines = precedenceLines
    member _.declarationLines = declarationLines

// FlatedFsyaccFileCrew(prototype,flatedRules,flatedPrecedences,flatedDeclarations)
type FlatedFsyaccFileCrew(prototype:RawFsyaccFileCrew,flatedRules:list<list<string>*string*string>,flatedPrecedences:Map<string,int>,flatedDeclarations:Map<string,string>) =
    inherit RawFsyaccFileCrew(prototype.inputText,prototype.tokens,prototype.header,prototype.rules,prototype.precedenceLines,prototype.declarationLines)
    member _.flatedRules = flatedRules
    member _.flatedPrecedences = flatedPrecedences
    member _.flatedDeclarations = flatedDeclarations
