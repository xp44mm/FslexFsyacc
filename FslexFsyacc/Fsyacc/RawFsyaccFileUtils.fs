module FslexFsyacc.Fsyacc.RawFsyaccFileUtils
open FslexFsyacc.Runtime.YACCs

///从`*.fsyacc`文件中解析成本类型的数据
//let parse text =
//    text
//    |> FsyaccCompiler.compile

//let toFlated (raw:RawFsyaccFile) =
//    let startSymbol,_ = raw.inputRules.[0]
//    let flatedRules =
//        raw.inputRules
//        |> FlatRulesUtils.ofRaw

//    let augRule = ["";startSymbol],"","s0"

//    let augmentRules =
//        flatedRules
//        |> Set.ofList
//        |> Set.add augRule

//    let precedences =
//        raw.precedenceLines
//        |> PrecedenceLinesUtils.toMap

//    let declarations = 
//        raw.declarationLines
//        |> DeclarationLinesUtils.toMap

//    id<FlatFsyaccFile> {
//        header = raw.header
//        rules = flatedRules
//        augmentRules = augmentRules
//        precedences = precedences
//        declarations = declarations
//    }

//let fromFlat (flat:FlatFsyaccFile) =
//    let rules =
//        flat.rules
//        |> FlatRulesUtils.toRaw

//    let precedences =
//        flat.precedences
//        |> PrecedenceLinesUtils.ofMap

//    let declarations = 
//        flat.declarations
//        |> DeclarationLinesUtils.ofMap

//    id<RawFsyaccFile>{
//        inputRules = rules
//        precedenceLines = precedences
//        header = flat.header
//        declarationLines = declarations
//    }

///打印`*.fsyacc`文件
let render (fsyacc:RawFsyaccFile) =
    let h = 
        fsyacc.header
        |> RawFsyaccFileRender.renderHeader 

    let r =
        fsyacc.ruleGroups
        |> List.map(fun ruleGroup ->
            let lhs = ruleGroup.lhs
            let rhs = 
                ruleGroup.bodies
                |> List.map(fun body -> body.rhs,body.dummy,body.reducer)
            RawFsyaccFileRender.renderRule(lhs, rhs)
            )
        |> String.concat "\r\n"

    let p() =
        fsyacc.operatorsLines
        |> List.map(fun (assoc,symbols) ->
            //RawFsyaccFileRender.renderPrecedenceLine(assoc.render(),symbols)
            [
                assoc.render()
                yield! symbols 
                    |> Seq.map RawFsyaccFileRender.renderSymbol
            ]
            |> String.concat " "
            )
        |> String.concat "\r\n"

    let d() =
        fsyacc.declarationsLines
        |> List.map RawFsyaccFileRender.renderTypeLine
        |> String.concat "\r\n"

    // rule+prec+decl
    let rpd =
        [
            r
            if fsyacc.operatorsLines.IsEmpty then () else p()
            if fsyacc.declarationsLines.IsEmpty then () else d()
        ]
        |> String.concat "\r\n%%\r\n"

    [h;rpd] |> String.concat "\r\n"

