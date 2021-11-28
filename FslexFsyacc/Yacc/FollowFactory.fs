[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.FollowFactory

open FSharp.Idioms

let make (nonterminals:Set<string>) (nullables:Set<string>)(firsts:Map<string,Set<string>>) (productions:Set<string list>) =
    let startSymbol = productions.MinimumElement.[1]

    let isNonterminal symbol = nonterminals.Contains symbol
    let rightmost = NullableFactory.rightmost nullables
    let first = FirstFactory.first nullables firsts

    // 产生式体长度为1或0的，没有用，丢弃。
    let productions = 
        productions
        |> Set.remove productions.MinimumElement
        |> Set.filter(fun p -> not p.Tail.IsEmpty)

    /// 所有跟在非终结符后面的符号串
    let subsequents =
        productions
        |> Set.map(fun p -> p.Tail)
        |> Set.filter(fun body -> body.Length>1)
        |> Set.map(fun body -> // 展开产生式
            [0 .. body.Length-2]
            |> List.map(fun start -> body.[start..])
            |> Set.ofList
        )
        |> Set.unionMany
        |> Set.filter(fun body -> isNonterminal body.Head)
        |> Set.map(fun body -> body.Head, first body.Tail)
        |> Set.add (startSymbol, set[""]) //空字符串代表书中的$

    //列表中包括的是(superset, subset)对
    let pairs =
        productions
        |> Set.map(fun p ->
            rightmost p.Tail
            //|> Set.ofList
            |> Set.filter isNonterminal
            |> Set.map(fun y -> y, p.Head) //产生式右边是超集
            |> Set.filter(fun (l,r)-> l <> r)
        )
        |> Set.unionMany

    subsequents
    |> Set.unionByKey
    |> Map.ofSeq
    |> Graph.propagate <| pairs
