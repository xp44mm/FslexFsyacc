module FslexFsyacc.Fsyacc.FlatedFsyaccFileCrewUtils

open FslexFsyacc.Yacc
open FSharp.Idioms

let getSemanticParseTableCrew (this:FlatedFsyaccFileCrew) =
    let mainProductionList = 
        this.flatedRules
        |> List.map Triple.first

    let dummyTokens = 
        this.flatedRules
        |> RuleListUtils.getDummyTokens

    let semanticList = 
        this.flatedRules 
        |> RuleListUtils.getSemanticRules

    let parseTable =
        EncodedParseTableCrewUtils.getEncodedParseTableCrew (
            mainProductionList,
            dummyTokens,
            this.flatedPrecedences
            )
    SemanticParseTableCrew(parseTable,this.header,semanticList,this.flatedDeclarations)

let getFlatedFsyaccFileCrew (raw:RawFsyaccFileCrew) =
    let startSymbol,_ = raw.inputRuleList.[0]
    let augmentRule = ["";startSymbol],"","s0"
    let flatedRules =
        raw.inputRuleList
        |> RuleListUtils.ofRaw

    let augmentRules =
        augmentRule::flatedRules
        |> List.map(fun(prod,dummy,semantic)-> prod,(dummy,semantic))
        |> Map.ofList

    let flatedPrecedences =
        raw.precedenceLines
        |> List.mapi(fun i (assoc,symbols) ->
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

    let flatedDeclarations = 
        raw.declarationLines
        |> List.collect(fun (tp,symbols)->symbols|>List.map(fun sym -> sym,tp))
        |> Map.ofList
    FlatedFsyaccFileCrew(raw,flatedRules,augmentRules,flatedPrecedences,flatedDeclarations)

let toFlatFsyaccFile(this:FlatedFsyaccFileCrew) =
    id<FlatFsyaccFile> {
        header = this.header
        rules = this.flatedRules
        precedences = this.flatedPrecedences
        declarations = this.flatedDeclarations
    }
