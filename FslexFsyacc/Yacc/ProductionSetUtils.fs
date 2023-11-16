module FslexFsyacc.Yacc.ProductionSetUtils

open FslexFsyacc.Runtime
open FSharp.Idioms

/// 用symbol的所有产生式推导，等价消去symbol从productions中
let eliminateSymbol (symbol:string) (productions:Set<string list>) =
    let removedProds,keepedProds =
        productions
        |> Set.partition(fun prod ->
            prod.Head = symbol
        )

    if removedProds.IsEmpty then
        failwith "removed is terminal or no-exists."

    /// symbol产生式的所有体，顺序无所谓
    let bodies = 
        removedProds 
        |> Set.map List.tail

    keepedProds
    |> Seq.collect(fun prod ->
        let body = prod.Tail
        if body |> List.exists((=) symbol) then
            body
            |> ProductionUtils.eliminateSymbol2 (symbol, bodies) //代入符号derive
            |> Set.map(fun body -> prod.Head::body)
        else set [prod]
    )
    |> Set.ofSeq

/// 每个产生式的头符号，与导出符号的集合
let getDerivations (productions:Set<string list>) =
    productions
    |> Set.map(fun prod ->
        match prod with
        | h::t -> h,(set t)
        | [] -> failwith "never"
    )
    |> Set.groupBy fst
    |> Map.ofSeq
    |> Map.map(fun lhs stst ->
        stst
        |> Set.map snd
        |> Set.unionMany
        |> Set.remove lhs
    )

/// 
let getSingles (productions:Set<Production>) =
    productions
    |> Set.groupBy List.head
    |> Seq.map snd
    |> Seq.choose ProductionListUtils.trySingle
    |> Set.ofSeq

/// 产生式优先级%prec命名的提示
let precedenceOfProductions (terminals:Set<string>) (productions:Set<string list>) =
    let productions =
        productions
        |> Set.map(fun prod ->
            match
                prod
                |> ProductionUtils.revTerminalsOfProduction terminals 
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

let eliminateChomsky (productions:Set<Production>) =
    let nonterminals =
        productions
        |> getSingles
        |> Seq.map List.head

    nonterminals
    |> Seq.fold(fun productions sym -> eliminateSymbol sym productions) productions

///以start提取产生式，顺序是深度优先。
let extractProductions (augmentProductions:Set<string*list<list<string>>>) =

    /// 尝试下一个符号
    let tryNextSymbol (heads:Set<string>) (revProductions:list<string*list<list<string>>>) =
        revProductions
        |> Seq.map(snd)
        |> Seq.concat
        |> Seq.concat
        |> Seq.filter(heads.Contains)
        |> Seq.tryHead

    let rec loop 
        (heads:Set<string>)
        (revProductions:list<string*list<list<string>>>)
        (remains:Set<string*list<list<string>>>)
        =
        match tryNextSymbol heads revProductions with
        | Some sym ->
            let a,remains = 
                remains
                |> Set.partition(fun (lhs,_) -> lhs = sym)
            let b = a |> Seq.exactlyOne
            loop (Set.add sym heads) (b::revProductions) remains
        | None ->
            revProductions |> List.rev
            
    let nonterminals =
        augmentProductions
        |> Set.map fst

    let st1,st2 =
        augmentProductions
        |> Set.partition(fun(lhs,_) -> lhs = "")

    loop nonterminals (Set.toList st1) st2
