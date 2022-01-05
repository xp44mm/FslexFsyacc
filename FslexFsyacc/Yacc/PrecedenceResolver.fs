module FslexFsyacc.Yacc.PrecedenceResolver

/// 已知产生式
let precedenceOfProductions (terminals:Set<string>)(productions:#seq<string list>) =
    let productions =
        productions
        |> Seq.mapi(fun i prod ->
            match ProductionUtils.revTerminalsOfProduction terminals prod with
            | [] -> i, prod, [] // ""
            | terminal::_ -> i, prod, [terminal]
        )

    let nonterminalProductions,terminalProductions =
        productions
        |> Seq.toArray
        |> Array.partition(fun(i, prod, maybeTerminal)->
            maybeTerminal.IsEmpty
        )
    let nonterminalProductions =
        nonterminalProductions
        |> Array.map(fun(i,prod,_)-> i,prod,"No prec terminal exist!")

    let terminalProductions =
        terminalProductions
        |> Array.map(fun(i,prod,maybeTerminal)-> i,prod,maybeTerminal.[0])
        |> Array.groupBy(fun(i,prod,terminal)-> terminal)
        |> Array.map(fun(terminal,items)->
            if items.Length = 1 then
                items
            else
                items
                |> Array.map(fun(i,prod,_) -> i,prod, "prec terminal conflicted.")
        )
        |> Array.concat

    // 保留产生式输入的顺序
    let productions =
        Array.concat [|nonterminalProductions;terminalProductions|]

    let indexMap =
        productions
        |> Array.mapi(fun i (j,_,_) -> i,j)
        |> Map.ofArray
        |> fun mp i -> mp.[i]
    let result =
        productions
        |> Array.permute indexMap
        |> Array.map(fun(_,prod,symbol)-> prod,symbol)
    result

/// 求解产生式的优先级
let resolvePrecOfProd 
    (productionOperators:Map<string list,string>) 
    (precedences: Map<PrecedenceKey,int>) 
    (production: string list) 
    =

    //先找是否有產生式
    if precedences.ContainsKey (ProductionKey production) then
        precedences.[ProductionKey production]
    else
        let x = productionOperators.[production]
        precedences.[TerminalKey x]

// 求解终结符的优先级
let resolvePrecOfTerminal 
    (kernelProductions:Map<Set<ItemCore>,Set<string list>>) 
    (precedences: Map<PrecedenceKey,int>) 
    (sj:Set<ItemCore>) 
    (x:string) 
    =

    //終結符號對應的產生式，一定在目標狀態的kernel核心中。kernel任何项目点号的前一个符号是此终结符号。
    let prod = Seq.exactlyOne kernelProductions.[sj]
    if precedences.ContainsKey (ProductionKey prod) then
        precedences.[ProductionKey prod]
    else
        precedences.[TerminalKey x]