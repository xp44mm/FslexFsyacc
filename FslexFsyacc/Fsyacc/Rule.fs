namespace FslexFsyacc.Fsyacc

open System
open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.BNFs
open FslexFsyacc.YACCs

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
            reducer = ""
        }

    //member this.printAugmentRule() =
    //    if this.production.Head = "" then
    //        $"{stringify this.production}, List.exactlyOne"
    //    else failwith $"{stringify this.production}"

    //member this.printMainRule(typeAnnotations:Map<string,string>) =
