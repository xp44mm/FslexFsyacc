module FslexFsyacc.Precedences.Precedence

open FSharp.Idioms
open FSharp.Idioms.Literal
open FslexFsyacc
open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FslexFsyacc.Precedences

/// Normally, the precedence of a production is taken to be the same as
/// that of its rightmost terminal.
/// 过滤终结符，并反向保存在list中。
/// todo:保存lastTerminal的索引号
let (|LastTerminalAsDummy|_|) (terminals:Set<string>) (production:string list) =
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

let (|HeadAsDummy|_|) (productions:Set<string list>) (production:string list) =
    match production with
    | hd :: _ ->
        if (
            let c =
                productions
                |> Set.filter(fun pp -> pp.Head = hd)
                |> Set.count
            c = 1 
        ) then
            Some hd // 此符号仅有一个产生式
        else
            None
    | [] -> failwith ""

type DummyData = 
    {
        productions: Set<string list>
        terminals: Set<string>
        dummyTokens: Map<string list,string>
    }

    static member just(productions,terminals,dummyTokens) = {
        productions = productions
        terminals = terminals
        dummyTokens = dummyTokens
    }

    ///获取产生式的优先级的符号:getDummyTokenOf
    member this.tryGetDummy (production:string list) =
        if this.dummyTokens.ContainsKey production then
            Some this.dummyTokens.[production]
        else
        match production with
        | HeadAsDummy this.productions dummy ->
            Some dummy
        | LastTerminalAsDummy this.terminals dummy ->
            Some dummy
        | _ -> None
            
/// 尝试获取产生式优先级编码
let tryGetPrecedence (precedences:Map<string,int*Associativity>) (maybeDummy: string option) =
    maybeDummy
    |> Option.bind(fun token ->
        if precedences.ContainsKey token then
            Some precedences.[token]
        else None
        )

///每个运算符号的优先级和相关性: fromRawYacc?
let from (operatorsLines:list<Associativity * Set<string>>) =
    operatorsLines
    |> List.mapi(fun i (assoc,operators) ->
        operators
        |> Set.map(fun op -> op,((i+1) * 100, assoc))
    )
    |> Seq.concat
    |> Map.ofSeq
        

///// 产生式优先级%prec命名的提示
//let precedenceOfProductions (terminals:Set<string>) (productions:Set<string list>) =
//    let productions =
//        productions
//        |> Set.map(fun prod ->
//            match lastTerminal terminals prod with
//            | rightmost -> prod,rightmost
//        )

//    let nonterminalProductions,terminalProductions =
//        productions
//        |> Seq.toList
//        |> List.partition(fun(prod, maybeTerminal)->
//            maybeTerminal.IsNone
//        )

//    let nonterminalProductions =
//        nonterminalProductions
//        |> List.map(fun(prod,_)-> 
//            // head space be used to sort first
//            prod," %prec is required!")

//    let terminalProductions =
//        terminalProductions
//        |> List.map(fun(prod,maybeTerminal)-> prod,maybeTerminal.Value)
//        |> List.groupBy(fun(prod,terminal)-> terminal)
//        |> List.map(fun(terminal,items)->
//            match items.Length with
//            | 1 -> items
//            | len ->
//                items
//                |> List.mapi(fun i (prod,term) -> 
//                    let tip = 
//                        // head space be used to sort first
//                        $" {term} ({i+1} of {len})"
//                    prod,tip
//                    )
//        )
//        |> List.concat
//    let productions = [
//        yield! nonterminalProductions;
//        yield! terminalProductions]
//    productions
//    |> List.sortBy snd

