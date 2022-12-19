module FslexFsyacc.Yacc.ProductionUtils

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

/// 产生式优先级%prec命名的提示
let precedenceOfProductions (terminals:Set<string>)(productions:Set<string list>) =
    let productions =
        productions
        |> Set.map(fun prod ->
            match
                revTerminalsOfProduction terminals prod
                |> List.truncate 1
            with rightmost -> prod,rightmost
        )
    let nonterminalProductions,terminalProductions =
        productions
        |> Seq.toList
        |> List.partition(fun(prod, maybeTerminal)->
            maybeTerminal.IsEmpty
        )
    let nonterminalProductions =
        nonterminalProductions
        |> List.map(fun(prod,_)-> 
            // head space be used to sort first
            prod," %prec is required!")
        |> Set.ofList
    let terminalProductions =
        terminalProductions
        |> List.map(fun(prod,maybeTerminal)-> prod,maybeTerminal.[0])
        |> List.groupBy(fun(prod,terminal)-> terminal)
        |> List.map(fun(terminal,items)->
            match items.Length with
            | 1 ->
                items
            | len ->
                items
                |> List.mapi(fun i (prod,term) -> 
                    let tip = $" {term} ({i} of {len})" // head space be used to sort first
                    prod, tip
                    )
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

/// 符号的孩子符号
let getNodes (productions:list<string list>) =
    productions
    |> List.groupBy List.head
    |> List.map(fun (lhs,rules) ->
        let children =
            rules
            |> List.collect List.tail // prod's body
            |> List.filter( (<>) lhs)
            |> List.distinct
        lhs,children
    )
    |> Map.ofList
