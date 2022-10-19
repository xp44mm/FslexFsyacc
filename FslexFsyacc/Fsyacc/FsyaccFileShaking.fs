module FslexFsyacc.Fsyacc.FsyaccFileShaking

open FSharp.Idioms
open FSharp.Literals.Literal
open System

let getProductions (rules:list<string list*string*string>) =
    rules
    |> List.map(fun(prod,_,_)->
        match prod with
        | lhs::rhs -> lhs,rhs
        | _ -> failwith $"never"
    )

//不过滤终结符，其参与排序
let getParentChildren (productions:list<string*string list>) =
    //左手边符号的集合
    let nonterminals = 
        productions
        |> List.map fst
        |> Set.ofList

    productions
    |> List.groupBy fst
    |> List.map(fun (lhs,rules) ->
        //下一组符号
        let children =
            rules
            |> List.collect(fun(lhs,rhs)->rhs)
            |> List.filter(fun sym -> sym <> lhs)
            |> List.filter(fun sym -> nonterminals.Contains sym)
            |> List.distinct
        lhs,children
    )
    |> Map.ofList

///返回符号的深度优先顺序列表。
//产生式改为父子关系节点nodes Map
let deepFirstSort (productions:list<string*string list>) (start:string) =
    let parentChildrenMap = getParentChildren productions

    let rec loop (discovered:list<string>) (unfinished:list<string>) =
        //Console.WriteLine(stringify (discovered,unfinished))
        match unfinished with
        | [] -> discovered |> List.rev
        | current::tail ->
            match
                parentChildrenMap.[current]
                |> List.tryFind(fun x ->
                    discovered
                    |> Set.ofList
                    |> Set.contains x
                    |> not
                )
            with
            | Some next ->
                //发现next加入discovered
                //next加入unfinished继续查找自己的下一个
                loop (next::discovered) (next::unfinished)
            | None ->
                //currnet在next时已经加入discovered，不要重复加入discovered，丢弃即可
                loop discovered tail

    loop [start] [start]

let extractRules (rules:list<string list*string*string>) (start:string) =
    let productions = getProductions rules
    let nonterminals = deepFirstSort productions start
    
    let mp = 
        rules
        |> FsyaccFileRules.flatToRawRules
        |> Map.ofList

    //根据nonterminals的顺序排列规则
    let rules =
        nonterminals
        |> List.map(fun s -> s,mp.[s])
        |> FsyaccFileRules.rawToFlatRules

    {|
        rules = rules
        symbols =
            productions
            |> List.collect(fun(lhs,rhs)->
                [
                    lhs
                    yield! rhs
                ]
            )
            |> Set.ofList
    |}
