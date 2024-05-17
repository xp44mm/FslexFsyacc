namespace FslexFsyacc.Runtime.YACCs

open System
open FSharp.Idioms

open FslexFsyacc.Runtime.BNFs
open FslexFsyacc.Runtime.YACCs

type FlatFsyaccFile =
    {
        header: string        
        rules:Rule Set //augment Rules
        precedences:Map<string,int> // symbol -> prec level
        declarations:Map<string,string> // symbol,type
    }

    static member from (raw:RawFsyaccFile) =
        let startSymbol = raw.ruleGroups.[0].head
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
                        production = grp.head::bd.body
                        dummy = bd.dummy
                        reducer =  bd.reducer
                    }
                )
            )
            |> fun tail -> augmentRule :: tail

        let precedences =
            raw.operatorsLines
            |> List.mapi(fun i (assoc,operators) ->
                let iprec = (i+1) * 100 // 索引大，则优先级高
                operators
                |> List.map(fun symbol -> symbol, iprec + assoc.value)
            )
            |> List.concat
            |> Map.ofList

        let declarations = 
            raw.declarationsLines
            |> List.collect(fun (tp,symbols)->symbols |> List.map(fun sym -> sym,tp))
            |> Map.ofList

        {
            header = raw.header
            rules = Set.ofList rules
            precedences = precedences
            declarations = declarations
        }

    member this.rulesMap =
        this.rules
        |> Seq.map(fun rule -> rule.production,rule)
        |> Map.ofSeq

    member this.reducers = 
        this.rules
        |> List.ofSeq
        |> List.map(fun rule -> rule.production, rule.reducer)

    member this.getParseTable () =
        let dummyTokens = 
            this.rules
            |> Seq.filter(fun rule -> rule.dummy > "")
            |> Seq.map(fun rule -> rule.production,rule.dummy)
            |> Map.ofSeq

        let productions =
            this.rules
            |> Set.map(fun rule -> rule.production)

        //let bnf = BNF.just productions

        ParseTableRow.from( productions, dummyTokens, this.precedences )
