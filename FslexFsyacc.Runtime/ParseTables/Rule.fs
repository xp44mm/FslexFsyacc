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

    static member augment (startSymbol) =
        {
            production = [""; startSymbol]
            dummy = ""
            reducer = "ss.[0]"
        }

    //member this.encode(productions: Map<string list,int>) =
