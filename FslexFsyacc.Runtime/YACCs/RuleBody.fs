namespace FslexFsyacc.Runtime.YACCs
open System

type RuleBody =
    {
        rhs:string list // right hand side
        dummy:string
        reducer:string
    }

