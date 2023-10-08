module FslexFsyacc.Fsyacc.FlatFsyaccFileUtils

open System
open System.Text
open System.Text.RegularExpressions
open System.IO

open FSharp.Idioms
open FSharp.Literals.Literal

open FslexFsyacc.Yacc

/// 将相同lhs的规则合并到一起
let normRules(this:FlatFsyaccFile) =
    let rules =
        this.rules
        |> List.groupBy (Triple.first>>List.head)
        |> List.collect snd

    {
        this with
            rules = rules
    }

///根据startSymbol提取相关规则，无用规则被无视忽略。
let start(startSymbol:string, terminals:Set<string>) (this:FlatFsyaccFile) =
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
let eliminate(noterminal:string) (this:FlatFsyaccFile) =
    let re = { rules = this.rules }
    let re = re.eliminate(noterminal)
    {
        this with
            rules = re.rules
    }

let fromRaw (raw:RawFsyaccFile) =
    raw
    |> RawFsyaccFileUtils.toFlated

let parse text =
    text
    |> RawFsyaccFileUtils.parse
    |> fromRaw

[<System.ObsoleteAttribute("getSemanticParseTableCrew")>]
let getAmbiguousCollectionCrew (fsyacc:FlatFsyaccFile) =
    fsyacc.rules
    |> FsyaccFileRules.getMainProductions
    |> GrammarCrewUtils.getProductionsCrew
    |> GrammarCrewUtils.getNullableCrew
    |> GrammarCrewUtils.getFirstLastCrew
    |> GrammarCrewUtils.getFollowPrecedeCrew
    |> GrammarCrewUtils.getItemCoresCrew
    |> LALRCollectionCrewUtils.getLALRCollectionCrew
    |> AmbiguousCollectionCrewUtils.getAmbiguousCollectionCrew

[<System.ObsoleteAttribute("getSemanticParseTableCrew")>]
let toFsyaccParseTableFile (this:FlatFsyaccFile) =
    let mainProductions = FsyaccFileRules.getMainProductions this.rules
    let dummyTokens = FsyaccFileRules.getDummyTokens this.rules
    let parseTable =
        EncodedParseTableCrewUtils.getEncodedParseTableCrew(
            mainProductions,
            dummyTokens,
            this.precedences)
    {
        header = this.header
        rules = this.rules |> FsyaccFileRules.getSemanticRules 
        declarations = Map.ofList this.declarations
        actions = parseTable.encodedActions
        closures = parseTable.encodedClosures
    }

