module FslexFsyacc.Fslex.FslexCompiler

open FslexFsyacc.Lex

/// 解析文本为结构化数据
let parseToStructuralData (fslex:string) =
    let tokens = 
        fslex
        |> FslexTokenUtils.tokenize
        |> FslexDFA2.analyze
        |> Seq.concat
    FslexParseTable.parse tokens

///获取被使用的正则定义名称
let getUsedNames
    (
        definitions: (string*RegularExpression<string>)list, 
        rules: (RegularExpression<string>list*string)list
    ) =
    let unirgxs =
        [
            for (_,rgx) in definitions do
                yield rgx
            for (rgxs,_) in rules do
                yield! rgxs
        ]

    let usednames = 
        unirgxs 
        |> List.map(LexFileNormalization.definitionNames) 
        |> Set.unionMany

    usednames


//let characterclass 
//    (
//        definitions: (string*RegularExpression<string>)list
//    ) =

//    fun def ->
//        definitions
//        |> List.pick(fun(k,v)-> if k = def then Some v else None)
//        |> LexFileNormalization.characterclass
