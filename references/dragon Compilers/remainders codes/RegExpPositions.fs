//3.9.2
module OptimizationLex.RegExpPositions
open Compiler
open System.Diagnostics

///转换为叶节点带标号的树
let from<'a> (re:RegularExpression<'a>) =
    let rec loop offset =
        function
        | Leaf l -> Leaf(offset,l)
        | Concat (a,b) ->
            Concat(loopLs offset [a;b])
        | Union (a,b) ->
            Union(loopLs offset [a;b])
        | Natural c1 -> Natural(loop offset c1)
        | Positive c1 -> Positive(loop offset c1)
        | Maybe c1 -> Maybe(loop offset c1)
        | extends ->failwith ""
    and loopLs offset ls =
        (offset,ls)
        |> Seq.unfold(fun (offset,ls) ->
            match ls with
            | [] -> None
            | c::tail ->
                let rp = loop offset c
                let offset = offset + (RegExpModule.leaves c).Length
                Some(rp,(offset,tail))
        )
        |> Seq.toList
        |> function [a;b] -> a,b | _ -> failwith ""
    loop 1 re

let firstpos<'a> (re:RegularExpression<int * 'a>) =
    let rec loop (re:RegularExpression<int * 'a>) =
        match re with
        | Leaf (pos,_) ->  [ pos ]
        | Union (a,b) -> [a;b] |> List.collect loop
        | Concat (a,b) ->
            let ls = [a;b]
            let lastIndex =
                ls 
                |> List.tryFindIndex(not << RegExpModule.nullable)
                |> Option.defaultValue (ls.Length-1)
            ls.[..lastIndex] |> List.collect loop
        | Natural c1
        | Positive c1 
        | Maybe c1 -> loop c1
        | _ -> failwith ""
    re |> loop |> Set.ofList

let lastpos<'a> (re:RegularExpression<int * 'a>) =
    let rec loop (re:RegularExpression<int * 'a>) =
        match re with
        | Leaf (pos,_) ->  [ pos ]
        | Union (a,b) -> loop a |> List.append <| loop b
        | Concat (a,b) ->
            let ls = [b;a]
            let lastIndex =
                ls 
                |> List.tryFindIndex(not << RegExpModule.nullable)
                |> Option.defaultValue (ls.Length-1)
            ls.[..lastIndex] |> List.collect loop
        | Natural c1
        | Positive c1 
        | Maybe c1 -> loop c1
        | _ -> failwith ""
    re |> loop |> Set.ofList

let followpos<'a>(re:RegularExpression<int*'a>) =
    let rec loop (re:RegularExpression<int*'a>) =
            match re with
            | Union (a,b) -> [a;b] |> List.collect loop
            | Concat (a,b) ->
                let ls = [a;b]
                let pairs = 
                    ls
                    |> Seq.pairwise
                    |> Seq.collect(fun(c1,c2) ->
                    [
                        for i in lastpos c1 do
                            for j in firstpos c2 do
                                yield i, j
                    ])
                [
                    yield! pairs
                    yield! ls |> List.collect loop
                ]

            | Natural c1 | Positive c1 -> // 相当于自己跟着自己
                [
                    for i in lastpos c1 do // last c1 == last re
                        for j in firstpos c1 do // first c1 == first re
                            yield i, j
                    yield! loop c1
                ]

            | Maybe c1 -> // 因为最多一个，不能自己跟着自己
                loop c1
            | Leaf _ -> []
            | _ -> failwith ""
    
    loop re
    |> List.groupBy fst
    |> Map.ofList
    |> Map.map(fun n g -> g |> List.map snd |> Set.ofList)

