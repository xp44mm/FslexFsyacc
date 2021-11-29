namespace FslexFsyacc.Fsyacc

open FSharp.Idioms
open FslexFsyacc.Yacc

type FsyaccFile = 
    {
        header:string
        rules:(string*((string list*string*string)list))list
        precedences:(string*string list)list
        declarations:(string*string)list
    }

    static member parse(sourceText:string) =
        let header,rules,precedences,declarations = 
            FsyaccCompiler.compile sourceText
        {
            header = header
            rules = rules
            precedences = precedences
            declarations = declarations
        }

    member this.mainRules =
        //简写的产生式规范化为完整的产生式
        this.rules
        |> List.collect(fun (head,bodies)->
            bodies
            |> List.map(fun (symbols,name,semantic)->
                head::symbols,name,semantic
            )
        )

    member this.mainProductions =
        this.mainRules |> List.map Triple.first

    member this.toFsyaccParseTable() = 
        let mainRules: (string list * string) list = this.mainRules |> List.map Triple.ends

        // name -> production
        let namedProds =
            this.mainRules
            |> List.filter(fun(_,nm,_) -> nm > "")
            |> List.map(fun(prod,nm,_) -> nm,prod)
            |> Map.ofList

        let precedences: (Associativity * PrecedenceKey list) list =
            this.precedences
            |> List.map(fun(assoc,symbols)->
                let assoc =
                    match assoc with
                    | "left" -> LeftAssoc
                    | "right" -> RightAssoc
                    | "nonassoc" -> NonAssoc
                    | _ -> failwith assoc
                let keys =
                    symbols
                    |> List.map(fun symbol ->
                        if namedProds.ContainsKey symbol then
                            ProductionKey namedProds.[symbol]
                        else
                            TerminalKey symbol
                    )
                assoc,keys
            )

        let ambiguousTable = AmbiguousTable.create this.mainProductions
        let precedences = Associativity.from precedences

        /// 消除歧义，获取真正的编译行为
        let eliminate =
            EliminatingAmbiguity.eliminateActions
                ambiguousTable.productionOperators
                ambiguousTable.kernelProductions
                precedences

        // 动作无歧义的表
        let uniqueTable =
            ambiguousTable.ambiguousTable |> Set.map eliminate

        ///产生式按从小到大顺序，确定位置编码
        let productionIndexes =
            ambiguousTable.productions
            |> Set.toArray
            |> Array.mapi (fun i p -> p, i)
            |> Map.ofArray

        // 状态索引
        let kernelIndexes =
            ambiguousTable.kernels
            |> Set.toArray
            |> Array.mapi (fun i k -> k, i)
            |> Map.ofArray

        /// 具体数据编码成整数的表
        let encodeTable =
            /// 状态为正，产生式为负
            let encodeAction =
                function
                | Shift j -> kernelIndexes.[j] |> Some
                | Reduce p -> -productionIndexes.[p] |> Some
                | DeadState -> None

            uniqueTable
            |> Set.map (fun (kernel, symbol, action) -> kernelIndexes.[kernel], symbol, encodeAction action)
            |> Set.filter (Triple.last >> Option.isSome)
            |> Set.map (fun (kernel, symbol, action) -> kernel, symbol, Option.get action)
        //sactions的索引
        let productions =
            ParseTableTools.getProductionsMap ambiguousTable.productions

        let kernelSymbols =
            ambiguousTable.kernelSymbols
            |> Map.mapKey (fun kernel symbol -> kernelIndexes.[kernel])
            |> Map.map (fun i sq -> Seq.exactlyOne sq)

        let isemantics = 
            let mp = Map.ofList mainRules // prod -> semantic
            productions
            |> Map.filter (fun i prod -> i < 0 && prod.Head > "")
            |> Map.map(fun i prod -> mp.[prod])

        let declarations = 
            this.declarations
            |> List.map (fun(symbol,tp)->symbol,tp.Trim()) // " int " -> "int"

        { 
            header = this.header
            productions = productions
            actions = ParseTableTools.getActions encodeTable
            kernelSymbols = kernelSymbols
            semantics = isemantics 
            declarations = declarations
          }

