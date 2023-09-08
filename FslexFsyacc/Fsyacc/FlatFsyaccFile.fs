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

    ///根据startSymbol提取相关规则，无用规则被无视忽略。
    member this.start(startSymbol:string, terminals:Set<string>) =
        let rules =
            this.rules
            |> List.filter(fun (prod,_,_) ->
                // 左手边符号当作终止符，不展开，删除产生式
                prod.Head
                |> terminals.Contains
                |> not)
            |> FsyaccFileShaking.extractRules startSymbol

        let symbols =
            rules
            |> List.collect Triple.first
            |> Set.ofList

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

    ///消除给定的非终结符
    member this.eliminate(noterminal:string)=
        let re = { rules = this.rules }
        let re = re.eliminate(noterminal)
        {
            this with
                rules = re.rules
        }

