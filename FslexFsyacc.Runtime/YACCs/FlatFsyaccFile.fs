namespace FslexFsyacc.Runtime.YACCs

open System
open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.Precedences
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
        let rules = RuleSet.fromGroups raw.ruleGroups
            
        RuleSet.redundantDummies rules

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

    /// 打印未使用的规则，操作符，类型声明等。
    member this.unusedReport() =
        let symbols = 
            this.rules
            |> RuleSet.getUsedSymbols ""

        let usedRules, unusedRules =
            this.rules
            |> Set.partition(fun rule -> rule.production.Head |> symbols.Contains)

        let usedProductions =
            usedRules
            |> Set.map(fun rule -> rule.production)

        let bnf = BNF.just usedProductions

        let usedDummies =
            usedRules
            |> Set.map(fun rule -> rule.dummy)
            |> Set.filter(fun dummy -> dummy > "")

        let operators =
            this.operatorsLines
            |> List.map snd
            |> Set.unionMany

        let unusedOperators =
            operators - bnf.terminals - usedDummies

        let unusedDummies =
            usedDummies - operators

        let typeDecls =
            this.declarationsLines
            |> Map.values
            |> Set.unionMany

        // unused operators (usedProductions, dummy)
        let unusedTypeDecls =
            typeDecls - bnf.symbols
        [
            "# unused report"
            "## unused rules"
            stringify unusedRules
            "## unusedOperators"
            stringify unusedOperators
            "## unusedDummies"
            stringify unusedDummies
            "## unusedTypeDecls"
            stringify unusedTypeDecls
        ]
        |> String.concat "\n"

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

