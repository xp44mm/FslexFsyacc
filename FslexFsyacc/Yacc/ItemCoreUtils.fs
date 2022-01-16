module FslexFsyacc.Yacc.ItemCoreUtils

let getActionName(this:ItemCore) =
    if this.dotmax then
        "reduce"
    else
        "shift"

let getPrecedence
    (terminals:Set<string>)
    (names:Map<string list,string>)
    (this:ItemCore) =

    if names.ContainsKey this.production then
        names.[this.production]
    else
        try
            this.production
            |> ProductionUtils.revTerminalsOfProduction terminals
            |> List.head
        with _ -> failwith "production need %prec"

let iprec
    (terminals:Set<string>)
    (names:Map<string list,string>)
    (precedences:Map<string,int>)
    (this:ItemCore) =

    let prec = getPrecedence terminals names this
    precedences.[prec]

let ambiguous(terminals:Set<string>)(closure:Set<string*ItemCore>) =
    let terminalItems,nonterminalItems =
        closure
        |> Set.toArray
        |> Array.partition(fun (la,_) -> terminals.Contains la)

    let terminalItems =
        terminalItems
        |> Array.groupBy(fun (la,_) -> la)
        |> Array.map(fun(la,ls)-> la, ls |> Array.map snd |> Array.toList)

    // nextSymbol在同一状态可能重复
    let nonterminalItems =
        nonterminalItems
        |> Array.map(fun (nextSymbol,item) -> nextSymbol,[item])

    [|
        yield! terminalItems
        yield! nonterminalItems
    |] |> Map.ofArray

///// 二选一
//let disambiguate
//    (terminals:Set<string>)
//    (names:Map<string list,string>)
//    (precedences:Map<string,int>)
//    (x:ItemCore,y:ItemCore)
//    =
//    let xiprec = iprec terminals names precedences x
//    let yiprec = iprec terminals names precedences y
//    if xiprec > yiprec then
//        Some x
//    elif xiprec < yiprec then
//        Some y
//    else
//        match xiprec % 10 with
//        | 0 -> // %nonassoc
//            None
//        | deci ->
//            let shift,reduce =
//                match getActionName x, getActionName y with
//                | "shift", "reduce" -> x,y
//                | "reduce", "shift" -> y,x
//                | _ -> failwith ""
//            match deci with
//            | 1 -> // %right
//                Some shift
//            | 9 -> // %left
//                Some reduce
//            | _ -> failwithf "%A" (x,y)

