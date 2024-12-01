namespace FslexFsyacc.Fsyacc

open System
open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Grammars
open FslexFsyacc.Precedences
open FslexFsyacc.BNFs
open FslexFsyacc.YACCs
open FslexFsyacc.TypeArguments

type FlatFsyaccFile =
    {
        header: string        
        rules: Rule Set // augment Rules
        operatorsLines: list<Associativity * Set<string>> // symbol -> prec level
        declarationsLines: Map<TypeArgument, Set<string>>
    }

    /// 不要检查，和crawl版本比较异同
    static member from (raw:RawFsyaccFile) =
        let rules = RuleSet.fromGroups raw.ruleGroups
            
        let operatorsLines =
            raw.operatorsLines
            |> List.map(fun (assoc,operators) ->
                let operators = Set.ofList operators
                assoc,operators)

        let declarationsLines =
            raw.declarationsLines
            |> List.map(fun(targ,symbols)-> 
                TypeArgumentUtils.uniform targ, symbols
            )
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
            rules = rules
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

    member this.getBNF() =
        this.rules
        |> Set.map(fun rule -> rule.production)
        |> BNF.just

    member this.getYacc() =
        let productions =
            this.rules
            |> Set.map(fun rule -> rule.production)

        let dummyTokens = 
            this.rules
            |> Seq.filter(fun rule -> rule.dummy > "")
            |> Seq.map(fun rule -> rule.production, rule.dummy)
            |> Map.ofSeq

        let precedences =
            Precedence.from this.operatorsLines

        YaccRow.from(productions, dummyTokens, precedences)

    member this.toRaw(rules:Rule list) =
        {
            header = this.header

            ruleGroups = RuleSet.toGroups rules

            operatorsLines = 
                this.operatorsLines
                |> OperatorsLine.filterOperatorsLines rules
                |> List.map(fun (ass, syms) -> ass, List.ofSeq syms)

            declarationsLines =
                this.declarationsLines
                |> Declaration.filterTarg rules
                |> List.ofSeq
                |> List.map(fun(KeyValue(targ, syms)) -> targ, List.ofSeq syms)

        }
