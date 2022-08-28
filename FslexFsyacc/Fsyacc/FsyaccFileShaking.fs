module FslexFsyacc.Fsyacc.FsyaccFileShaking
open FSharp.Idioms

let getProductions (rules:list<string list*string*string>) =
    rules
    |> List.map(fun(prod,_,_)->
        match prod with
        | lhs::rhs -> lhs,rhs
        | _ -> failwith $"never"
    )

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
            |> List.distinct
            |> List.filter(fun sym -> nonterminals.Contains sym)
            |> List.filter(fun sym -> sym <> lhs)
        lhs,children
    )
    |> Map.ofList

///返回符号的深度优先顺序列表。
let deepFirstSort (productions:list<string*string list>) (start:string) =
    let parentChildrenMap = getParentChildren productions

    let rec loop (acc:string list) (handlings:list<string>)(current:string) =
        let next() =
            parentChildrenMap.[current]
            |> List.tryFind(fun x ->
                (set acc).Contains x
                |> not
            )
        match next() with
        | None -> 
            match handlings with
            | next::handling ->
                loop acc handling next
            | [] -> acc |> List.rev
        | Some next ->
            let handling = current :: handlings
            loop (next::acc) handling next

    loop [start] [] start

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
