module FslexFsyacc.Runtime.Grammars.ProductionUtils

//从文法生成增广文法
let augment (input:list<list<string>>) =
    let startSymbol = input.[0].[0]
    let p0 = ["";startSymbol]

    (p0::input)
    |> Set.ofList
