module FslexFsyacc.Runtime.YACCs.Symbol

///获取重复的符号
let getDuplication (symbols:string list) =
    symbols
    |> List.groupBy id
    |> List.map snd
    |> List.filter(fun ls -> ls.Length > 1)
    |> List.map List.head

let duplOperators (operatorsLines: list<Associativity*string list>) =
    match
        operatorsLines
        |> List.collect snd
        |> getDuplication
    with
    | [] -> ()
    | ls -> failwith $"操作符优先级有重复：{ls}"

let duplDeclar (declarationsLines: list<string*string list>) =
    match
        declarationsLines
        |> List.collect snd
        |> getDuplication
    with
    | [] -> ()
    | ls -> failwith $"类型声明有重复：{ls}"

/// 用到的符号，没有用到的产生式
let getNonterminals (productions:Set<list<string>>) =
    let rec loop (heads:Set<string>) (productions:Set<list<string>>) =
        let nextProductions, remainProductions =
            productions
            |> Set.partition(fun p -> heads.Contains p.Head )
        if nextProductions.IsEmpty then
            heads
        else
            let nextHeads =
                nextProductions
                |> Set.map(fun p -> set p.Tail)
                |> Set.unionMany
                |> Set.union heads
            loop nextHeads remainProductions
    let heads = set [""]
    loop heads productions
        

