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

    /// 带有重复检查
    static member from (raw:RawFsyaccFile) =
        let rules = Rule.fromGroups raw.ruleGroups

        let operatorsLines =
            raw.operatorsLines
            |> List.map(fun (assoc,operators) ->
                let operators = Set.ofList operators
                assoc,operators)

        Symbol.duplOperators raw.operatorsLines

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

        Symbol.duplDeclar raw.declarationsLines

        {
            header = raw.header
            rules = rules
            operatorsLines = operatorsLines
            declarationsLines = declarationsLines
        }

    member this.unused() =
        let heads = 
            this.rules
            |> Set.map(fun rule -> rule.production)
            |> Symbol.getNonterminals

        let usedRules,unusedRules =
            this.rules
            |> Set.partition(fun rule -> rule.production.Head |> heads.Contains)

        let usedProductions =
            usedRules
            |> Set.map(fun rule -> rule.production)

        let usedDummies =
            usedRules
            |> Set.map(fun rule -> rule.dummy)
            |> Set.filter(fun dummy -> dummy > "")

        // unused operators (usedProductions, dummy)

        // unused type decl
        ()
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
