namespace FslexFsyacc.Yacc

type AmbiguityEliminator = 
    {
        terminals:Set<string>
        names:Map<string list,string>
        precedences:Map<string,int>
    }
    ///获取产生式的优先级的符号
    member this.getPrecedence(production:string list) =
        if this.names.ContainsKey production then
            this.names.[production]
        else
            try
                production
                |> ProductionUtils.revTerminalsOfProduction this.terminals
                |> List.head
            with _ -> failwith "production need %prec"

    ///获取产生式的优先级整数值
    member this.iprec(production:string list) =
        let prec = this.getPrecedence production
        this.precedences.[prec]

    /// 删除冲突的项目
    member this.disambiguate(itemcores:Set<ItemCore>) =
        let reduces,shifts =
            itemcores
            |> Set.partition(fun i -> i.dotmax)

        if reduces.IsEmpty then
            itemcores
        elif shifts.IsEmpty then
            failwithf "r/r conflict: %A" itemcores
        else
            let rdc = Seq.exactlyOne reduces
            let shft = Set.minElement shifts

            let rprec = this.iprec rdc.production
            let sprec = this.iprec shft.production

            if rprec > sprec then
                reduces
            elif rprec < sprec then
                shifts
            else
                match rprec % 10 with
                | 0 -> // %nonassoc
                    Set.empty
                | 1 -> // %right
                    shifts
                | 9 -> // %left
                    reduces
                | _ -> failwithf "precedence 019: %A" this.precedences
