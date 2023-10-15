module FslexFsyacc.Yacc.ProductionListUtils
open FslexFsyacc.Runtime
open FSharp.Idioms

/// 对相同lhs的rule合并，保留右手边顺序
let toRaw (productions:list<list<string>>) =
    productions
    |> List.groupBy(List.head) //lhs
    |> List.map(fun(lhs,groups)->
        let rhs =
            groups
            |> List.map(List.tail)
        lhs,rhs
    )
///分配左手边到右手边，保持规则顺序不变
let ofRaw (rawProductions:list<string*list<list<string>>>) =
    rawProductions
    |> List.collect(fun(lhs,bodies) ->
        bodies
        |> List.map(fun (body)-> lhs::body)
    )

//从文法生成增广文法
let ofMainProductions (inputProductionList:list<Production>) =
    let startSymbol = inputProductionList.[0].[0]
    let mainProductions = set inputProductionList
    mainProductions
    |> Set.add ["";startSymbol]

let getStartSymbol (augmentedProductions:Set<Production>) =
    let augProduction = 
        augmentedProductions
        |> Set.minElement
    augProduction.[1]

//productions是一个非终结符的所有产生式集合，仅有一个产生式，这个产生式或者为空，或者只有一个符号
let tryChomsky (productions:list<Production>) =
    match productions with
    | [[_] as prod] 
    | [[_;_] as prod]
        -> Some prod
    | _ -> None

/// 
let getChomsky (productions:list<Production>) =
    productions
    |> List.groupBy List.head
    |> List.map snd
    |> List.choose tryChomsky

/// 用symbol的所有产生式推导，等价消去symbol从productions中
let eliminateSymbol (symbol:string) (productions:list<string list>) =
    let removedProds,keepedProds =
        productions
        |> List.partition(fun prod ->
            prod.Head = symbol
        )
    if removedProds.IsEmpty then
        failwith "removed is terminal or no-exists."

    let bodies = 
        removedProds 
        |> List.map List.tail

    keepedProds
    |> List.collect(fun prod ->
        let body = prod.Tail
        if body |> List.exists((=) symbol) then
            body
            |> ProductionUtils.eliminateSymbol (symbol, bodies)
            |> List.map(fun body -> prod.Head::body)
        else [prod]
    )

let eliminateChomsky (productions:List<Production>) =
    let nonterminals =
        productions
        |> getChomsky
        |> Seq.map List.head

    nonterminals
    |> Seq.fold(fun productions sym -> eliminateSymbol sym productions) productions

///以start提取产生式，顺序是深度优先。
let extractSymbols (start:string) (productions:list<list<string>>) =
    //跟在一个符号后面的符号
    let follows =
        productions
        |> ProductionUtils.getNodes

    //深度优先排序的符号
    let symbols =
        (follows,start)
        ||> List.depthFirstSort
    symbols




