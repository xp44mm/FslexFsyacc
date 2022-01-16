module FslexFsyacc.Yacc.PrecedenceResolver

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