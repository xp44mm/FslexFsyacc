namespace FslexFsyacc.Runtime.YACCs

open System
open FSharp.Idioms

open FslexFsyacc.Runtime.BNFs
open FslexFsyacc.Runtime.YACCs

type FlatFsyaccFile =
    {
        header: string        
        rules: Rule Set //augment Rules
        operatorsLines: list<Associativity * Set<string>> // symbol -> prec level
        declarationsLines: Map<string, Set<string>>
    }

    static member from (raw:RawFsyaccFile) =
        let startSymbol = raw.ruleGroups.[0].lhs
        let augmentRule =
            {
                production = [""; startSymbol]
                dummy = ""
                reducer = "s0"
            }

        let rules =
            raw.ruleGroups
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
            |> fun tail -> augmentRule :: tail

        let operatorsLines =
            raw.operatorsLines
            |> List.map(fun (assoc,operators) ->
                let operators = Set.ofList operators
                assoc,operators)

        //let declarations = 
        //    raw.declarationsLines
        //    |> List.collect(fun (tp,symbols)->symbols |> List.map(fun sym -> sym,tp))
        //    |> Map.ofList

        let declarationsLines =
            raw.declarationsLines
            |> List.groupBy fst
            |> List.map(fun (tp,ls) ->
                let symbols =
                    ls
                    |> List.collect snd
                    |> Set.ofList
                tp,symbols
            )
            |> Map.ofList

        {
            header = raw.header
            rules = Set.ofList rules
            operatorsLines = operatorsLines
            declarationsLines = declarationsLines
        }

    member this.rulesMap =
        this.rules
        |> Seq.map(fun rule -> rule.production,rule)
        |> Map.ofSeq

    member this.reducers = 
        this.rules
        |> List.ofSeq
        |> List.map(fun rule -> rule.production, rule.reducer)

    member this.getYacc () =
        let productions =
            this.rules
            |> Set.map(fun rule -> rule.production)

        let dummyTokens = 
            this.rules
            |> Seq.filter(fun rule -> rule.dummy > "")
            |> Seq.map(fun rule -> rule.production,rule.dummy)
            |> Map.ofSeq

        let precedences =
            Precedence.from this.operatorsLines

        YaccRow.from( productions, dummyTokens, precedences )
