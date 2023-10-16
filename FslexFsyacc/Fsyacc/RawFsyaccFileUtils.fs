module FslexFsyacc.Fsyacc.RawFsyaccFileUtils

///从`*.fsyacc`文件中解析成本类型的数据
let parse text =
    text
    |> FsyaccCompiler.compile

let toFlated (raw:RawFsyaccFile) =
    let startSymbol,_ = raw.inputRules.[0]
    let flatedRules =
        raw.inputRules
        |> RuleListUtils.ofRaw

    let augRule = ["";startSymbol],"","s0"

    //let augmentRules =
    //    augRule::flatedRules
    //    |> List.map(fun(prod,dummy,semantic)-> prod,(dummy,semantic))
    //    |> Map.ofList

    let precedences =
        raw.precedenceLines
        |> PrecedenceLinesUtils.toMap

    let declarations = 
        raw.declarationLines
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
        inputRules = rules
        precedenceLines = precedences
        header = flat.header
        declarationLines = declarations
    }

///打印`*.fsyacc`文件
let render (fsyacc:RawFsyaccFile) =
    let h = 
        fsyacc.header
        |> RawFsyaccFileRender.renderHeader 

    let r =
        fsyacc.inputRules
        |> List.map RawFsyaccFileRender.renderRule
        |> String.concat "\r\n"

    let p() =
        fsyacc.precedenceLines
        |> List.map RawFsyaccFileRender.renderPrecedenceLine
        |> String.concat "\r\n"

    let d() =
        fsyacc.declarationLines
        |> List.map RawFsyaccFileRender.renderTypeLine
        |> String.concat "\r\n"

    // rule+prec+decl
    let rpd =
        [
            r
            if fsyacc.precedenceLines.IsEmpty then () else p()
            if fsyacc.declarationLines.IsEmpty then () else d()
        ]
        |> String.concat "\r\n%%\r\n"

    [h;rpd] |> String.concat "\r\n"

