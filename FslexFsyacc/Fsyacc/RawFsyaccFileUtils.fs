module FslexFsyacc.Fsyacc.RawFsyaccFileUtils

///从`*.fsyacc`文件中解析成本类型的数据
let parse text =
    text
    |> FsyaccCompiler.compile

let toFlated (raw:RawFsyaccFile) =
    let startSymbol,_ = raw.rules.[0]
    let flatedRules =
        raw.rules
        |> RuleListUtils.ofRaw

    let augRule = ["";startSymbol],"","s0"

    //let augmentRules =
    //    augRule::flatedRules
    //    |> List.map(fun(prod,dummy,semantic)-> prod,(dummy,semantic))
    //    |> Map.ofList

    let precedences =
        raw.precedences
        |> PrecedenceLinesUtils.toMap

    let declarations = 
        raw.declarations
        |> DeclarationLinesUtils.toMap

    id<FlatFsyaccFile> {
        header = raw.header
        rules = flatedRules
        //augmentRules = augmentRules
        precedences = precedences
        declarations = declarations
    }

let fromFlat (flat:FlatFsyaccFile) =
    let rules =
        flat.rules
        |> RuleListUtils.toRaw

    let precedences =
        flat.precedences
        |> PrecedenceLinesUtils.ofMap

    let declarations = 
        flat.declarations
        |> DeclarationLinesUtils.ofMap

    id<RawFsyaccFile>{
        rules = rules
        precedences = precedences
        header = flat.header
        declarations = declarations
    }

///打印`*.fsyacc`文件
let render (fsyacc:RawFsyaccFile) =
    let h = 
        fsyacc.header
        |> RawFsyaccFileRender.renderHeader 

    let r =
        fsyacc.rules
        |> List.map RawFsyaccFileRender.renderRule
        |> String.concat "\r\n"

    let p() =
        fsyacc.precedences
        |> List.map RawFsyaccFileRender.renderPrecedenceLine
        |> String.concat "\r\n"

    let d() =
        fsyacc.declarations
        |> List.map RawFsyaccFileRender.renderTypeLine
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

