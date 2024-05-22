namespace FslexFsyacc.Runtime.YACCs

open System
open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime.BNFs
open FslexFsyacc.Runtime.YACCs

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

    static member fromGroups (ruleGroups: RuleGroup list) =

        let ruleList =
            ruleGroups
            |> List.collect(fun grp -> 
                grp.bodies
                |> List.map(fun bd ->
                    {
                        production = grp.lhs::bd.rhs
                        dummy = bd.dummy
                        reducer =  bd.reducer
                    }
                )
            )
        //去重
        let repl =
            ruleList
            |> List.groupBy(fun rule -> rule.production)
            |> List.filter(fun (p,ls) -> ls.Length > 1)

        if repl.IsEmpty then
            let startSymbol = ruleGroups.[0].lhs
            let augmentRule = Rule.augment startSymbol
            Set.ofList (augmentRule :: ruleList)
        else
            let ps = repl |> List.map fst
            failwith $"输入有重复的产生式：{stringify ps}"

    //member this.printAugmentRule() =
    //    if this.production.Head = "" then
    //        $"{stringify this.production}, List.exactlyOne"
    //    else failwith $"{stringify this.production}"

    //member this.printMainRule(typeAnnotations:Map<string,string>) =
