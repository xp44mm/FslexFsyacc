module FslexFsyacc.Fsyacc.Symbol
open FslexFsyacc.Precedences

///获取重复的符号
let getDuplication (symbols:string list) =
    symbols
    |> List.groupBy id
    |> List.map snd
    |> List.filter(fun ls -> ls.Length > 1)
    |> List.map List.head

let getSymbols<'a> (lines: seq<'a * Set<string>>) =
    lines
    |> Seq.map snd
    |> Set.unionMany

let duplOperators (operatorsLines: list<Associativity*string list>) =
    match
        operatorsLines
        |> List.collect snd
        |> getDuplication
    with
    | [] -> ()
    | ls -> failwith $"操作符优先级有重复：{ls}"

let duplDeclar<'a> (declarationsLines: list<'a*string list>) =
    match
        declarationsLines
        |> List.collect snd
        |> getDuplication
    with
    | [] -> ()
    | ls -> failwith $"类型声明有重复：{ls}"

