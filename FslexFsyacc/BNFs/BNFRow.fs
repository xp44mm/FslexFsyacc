namespace FslexFsyacc.BNFs

open FslexFsyacc.Precedences
open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FSharp.Idioms

type BNFRow = 
    {
    /// augmented Productions
    productions: Set<list<string>>

    grammar: Grammar

    /// (kernel:Set<ItemCore>)
    kernels: Set<Set<ItemCore>>

    kernelSymbols: Map<Set<ItemCore>,string>

    // kernel -> closure
    closures: Map<Set<ItemCore>,Set<ItemCore*Set<string>>> 

    actions: Map<Set<ItemCore>,Map<string,Set<ParseTableAction>>>

    //properConflictActions: Map<Set<ItemCore>,Map<string,Set<ParseTableAction>>>

    conflictedItemCores: Map<Set<ItemCore>,Map<string,Set<ItemCore>>>

    }

    static member from (productions: Set<list<string>>) =
        let grammar = Grammar.just productions

        let collection =
            Grammar.getKernelCollection(grammar).collection

        let kernels =
            collection
            |> Set.map(fun lalr -> SLR.from(lalr).items)

        let kernelSymbols =
            kernels
            |> Seq.map(fun kernel -> kernel, SLR.just(kernel).getSymbol())
            |> Map.ofSeq
            
        let toclosure = Grammar.kernelToClosure grammar

        let closures = 
            collection
            |> Set.map(fun k -> SLR.from(k).items, toclosure k)
            |> Map.ofSeq

        let conflictedItemCores =
            closures
            |> Map.map( fun _ closure ->
                SpreadClosure.from(closure).getConflicts()
            )

        let actions =
            conflictedItemCores
            |> Map.map( fun _ mp ->
                mp
                |> Map.map(fun _ items ->
                    SLR.just(items).toActions()
                )
            )
        {
        productions = productions
        grammar = grammar
        kernels = kernels
        kernelSymbols = kernelSymbols
        closures = closures
        actions = actions
        conflictedItemCores = conflictedItemCores
        }
