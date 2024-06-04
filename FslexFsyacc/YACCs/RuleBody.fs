namespace FslexFsyacc.YACCs
open System

type RuleBody =
    {
        rhs:string list // right hand side
        dummy:string
        reducer:string
    }

