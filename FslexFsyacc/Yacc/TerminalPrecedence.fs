module FslexFsyacc.Yacc.TerminalPrecedence

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