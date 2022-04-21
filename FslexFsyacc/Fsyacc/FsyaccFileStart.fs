module FslexFsyacc.Fsyacc.FsyaccFileStart
open FSharp.Idioms

let getParentChildren (rules:(string*((string list)list))list) =
    let nonterminals = 
        rules
        |> List.map fst
        |> Set.ofList

    rules
    |> List.map(fun (lhs,rhs) -> 
        let children =
            rhs
            |> List.concat
            |> List.distinct
            |> List.filter(fun sym -> nonterminals.Contains sym)
            |> List.filter(fun sym -> sym <> lhs)
        lhs,children
    )
    |> Map.ofList

let dfsort (rules:(string*((string list)list))list) (start:string) =
    let parentChildrenMap = getParentChildren rules

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

let getProductions (rules:(string*((string list*string*string)list))list) =
    rules
    |> List.map(fun(a,ls)->a, ls|>List.map Triple.first)

let extractRules (rules:(string*((string list*string*string)list))list) (start:string) =
    let ls = 
        rules
        |> getProductions
        |> dfsort <| start
    
    let mp = rules |> Map.ofList

    let rules =
        ls
        |> List.map(fun s -> s,mp.[s])

    let productions =
        rules
        |> getProductions

    {|
        rules = rules
        symbols =
            productions
            |> List.collect(fun(lhs,rhs)->
                [
                    yield lhs
                    yield! List.concat rhs
                ]
            )
            |> Set.ofList
    |}
