module FslexFsyacc.Yacc.ProductionUtils

/// Normally, the precedence of a production is taken to be the same as
/// that of its rightmost terminal.
// production -> symbol
let revTerminalsOfProduction (terminals:Set<string>) (production:string list) =
    let body = production.Tail

    let rec loop (revls:string list) ls =
        match ls with
        | [] -> revls
        | h :: t -> 
            if terminals.Contains h then
                loop (h::revls) t
            else loop revls t
    loop [] body

/// 产生式优先级%prec命名的提示
let precedenceOfProductions (terminals:Set<string>)(productions:Set<string list>) =
    let productions =
        productions
        |> Set.map(fun prod ->
            match revTerminalsOfProduction terminals prod with
            | [] -> prod, [] // ""
            | terminal::_ -> prod, [terminal]
        )

    let nonterminalProductions,terminalProductions =
        productions
        |> Seq.toList
        |> List.partition(fun(prod, maybeTerminal)->
            maybeTerminal.IsEmpty
        )

    let nonterminalProductions =
        nonterminalProductions
        |> List.map(fun(prod,_)-> prod," No exist terminal!")
        |> Set.ofList

    let terminalProductions =
        terminalProductions
        |> List.map(fun(prod,maybeTerminal)-> prod,maybeTerminal.[0])
        |> List.groupBy(fun(prod,terminal)-> terminal)
        |> List.map(fun(terminal,items)->
            if items.Length = 1 then
                items
            else
                items
                |> List.mapi(fun i (prod,term) -> prod, $" {term} {i}")
        )
        |> List.concat

    let productions = [
        yield! nonterminalProductions;
        yield! terminalProductions]
    productions
    |> List.sortBy snd

let split (prodBody:'a list) (symbol:'a):'a list list =
    let newGroups groups group =
        match group with
        | [] -> groups
        | _ -> (List.rev group)::groups

    let rec loop (groups:'a list list) (group:'a list) (body:'a list) =
        match body with
        | [] ->
            List.rev (newGroups groups group)
        | h::t ->
            if h = symbol then
                let groups = [h]::newGroups groups group
                loop groups [] t
            else
                loop groups (h::group) t

    loop [] [] prodBody

let crosspower n (body:'a list):'a list list =
    let rec loop n (acc:'a list list) =
        if n > 1 then
            let acc = 
                List.allPairs acc body
                |> List.map(fun (ls, a) -> ls @ [a])
            loop (n-1) acc
        else
            acc
    body
    |> List.map(fun a -> [a])
    |> loop n

let eliminateSymbol (symbol:string) (bodiesOfSymbol:string list list) (body:string list) =
    let splitedBody = 
        split body symbol
        |> List.mapi(fun i ls -> i,ls)

    let holes =
        splitedBody
        |> List.filter(fun(i,ls)->ls=[symbol])

    let indexesOfHoles =
        holes 
        |> List.map fst

    let holeArgsRows = 
        crosspower holes.Length bodiesOfSymbol
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
