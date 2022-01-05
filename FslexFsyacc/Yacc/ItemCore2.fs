namespace FslexFsyacc.Yacc
open FSharp.Idioms

type ItemCore2 =
    {
        production :string list
        dot :int
        lookaheads :string Set
    }

    static member create(mainProductions:string list list) =
        let grammar = Grammar.from mainProductions
        let itemCores = ItemCoreFactory.make grammar.productions
        let itemCoreAttributes = 
            ItemCoreAttributeFactory.make grammar.nonterminals grammar.nullables grammar.firsts itemCores
        let closures = 
            CollectionFactory.make itemCores itemCoreAttributes grammar.productions

        closures
        |> Set.toArray
        |> Array.mapi(fun i (k,closure) -> 
            let items = ItemCoreLookaheads.from(closure)
            let tbl =
                items
                |> Set.toArray
                |> Array.map(fun obj -> 
                    let prod = obj.itemCore.production
                    {
                        production = prod
                        dot = obj.itemCore.dot
                        lookaheads = obj.lookaheads
                    }
                )
            i,tbl
        )
/// 状态冲突侦测器
type ClosureConflictDetector(terminals:Set<string>,closure:ItemCore2[]) =
    // 全集
    member this.lookaheads =
        closure
        |> Array.map(fun i -> i.lookaheads)
        |> Set.unionMany
        |> Set.intersect terminals

    // 判断这个闭包是否有冲突，并保留冲突项
    member this.conflicts =
        [|
            for la in this.lookaheads do
                let laitems =
                    closure
                    |> Array.filter(fun i -> i.lookaheads.Contains la)
                if laitems.Length > 1 then
                    yield! laitems
                else ()
        |] 
        |> Set.ofArray

    //闭包冲突中的所有产生式及其优先级
    member this.productions =
        this.conflicts
        |> Set.map(fun i -> i.production)
