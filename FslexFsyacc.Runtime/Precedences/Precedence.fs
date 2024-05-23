module FslexFsyacc.Runtime.Precedences.Precedence
open FSharp.Idioms
open FSharp.Idioms.Literal
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.Precedences

/// Normally, the precedence of a production is taken to be the same as
/// that of its rightmost terminal.
/// 过滤终结符，并反向保存在list中。
let lastTerminal (terminals:Set<string>) (production:string list) =
    // rev & filter terminal 
    let rec revFilterTerminals (revls:string list) ls =
        match ls with
        | [] -> revls
        | h :: t -> 
            if terminals.Contains h then
                revFilterTerminals (h::revls) t
            else 
                revFilterTerminals revls t
    production.Tail
    |> revFilterTerminals []
    |> Seq.tryHead

///获取产生式的优先级的符号:getDummyTokenOf
let tryGetDummy (dummyTokens:Map<string list,string>) (terminals:Set<string>) (production:string list) =
    if dummyTokens.ContainsKey production then
        Some dummyTokens.[production]
    else
        lastTerminal terminals production
            
///每个运算符号的优先级和相关性
let from (operatorsLines:list<Associativity * Set<string>>) =
    operatorsLines
    |> List.mapi(fun i (assoc,operators) ->
        operators
        |> Set.map(fun op -> op,((i+1) * 100, assoc))
    )
    |> Seq.concat
    |> Map.ofSeq

/// 尝试获取产生式优先级编码
let tryGetPrecedence tryGetDummy (precedences:Map<string,int*Associativity>) (production: string list) =
    production
    |> tryGetDummy
    |> Option.bind(fun token -> 
        if precedences.ContainsKey token then
            Some precedences.[token]
        else None)

/// 产生式优先级%prec命名的提示
let precedenceOfProductions (terminals:Set<string>) (productions:Set<string list>) =
    let productions =
        productions
        |> Set.map(fun prod ->
            match lastTerminal terminals prod with
            | rightmost -> prod,rightmost
        )
    let nonterminalProductions,terminalProductions =
        productions
        |> Seq.toList
        |> List.partition(fun(prod, maybeTerminal)->
            maybeTerminal.IsNone
        )

    let nonterminalProductions =
        nonterminalProductions
        |> List.map(fun(prod,_)-> 
            // head space be used to sort first
            prod," %prec is required!")

    let terminalProductions =
        terminalProductions
        |> List.map(fun(prod,maybeTerminal)-> prod,maybeTerminal.Value)
        |> List.groupBy(fun(prod,terminal)-> terminal)
        |> List.map(fun(terminal,items)->
            match items.Length with
            | 1 -> items
            | len ->
                items
                |> List.mapi(fun i (prod,term) -> 
                    let tip = 
                        // head space be used to sort first
                        $" {term} ({i+1} of {len})"
                    prod,tip
                    )
        )
        |> List.concat
    let productions = [
        yield! nonterminalProductions;
        yield! terminalProductions]
    productions
    |> List.sortBy snd

