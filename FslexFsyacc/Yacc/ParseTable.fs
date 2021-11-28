namespace FslexFsyacc.Yacc

open FSharp.Idioms

type ParseTable =
    {
        productions:Map<int,string list>
        actions:Map<int,Map<string,int>>
        kernelSymbols: Map<int,string>
    }

    static member create(
                        mainProductions:string list list, 
                        precedences:(Associativity*PrecedenceKey list) list
                        ) =
        let ambiguousTable = AmbiguousTable.create mainProductions
        let precedences = Associativity.from precedences

        /// 消除歧义，获取真正的编译行为
        let eliminate = 
            EliminatingAmbiguity.eliminateActions 
                ambiguousTable.productionOperators 
                ambiguousTable.kernelProductions 
                precedences

        // 动作无歧义的表
        let uniqueTable = ambiguousTable.ambiguousTable |> Set.map eliminate

        ///产生式按从小到大顺序，确定位置编码
        let productionIndexes =
            ambiguousTable.productions
            |> Set.toArray
            |> Array.mapi(fun i p -> p,i)
            |> Map.ofArray

        // 状态索引
        let kernelIndexes =
            ambiguousTable.kernels
            |> Set.toArray
            |> Array.mapi(fun i k -> k,i)
            |> Map.ofArray

        /// 具体数据编码成整数的表
        let encodeTable =
            /// 状态为正，产生式为负
            let encodeAction =
                function
                | Shift j -> kernelIndexes.[j] |> Some
                | Reduce p -> - productionIndexes.[p] |> Some
                | DeadState -> None

            uniqueTable
            |> Set.map(fun(kernel,symbol,action)->
                kernelIndexes.[kernel],symbol,encodeAction action
            )
            |> Set.filter(Triple.last >> Option.isSome)
            |> Set.map(fun(kernel,symbol,action)->kernel,symbol,Option.get action)

        {
            productions = ParseTableTools.getProductionsMap ambiguousTable.productions
            actions = ParseTableTools.getActions encodeTable
            kernelSymbols = 
                ambiguousTable.kernelSymbols
                |> Map.mapKey(fun kernel symbol -> kernelIndexes.[kernel])
                |> Map.map(fun i sq -> Seq.exactlyOne sq)
        }


