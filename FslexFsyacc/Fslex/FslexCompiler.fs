module FslexFsyacc.Fslex.FslexCompiler

open FslexFsyacc.Runtime
open FslexFsyacc.Lex
open FslexFsyacc.Fslex.FslexTokenUtils
type token = int*int*FslexToken

let parser = Parser<token>(
    FslexParseTable.rules,
    FslexParseTable.actions,
    FslexParseTable.closures,getTag,getLexeme)

let parse(tokens:seq<token>) =
    tokens
    |> parser.parse
    |> FslexParseTable.unboxRoot

/// 解析文本为结构化数据
let parseToStructuralData (fslex:string) =
    let tokens = 
        fslex
        |> FslexTokenUtils.tokenize
        |> FslexDFA.analyze
        |> Seq.concat
    parse tokens

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

