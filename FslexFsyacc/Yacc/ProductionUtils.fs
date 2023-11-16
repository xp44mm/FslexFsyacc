module FslexFsyacc.Yacc.ProductionUtils
open FSharp.Idioms

let leftside (production:string list) = 
        production 
        |> List.head

let body (production:string list) = 
        production 
        |> List.tail

/// Normally, the precedence of a production is taken to be the same as
/// that of its rightmost terminal.
/// 过滤终结符，并反向保存在list中。
let revTerminalsOfProduction (terminals:Set<string>) (production:string list) =
    let rec loop (revls:string list) ls =
        match ls with
        | [] -> revls
        | h :: t -> 
            if terminals.Contains h then
                loop (h::revls) t
            else loop revls t
    loop [] production.Tail

/// 通过derive，消除产生式中的符号，产生式不能递归。
let eliminateSymbol (symbol:string, bodiesOfSymbol:string list list) (body:string list) =
    let splitedBody = 
        body
        |> List.splitBy symbol
        |> List.mapi(fun i ls -> i,ls)

    let holes =
        splitedBody
        |> List.filter(fun(i,ls)->ls=[symbol])

    let indexesOfHoles =
        holes 
        |> List.map fst

    let holeArgsRows = 
        bodiesOfSymbol
        |> List.crosspower holes.Length 
        |> List.map(fun row ->
            row
            |> List.zip indexesOfHoles
            |> Map.ofList)

    holeArgsRows
    |> List.map(fun holeArgs ->
        splitedBody
        |> List.collect(fun(i,ls)->
            if holeArgs.ContainsKey i then
                holeArgs.[i]
            else
                ls
        )
    )


/// 通过derive，消除产生式中的符号，产生式不能递归。
let eliminateSymbol2 (symbol:string, bodiesOfSymbol:Set<string list>) (body:string list) =
    //分隔产生式体，并为其编号
    let splitedBody = 
        body
        |> List.splitBy symbol
        |> List.mapi(fun i ls -> i,ls)

    let holes =
        splitedBody
        |> List.filter(fun(i,ls)->ls=[symbol])

    let indexesOfHoles =
        holes 
        |> List.map fst

    let holeArgsRows = 
        bodiesOfSymbol
        |> Set.crosspower holes.Length 
        |> Set.map(fun row ->
            row
            |> List.zip indexesOfHoles
            |> Map.ofList)

    holeArgsRows
    |> Set.map(fun holeArgs ->
        splitedBody
        |> List.collect(fun(i,ls)->
            if holeArgs.ContainsKey i then
                holeArgs.[i]
            else
                ls
        )
    )


///产生式prod不带有symbols中的任何元素
let without (symbols:Set<string>) prod =
    prod
    |> Set.ofList
    |> Set.intersect symbols
    |> Set.isEmpty

