module FslexFsyacc.Yacc.ProductionPrecedence

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
