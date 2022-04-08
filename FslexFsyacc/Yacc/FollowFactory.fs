[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.FollowFactory

open FSharp.Idioms

let make 
    (nonterminals:Set<string>)
    (nullables:Set<string>)
    (firsts:Map<string,Set<string>>)
    (productions:Set<string list>) =

    let startSymbol = productions.MinimumElement.[1]

    let isNonterminal symbol = nonterminals.Contains symbol
    // 一个串的最右符号，终结符或非终结符
    let rightmostSymbol = NullableFactory.rightmost nullables
    // 一个串的第一个终结符号集合
    let firstTerminal = FirstFactory.first nullables firsts

    // 产生式体长度为1或0的，没有用，丢弃。
    let productions = 
        productions
        |> Set.remove productions.MinimumElement
        |> Set.filter(fun p -> not p.Tail.IsEmpty)

    // 展开产生式体
    let spreadBodies =
        productions
        |> Set.map(fun p -> p.Tail)
        |> Set.filter(fun body -> body.Length>1)
        |> Set.map(fun body -> 
            // 展开:(1,2,3)->[(1,2,3); (2,3); (3)]
            [0 .. body.Length-2]
            |> List.map(fun start -> body.[start..])
            |> Set.ofList
        )
        |> Set.unionMany

    // 所有跟在符号后面的首终结符号集合
    let subsequents =
        spreadBodies
        |> Set.map(fun body -> body.Head, firstTerminal body.Tail)
        //|> Set.filter(fst >> isNonterminal)
        |> Set.add (startSymbol, set [""]) //空字符串代表书中的$

    //列表中包括的是(superset, subset)对
    let pairs =
        productions
        |> Set.map(fun p ->
            p.Tail
            |> rightmostSymbol
            //|> Set.filter isNonterminal
            |> Set.map(fun nonterm -> nonterm, p.Head) //产生式右边是超集
            |> Set.filter(fun (l,r)-> l <> r)
        )
        |> Set.unionMany

    subsequents
    |> Set.unionByKey
    |> Map.ofSeq
    |> Graph.propagate <| pairs
