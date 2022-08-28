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

