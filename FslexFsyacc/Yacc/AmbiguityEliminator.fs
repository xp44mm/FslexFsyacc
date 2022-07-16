namespace FslexFsyacc.Yacc

open FSharp.Literals.Literal

type AmbiguityEliminator = 
    {
        terminals:Set<string>
        names:Map<string list,string> // production -> dummy token
        precedences:Map<string,int> // token -> precedence
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
        let token = this.getPrecedence production
        this.precedences.[token]

    /// 删除冲突的项目
    member this.disambiguate(itemcores:Set<ItemCore>) =
        let reduces,shifts =
            itemcores
            |> Set.partition(fun i -> i.dotmax)

        if reduces.IsEmpty then
            //shifts经常有一个以上的项目，无需去重
            // ifexp : *if b then e else e
            //       | *if b then e
            itemcores
        elif shifts.IsEmpty then
        //A Reduce-Reduce error is a caused when a grammar allows two or more different rules to be reduced at the same time, for the same token.
        //When this happens, the grammar becomes ambiguous since a program can be interpreted more than one way. 
        //This error can be caused when the same rule is reached by more than one path.
            //如果有一个以上的reduces则需要去重
            failwith $"R/R conflict:{stringify itemcores}"
        else
            
            let rdc = Seq.exactlyOne reduces //只能有一个元素
            let sft = Set.minElement shifts  //可以有多个元素

            let rprec = this.iprec rdc.production
            let sprec = this.iprec sft.production // 任何产生式的优先级

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
