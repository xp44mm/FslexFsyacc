namespace FslexFsyacc.Runtime.ParseTables

open System
open FSharp.Idioms

open FslexFsyacc.Runtime.BNFs
open FslexFsyacc.Runtime.ParseTables

type Rule =
    {
        production: string list
        dummy:string
        reducer:string
    }

    //member this.encode(productions: Map<string list,int>) =
