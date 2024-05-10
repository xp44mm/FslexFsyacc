namespace FslexFsyacc.Runtime.ParseTables

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

open FSharp.Idioms.Literal

type Yacc = 
    {
        productions:Set<list<string>>
        dummyTokens:Map<string list,string> // production -> dummy-token
        precedences:Map<string,int> // token:string -> precedence:int
    }


