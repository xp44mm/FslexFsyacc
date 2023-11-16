module FslexFsyacc.Fsyacc.FlatedFsyaccFileCrewUtils

open FslexFsyacc.Yacc
open FSharp.Idioms

let fromRawFsyaccFileCrew (raw:RawFsyaccFileCrew) =
    let startSymbol,_ = raw.inputRuleList.[0]
    let augmentRule = ["";startSymbol],"","s0"
    let flatedRules =
        raw.inputRuleList
        |> FlatRulesUtils.ofRaw

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

let getFlatedFsyaccFileCrew (text:string) =
    text
    |> RawFsyaccFileCrewUtils.parse
    |> fromRawFsyaccFileCrew

let getMainProductions (this:FlatedFsyaccFileCrew) =
    this.flatedRules
    |> List.map Triple.first

let getAmbiguousCollectionCrew (this:FlatedFsyaccFileCrew) =
    this
    |> getMainProductions
    |> AmbiguousCollectionCrewUtils.newAmbiguousCollectionCrew

let getSemanticParseTableCrew (this:FlatedFsyaccFileCrew) =
    let mainProductionList = 
        this
        |> getMainProductions

    let dummyTokens = 
        this.flatedRules
        |> List.filter(fun (prod,dummy,act) -> dummy > "")
        |> List.map(Triple.firstTwo)
        |> Map.ofList

    let semanticList = 
        this.flatedRules 
        |> List.map(fun(x,y,z)->x,z)

    let parseTable =
        EncodedParseTableCrewUtils.getEncodedParseTableCrew (
            mainProductionList,
            dummyTokens,
            this.flatedPrecedences
            )
    SemanticParseTableCrew(parseTable,this.header,semanticList,this.flatedDeclarations)


let toFlatFsyaccFile(this:FlatedFsyaccFileCrew) =
    let augmentRule =
        this.augmentRules
        |> Map.filter(fun prod _ -> prod.Head = "")
        |> Map.toSeq
        |> Seq.map(fun (prod,(d,a)) -> prod,d,a)
        |> Seq.exactlyOne

    let augmentRules =
        this.flatedRules
        |> Set.ofList
        |> Set.add augmentRule

    id<FlatFsyaccFile> {
        header = this.header
        rules = this.flatedRules
        augmentRules = augmentRules
        precedences = this.flatedPrecedences
        declarations = this.flatedDeclarations
    }
