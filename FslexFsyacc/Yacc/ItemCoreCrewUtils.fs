module FslexFsyacc.Yacc.ItemCoreCrewUtils
open FslexFsyacc.Runtime
open FSharp.Idioms.Literal

// Simple LR 的
let getItemCoreCrew(prototype:ProductionCrew,dot) =
    ///前进一半，留一半
    let backwards,forwards =
        let valueFactory(production,dot) =
            let body = List.tail production
            let rec loop i rev (ls: _ list) =
                if i = dot then
                    rev,ls
                else
                    loop (i+1) (ls.Head::rev) ls.Tail
            loop 0 [] body

        valueFactory(prototype.production,dot)

    /// 点号在最右，所有符号之后
    let dotmax = List.isEmpty forwards
    let isKernel = prototype.leftside = "" || dot > 0

    ItemCoreCrew(prototype,dot,backwards,forwards,dotmax,isKernel)

/// 点号紧左侧的符号，终结符，或者非终结符
/// 可以删除吗？
let getPrevSymbol (itemCore:ItemCoreCrew) =
    itemCore.backwards
    |> List.head

/// 点号紧右侧的符号，终结符，或者非终结符
let getNextSymbol (itemCore:ItemCoreCrew) =
    itemCore.forwards
    |> List.head

// 非终结符B右侧的串，A -> alpha @ B beta 。来源于4.7.2
let getBeta (itemCore:ItemCoreCrew) =
    itemCore.forwards
    |> List.tail

/// 点号前进一个符号
let dotIncr(itemCore:ItemCoreCrew) =
    let nextDichotomy (backwards:string list,forwards:string list) =
        forwards.Head::backwards, forwards.Tail
    nextDichotomy (itemCore.backwards,itemCore.forwards)

