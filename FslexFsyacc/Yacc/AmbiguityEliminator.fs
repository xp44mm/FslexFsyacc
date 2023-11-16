namespace FslexFsyacc.Yacc
open FslexFsyacc.Runtime

open FSharp.Idioms.Literal

type AmbiguityEliminator = 
    {
        terminals:Set<string>
        dummyTokens:Map<string list,string> // production -> dummy-token
        precedences:Map<string,int> // token -> precedence
    }

    ///获取产生式的优先级的符号:getDummyTokenOf
    member this.getPrecedence(production:string list) =
        if this.dummyTokens.ContainsKey production then
            this.dummyTokens.[production]
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
            |> Set.partition(ItemCoreUtils.dotmax)

        if reduces.IsEmpty then
            //shift/shift 经常有一个以上的符号(终结符或非终结符)，无需去重
            // ifexp : @ if b then e else e
            //       | @ if b then e
            // 所有项目将共享一个shift动作。
            itemcores
        elif shifts.IsEmpty then
            // A Reduce-Reduce error is a caused when a grammar allows two or more different rules 
            // to be reduced at the same time, for the same token. When this happens, 
            // the grammar becomes ambiguous since a program can be interpreted more than one way. 
            // This error can be caused when the same rule is reached by more than one path.
            
            // reduce/reduce 说明BNF写错了
            failwith $"R/R conflict:{stringify itemcores}"
        else
            let rdc = Seq.exactlyOne reduces //只能有一个reduce元素
            let sft = Set.minElement shifts  //可以有多个shift元素

            let rprec = this.iprec rdc.production //reduce产生式的优先级
            let sprec = this.iprec sft.production //任何shift产生式的优先级

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
                | _ -> failwith $"precedence 019: {stringify this.precedences}"

