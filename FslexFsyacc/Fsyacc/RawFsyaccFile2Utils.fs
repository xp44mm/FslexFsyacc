module FslexFsyacc.Fsyacc.RawFsyaccFile2Utils

let flat (raw:RawFsyaccFile2) =
    let rules =
        raw.rules
        |> FsyaccFileRules.rawToFlatRules

    let precedences =
        raw.precedences
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

    let declarations = 
        raw.declarations
        |> List.collect(fun (tp,symbols)->symbols|>List.map(fun sym -> sym,tp))

    id<FlatFsyaccFile> {
        header = raw.header
        rules = rules
        precedences = precedences
        declarations = declarations
    }

let fromFlat (flat:FlatFsyaccFile) =
    let rules =
        flat.rules
        |> FsyaccFileRules.flatToRawRules

    let precedences =
        flat.precedences
        |> FsyaccFilePrecedences.normToRawPrecedences

    let declarations = 
        flat.declarations
        |> List.groupBy(fun (sym,tp)->tp)
        |> List.map(fun (tp,groups) ->
            let symbols =
                groups |> List.map fst
            tp,symbols
        )
    id<RawFsyaccFile2>{
        rules = rules
        precedences = precedences
        header = flat.header
        declarations = declarations
    }

let render (fsyacc:RawFsyaccFile2) =
    let h = 
        fsyacc.header
        |> FsyaccFileRender.renderHeader 

    let r =
        fsyacc.rules
        |> List.map FsyaccFileRender.renderRule
        |> String.concat "\r\n"

    let p() =
        fsyacc.precedences
        |> List.map FsyaccFileRender.renderPrecedenceLine
        |> String.concat "\r\n"

    let d() =
        fsyacc.declarations
        |> List.map FsyaccFileRender.renderTypeLine
        |> String.concat "\r\n"

    // rule+prec+decl
    let rpd =
        [
            r
            if fsyacc.precedences.IsEmpty then () else p()
            if fsyacc.declarations.IsEmpty then () else d()
        ]
        |> String.concat "\r\n%%\r\n"

    [h;rpd] |> String.concat "\r\n"
