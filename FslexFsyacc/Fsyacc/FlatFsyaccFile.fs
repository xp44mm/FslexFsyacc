namespace FslexFsyacc.Fsyacc

open System
open FSharp.Idioms
open FslexFsyacc.Yacc

type FlatFsyaccFile =
    {
        rules:list<string list*string*string>
        precedences:Map<string,int> // symbol -> prec level
        header:string
        declarations:list<string*string> // symbol,type
    }

    static member fromRaw(fsyacc:RawFsyaccFile) =
        let rules =
            fsyacc.rules
            |> FsyaccFileRules.rawToFlatRules

        let precedences =
            fsyacc.precedences
            |> List.mapi(fun i (assoc,symbols)->
                let assocoffset =
                    match assoc with
                    | "left" -> -1
                    | "right" -> 1
                    | "nonassoc" -> 0
                    | _ -> failwith assoc

                let prec = (i+1) * 100 // 索引大，则优先级高
                symbols
                |> List.map(fun symbol -> symbol, prec + assocoffset)
            )
            |> List.concat
            |> Map.ofList

        {
            rules = rules
            precedences = precedences
            header = fsyacc.header
            declarations = fsyacc.declarations
        }

    /// 将相同lhs的规则合并到一起
    member this.normRules() =
        let rules =
            this.rules
            |> List.groupBy (Triple.first>>List.head)
            |> List.collect snd

        {
            this with
                rules = rules
        }

    member this.toRaw() =
        let rules =
            this.rules
            |> FsyaccFileRules.flatToRawRules
        let precedences =
            this.precedences
            |> FsyaccFilePrecedences.normToRawPrecedences

        {
            rules = rules
            precedences = precedences
            header = this.header
            declarations = this.declarations
        }:RawFsyaccFile

    ///根据startSymbol提取相关规则，无用规则被无视忽略。
    member this.start(startSymbol:string, terminals:Set<string>) =
        let rules, symbols =
            let o =
                // 有些左手边符号不展开，当作终止符
                this.rules
                |> List.filter(fun (prod,_,_) ->
                    prod.Head
                    |> terminals.Contains
                    |> not)
                |> FsyaccFileShaking.extractRules <| startSymbol
            o.rules, o.symbols

        let precedences =
            this.precedences
            |> Map.filter(fun symbol level ->
                symbols.Contains symbol
            )

        let declarations =
            this.declarations
            |> List.filter(fst>>symbols.Contains)

        {
            this with
                rules = rules
                precedences = precedences
                declarations = declarations
        }

    member this.getMainProductions() =
        this.rules |> List.map Triple.first

    member this.toFsyaccParseTableFile() =
        let mainProductions = FsyaccFileRules.getProductions this.rules
        let productionNames = FsyaccFileRules.getProductionNames this.rules
        let parseTable =
            ParseTable.create(
                mainProductions,
                productionNames,
                this.precedences)
        {
            rules = this.rules |> List.map Triple.ends
            actions = parseTable.actions
            closures = parseTable.closures
            header = this.header
            declarations = this.declarations
        }
