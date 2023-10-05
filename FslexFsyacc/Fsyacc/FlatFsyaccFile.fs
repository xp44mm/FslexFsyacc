namespace FslexFsyacc.Fsyacc

open System
open FSharp.Idioms
open FslexFsyacc.Yacc

type FlatFsyaccFile =
    {
        header:string
        rules:list<string list*string*string>
        precedences:Map<string,int> // symbol -> prec level
        declarations:list<string*string> // symbol,type
    }

