module FslexFsyacc.Runtime.YACCs.Precedence
open FSharp.Idioms
open FSharp.Idioms.Literal

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

///// 尝试获取优先级编码
//let tryGetPrecedenceCode tryGetDummy (precedences:Map<string,int>) (production: string list) =
//    production
//    |> tryGetDummy
//    |> Option.bind(fun token -> 
//        if precedences.ContainsKey token then
//            Some precedences.[token]
//        else None)
    
///每个运算符号的优先级和相关性
let from (operatorsLines:list<Associativity * Set<string>>) =
    operatorsLines
    |> List.mapi(fun i (assoc,operators) ->
        let prec = (i+1) * 100 // 索引大，则优先级高
        operators
        |> Set.map(fun op -> op,(prec,assoc))
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

