namespace FslexFsyacc.Fsyacc
open FslexFsyacc.Runtime

// FsyaccFileCrew(inputText,tokens,header,rules,precedences,declarations)
type FsyaccFileCrew(inputText:string,tokens:list<Position<FsyaccToken>>,header:string,rules:list<string*list<list<string>*string*string>>,precedences:list<string*list<string>>,declarations:list<string*list<string>>) =
    member _.inputText = inputText
    member _.tokens = tokens
    member _.header = header
    member _.rules = rules
    member _.precedences = precedences
    member _.declarations = declarations

// FlatedFsyaccFileCrew(prototype,flatedRules,flatedPrecedences,flatedDeclarations)
type FlatedFsyaccFileCrew(prototype:FsyaccFileCrew,flatedRules:list<list<string>*string*string>,flatedPrecedences:Map<string,int>,flatedDeclarations:list<string*string>) =
    inherit FsyaccFileCrew(prototype.inputText,prototype.tokens,prototype.header,prototype.rules,prototype.precedences,prototype.declarations)
    member _.flatedRules = flatedRules
    member _.flatedPrecedences = flatedPrecedences
    member _.flatedDeclarations = flatedDeclarations
