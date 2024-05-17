namespace FslexFsyacc.Runtime.YACCs

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

open FSharp.Idioms.Literal

type Yacc = 
    {
        productions:Set<string list> // augment productions
        dummyTokens:Map<string list,string> // production -> dummy-token
        precedences:Map<string,int> // token:string -> precedence:int
    }


