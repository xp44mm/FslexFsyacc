module FslexFsyacc.Fslex.FslexCompiler

open FslexFsyacc.Runtime
open FslexFsyacc.Lex
open FslexFsyacc.Fslex.FslexTokenUtils
open FSharp.Literals.Literal

let parser = Parser<FslexToken Position>(
    FslexParseTable.rules,
    FslexParseTable.actions,
    FslexParseTable.closures,getTag,getLexeme)

let parse(tokens:seq<FslexToken Position>) =
    tokens
    |> parser.parse
    |> FslexParseTable.unboxRoot

/// 获取被使用的正则定义名称
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

/// 解析文本为结构化数据
let compile (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]
    //let mutable result = defaultValue<_>
    txt
    |> FslexTokenUtils.tokenize 0
    |> FslexDFA.analyze
    |> Seq.concat
    |> Seq.map(fun tok ->
        tokens <- tok::tokens
        tok
    )
    |> Seq.iter(fun lookahead ->
        //match parser.tryReduce(states,lookahead) with
        //| Some x -> states <- x
        //| None -> ()

        states <- parser.shift(states,lookahead)
    )

    //match parser.tryReduce(states) with
    //| Some x -> states <- x
    //| None -> ()

    match parser.accept states with
    | [1,lxm; 0,null] ->
        FslexParseTable.unboxRoot lxm
    | _ ->
        failwith $"{stringify states}"
