module FslexFsyacc.Yacc.ItemCoreUtils
open System

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ItemCores

///前进一半，留一半
let dichotomy (itemCore:ItemCore) =
    match itemCore with {production=production;dot=dot} ->
    let body = List.tail production
    let rec loop i rev (ls: _ list) =
        if i = dot then
            rev,ls
        else
            loop (i+1) (ls.Head::rev) ls.Tail
    loop 0 [] body

/// 点号紧左侧的符号，终结符，或者非终结符
/// 可以删除吗？
let prevSymbol (itemCore:ItemCore) = itemCore |> dichotomy |> fst |> List.head

/// 点号右侧的产生式体的切片
let rest (itemCore:ItemCore) = itemCore |> dichotomy |> snd

/// 点号在最右，所有符号之后
let dotmax (itemCore:ItemCore) = itemCore |> rest |> List.isEmpty

/// 点号紧右侧的符号，终结符，或者非终结符
let nextSymbol (itemCore:ItemCore) = itemCore |> rest |> List.head

// 非终结符B右侧的串，A -> alpha @ B beta 。来源于4.7.2
let beta (itemCore:ItemCore) = itemCore |> rest |> List.tail

/// 点号前进一个符号
let dotIncr (itemCore:ItemCore) = 
    {
        itemCore with
            dot = itemCore.dot + 1
    }

let isKernel (itemCore:ItemCore) =
    List.head itemCore.production = "" || itemCore.dot > 0

let render (itemCore:ItemCore) =
    RenderUtils.renderItemCore itemCore.production itemCore.dot

/// 语法中的所有ItemCore
let make (productions:Set<string list>) =
    productions
    |> Seq.collect(fun prod ->
        let body = prod.Tail
        [0..body.Length]
        |> Seq.map(fun i -> {production = prod; dot = i})
    )
    |> Set.ofSeq
