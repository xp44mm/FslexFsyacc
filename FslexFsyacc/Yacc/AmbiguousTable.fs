namespace FslexFsyacc.Yacc

type AmbiguousTable =
    {
        /// 增广产生式
        productions:Set<string list>
        ambiguousTable:Set<Set<ItemCore>*string*Set<Action>>
        kernels:Set<Set<ItemCore>>
        /// 产生式对应的运算符，这个运算符可以代表产生式，查找产生式的优先级，如果有则收集在映射中。
        productionOperators: Map<string list, string>
        ///项集符号的输入符号，如果有则收集在映射中。
        kernelSymbols: Map<Set<ItemCore>,string>
        /// 项集核心对应的产生式，这个产生式的运算符就是项集符号的输入符号，如果有则收集在映射中。
        kernelProductions: Map<Set<ItemCore>,Set<string list>>
    }

    static member create(mainProductions:string list list) =
        let grammar = Grammar.from mainProductions
        let itemCores = ItemCoreFactory.make grammar.productions
        let itemCoreAttributes = 
            ItemCoreAttributeFactory.make grammar.nonterminals grammar.nullables grammar.firsts itemCores
        let closures = 
            CollectionFactory.make itemCores itemCoreAttributes grammar.productions

        let gotos = GotoFactory.make closures

        let kernelSymbols = 
            gotos
            |> Set.map(fun(_,symbol,kernel)-> kernel,symbol)

        /// Normally, the precedence of a production is taken to be the same as
        /// that of its rightmost terminal.
        // production -> symbol
        let productionOperators:Map<string list,string> =
            grammar.productions
            |> Seq.choose(fun prod ->
                prod
                |> List.tail
                |> List.tryFindBack(grammar.nonterminals.Contains>>not)
                |> Option.map(fun term -> prod,term)
            )
            |> Map.ofSeq

        ///kernel中，产生式优先级符号与kernel输入符号相同的产生式。
        let kernelProductions =
            kernelSymbols
            |> Set.filter(snd>>grammar.nonterminals.Contains>>not) // terminal
            |> Seq.choose(fun(kernel,terminal)->
                let ps =
                    kernel
                    |> Set.map(fun i -> i.production)
                    |> Set.filter productionOperators.ContainsKey
                    |> Set.filter(fun p ->
                        productionOperators.[p] = terminal
                    )
                if Set.isEmpty ps then
                    None
                else
                    Some(kernel,ps) // 有时一个符号代表不同的含义，比如分号分隔语句，也分隔列表元素
            )
            |> Map.ofSeq


        {
            productions = grammar.productions
            ambiguousTable = AmbiguousTableFactory.make closures gotos
            kernels = Set.map fst closures
            kernelSymbols = Map.ofSeq kernelSymbols
            productionOperators = productionOperators
            kernelProductions = kernelProductions
        }