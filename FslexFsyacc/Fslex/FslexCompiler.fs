module FslexFsyacc.Fslex.FslexCompiler

open FslexFsyacc.Runtime
open FslexFsyacc.Lex
open FslexFsyacc.Fslex.FslexTokenUtils
open FSharp.Literals.Literal

open System.Reactive
open System.Reactive.Linq

let parser = Parser<FslexToken Position>(
    FslexParseTable.rules,
    FslexParseTable.actions,
    FslexParseTable.closures,getTag,getLexeme)

let parse(tokens:seq<FslexToken Position>) =
    tokens
    |> parser.parse
    |> FslexParseTable.unboxRoot

///// 解析文本为结构化数据
//let parseToStructuralData (fslex:string) =
//    let tokens = 
//        fslex
//        |> FslexTokenUtils.tokenize 0
//        |> FslexDFA.analyze
//        |> Seq.concat
//    parse tokens

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
    let mutable result = defaultValue<_>
    let sq =
        txt
        |> FslexTokenUtils.tokenize 0
        |> FslexDFA.analyze
        |> Seq.concat
        |> Seq.map(fun tok ->
            tokens <- tok::tokens
            tok
        )
        |> Seq.map(fun lookahead ->
            match parser.tryReduce(states,lookahead) with
            | Some x -> states <- x
            | None -> ()

            states <- parser.shift(states,lookahead)
        )
    use _ = sq.Subscribe(Observer.Create(
        (fun () -> ()),
        (fun () ->
            match parser.tryReduce(states) with
            | Some x -> states <- x
            | None -> ()

            match states with
            |[1,lxm; 0,null] ->
                result <- FslexParseTable.unboxRoot lxm
            | _ ->
                failwith $"{stringify states}"
        )
    ))

    result
