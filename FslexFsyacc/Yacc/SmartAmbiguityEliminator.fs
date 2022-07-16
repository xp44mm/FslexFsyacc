namespace FslexFsyacc.Yacc

open FSharp.Literals.Literal

type SmartAmbiguityEliminator = 
    {
        productions: string list list
        terminals:Set<string>
        names:Map<string list,string> // production -> dummy token
        precedences:Map<string,int> // token -> precedence
    }
    ///获取产生式的优先级的符记
    member this.getToken(production:string list) =
        if this.names.ContainsKey production then
            Some this.names.[production]
        else
            //try
                production
                |> ProductionUtils.revTerminalsOfProduction this.terminals
                |> List.tryHead
            //with _ -> failwith "production need %prec dummy_token"

    ///获取产生式的优先级整数值
    member this.tryGetPrec(production:string list) =
        production
        |> this.getToken
        |> Option.map(fun tok ->
            if this.precedences.ContainsKey tok then
                Some this.precedences.[tok]
            else None
        )
        |> Option.flatten

    /// 当没有优先级时，优先的项目
    member this.defaultFirst(itemcore1:ItemCore,itemcore2:ItemCore) =
            match itemcore1.dotmax, itemcore2.dotmax with
            //S/R: shift over reduce
            | true,false -> [itemcore2]
            | false,true -> [itemcore1]
            // R/R: reduce earlier rule over later rule.
            | true,true -> 
                let itemcores =
                    [
                        itemcore1
                        itemcore2
                    ]
                    |> List.sortBy(fun ic -> 
                        this.productions
                        |> List.findIndex(fun p -> p = ic.production)
                    )
                failwith $"R/R conflict:{stringify itemcores}"
            | false,false ->
                // shift 无需去重
                [itemcore1;itemcore2]








    //(b) If both rules have precedence:
    //    S/R: choose highest precedence action (precedence of reduce rule vs shift token)
    //         if same precedence: leftassoc gives reduce, rightassoc gives shift, nonassoc error.
    //    R/R: reduce the rule that comes first (textually first in the yacc file)



    ///// 删除冲突的项目
    //member this.disambiguate(itemcore1:ItemCore,itemcore2:ItemCore) =
    //    let prec1 = this.tryGetPrec itemcore1.production
    //    let prec2 = this.tryGetPrec itemcore2.production

    //    match prec1,prec2 with
    //    | Some prec1,Some prec2 ->
    //        match itemcore1.dotmax, itemcore2.dotmax with
    //        | true,false ->
    //            if prec1 > prec2 then
    //                itemcore1
    //            else
    //                itemcore2

    //        | false,true -> itemcore1

    //        | true,true -> 
    //            let pi1 = this.getProductionIndex itemcore1.production
    //            let pi2 = this.getProductionIndex itemcore2.production
    //            if pi1 < pi2 then
    //                itemcore1
    //            else
    //                itemcore2

    //        | false,false -> failwith $"r/r conflict:{itemcore1},{itemcore2}"
            

    //    | None,None ->  
    //    | None,_ -> failwith $"need prec:{itemcore1}"
    //    | _,None -> failwith $"need prec:{itemcore2}"


    //    let reduces,shifts =
    //        itemcores
    //        |> Set.partition(fun i -> i.dotmax)

    //    if reduces.IsEmpty then
    //        failwithf "r/r conflict: %A" itemcores
    //    elif shifts.IsEmpty then
    //        itemcores
    //    else

    //    let rdc = Seq.exactlyOne reduces
    //    let shft = Set.minElement shifts

    //    let rprec = this.getPrec rdc.production
    //    let sprec = this.getPrec shft.production

    //    if rprec > sprec then
    //        reduces
    //    elif rprec < sprec then
    //        shifts
    //    else
    //        match rprec % 10 with
    //        | 0 -> // %nonassoc
    //            Set.empty
    //        | 1 -> // %right
    //            shifts
    //        | 9 -> // %left
    //            reduces
    //        | _ -> failwithf "precedence 019: %A" this.precedences
