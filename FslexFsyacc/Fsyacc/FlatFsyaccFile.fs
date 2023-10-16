namespace FslexFsyacc.Fsyacc

open System
open FSharp.Idioms
open FslexFsyacc.Yacc

type FlatFsyaccFile =
    {
        header:string
        rules:list<string list*string*string>
        // augmentRules:Map<list<string>,string*string>
        precedences:Map<string,int> // symbol -> prec level
        declarations:Map<string,string> // symbol,type
    }

