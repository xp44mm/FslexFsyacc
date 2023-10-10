module FslexFsyacc.Fsyacc.FlatedFsyaccFileCrewUtils

open FslexFsyacc.Yacc

let getSemanticParseTableCrew (this:FlatedFsyaccFileCrew) =
    let mainProductionList = 
        this.flatedRules
        |> FsyaccFileRules.getMainProductions
    let dummyTokens = 
        this.flatedRules
        |> FsyaccFileRules.getDummyTokens 
    let semanticList = 
        this.flatedRules 
        |> FsyaccFileRules.getSemanticRules

    let parseTable =
        EncodedParseTableCrewUtils.getEncodedParseTableCrew (
            mainProductionList,
            dummyTokens,
            this.flatedPrecedences
            )
    SemanticParseTableCrew(parseTable,this.header,semanticList,this.flatedDeclarations)

let getFlatedFsyaccFileCrew (raw:RawFsyaccFileCrew) =
    let startSymbol,_ = raw.rules.[0]
    let flatedRules =
        raw.rules
        |> FsyaccFileRules.rawToFlatRules

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
    FlatedFsyaccFileCrew(raw,flatedRules,flatedPrecedences,flatedDeclarations)
let toFlatFsyaccFile(this:FlatedFsyaccFileCrew) =
    id<FlatFsyaccFile> {
        header = this.header
        rules = this.flatedRules
        precedences = this.flatedPrecedences
        declarations = this.flatedDeclarations
    }
