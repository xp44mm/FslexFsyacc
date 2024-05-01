[<RequireQualifiedAccess>]
module FslexFsyacc.Runtime.Grammars.FollowUtils

open FSharp.Idioms

let make 
    (nullables:Set<string>)
    (firsts:Map<string,Set<string>>)
    (productions:Set<string list>) =

    let startSymbol = productions.MinimumElement.[1]

    // 一个串的最右符号，终结符或非终结符
    let rightmostSymbol = NullableUtils.rightmost nullables
    // 一个串的第一个终结符号集合
    let firstTerminal = FirstUtils.first nullables firsts
        
    let productions = 
        productions
        |> Set.remove productions.MinimumElement // 移除增广产生式
        |> Set.filter(fun p -> // 移除空产生式
            let body = p.Tail
            not body.IsEmpty
        )

    // 展开产生式体
    let spreadBodies =
        productions
        |> Set.map(fun p -> p.Tail) // body
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

    // super/sub set relation pairs
    // 如果有产生式 A -> B ...， 那么 follow(B) 包含 follow(A)
    let pairs =
        productions
        |> Set.map(function
            | lhs::rhs ->
                rhs
                |> rightmostSymbol
                //|> Set.filter isNonterminal
                |> Set.map(fun symbol -> symbol,lhs)
                |> Set.filter(fun(superset,subset)-> superset <> subset)
            | p -> failwithf "%A" p
        )
        |> Set.unionMany

    subsequents
    |> Set.unionByKey
    |> Map.ofSeq
    |> Graph.propagate <| pairs
