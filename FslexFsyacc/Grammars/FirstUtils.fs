﻿[<RequireQualifiedAccess>]
module FslexFsyacc.Grammars.FirstUtils

open FSharp.Idioms
open System.Collections.Concurrent

 /// first set 不包括空，非終結符號是否可為空，使用nullable判斷。
let make 
    (nullables:Set<string>) 
    (mainProductions:#seq<string list>) =

    let leftmost = NullableUtils.leftmost nullables
    
    // super/sub set relation pairs
    // 如果有产生式 A -> B ...， 那么 first(A) 包含 first(B)
    let pairs =
        mainProductions
        |> Seq.map(fun p ->
            leftmost p.Tail
            |> Set.map(fun y -> p.Head, y)
            |> Set.filter(fun(superset,subset)-> superset <> subset)
        )
        |> Set.unionMany

    let terminals = 
        let lefts = Set.map fst pairs
        let rights = Set.map snd pairs
        rights - lefts

    let basis =
        terminals
        |> Set.map(fun term -> term, Set.singleton term) // 终结符号
        |> Map.ofSeq

    Graph.propagate basis pairs
    |> Map.filter(fun s st -> st.Count > 1 || Set.minElement st <> s) // 只包括非终结符号

let firstLookupMemoiz = ConcurrentDictionary<
    Set<string>*Map<string,Set<string>>, 
    ConcurrentDictionary<string list, Set<string>>>(HashIdentity.Structural)

/// 符号串的first终结符集合
let first (nullables:Set<string>) (firsts:Map<string,Set<string>>) =
    let lookup = 
        firstLookupMemoiz.GetOrAdd(
            (nullables,firsts), 
            fun _ -> ConcurrentDictionary<string list, Set<string>>(HashIdentity.Structural)
            )

    let rec loop alpha =
        lookup.GetOrAdd(
            alpha,
            function
            | [] -> Set.empty
            | x :: tail ->
                let fx =
                    if firsts.ContainsKey x then
                        firsts.[x]
                    elif nullables.Contains x then
                        // x in nullables not in firsts，此符号一定只有一个空产生式。
                        Set.empty
                    else
                        // 不是文法中出现的符号，看作是终结符号，其first集合就是自己，如"$","#"
                        Set.singleton x

                let rest =
                    if nullables.Contains x then
                        loop tail
                    else Set.empty

                Set.union fx rest
        )
    loop
