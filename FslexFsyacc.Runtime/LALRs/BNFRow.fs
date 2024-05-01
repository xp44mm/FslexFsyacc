namespace FslexFsyacc.Runtime.LALRs

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FSharp.Idioms

type BNFRow = 
    {
    /// augmented Productions
    productions: Set<list<string>>

    grammar: Grammar

    /// (kernel:Set<ItemCore>)
    kernels: Set<Set<ItemCore>>

    closures: Map<Set<ItemCore>,Set<ItemCore*Set<string>>>

    /// kernel -> symbol(T/N) -> Actions
    actions: Map<Set<ItemCore>,Map<string,Set<Action>>>

    }

    static member from(productions: Set<list<string>>) =
        let grammar = Grammar.just productions

        let collection =
            Grammar.getKernelCollection(grammar).collection

        let kernels =
            collection
            |> Set.map(fun lalr -> SLR.from(lalr).items)

        let toclosure = Grammar.kernelToClosure grammar

        let closures = 
            collection
            |> Set.toSeq
            |> Seq.map(toclosure)
            |> Seq.zip kernels
            |> Map.ofSeq

        let actions =
            closures
            |> Seq.map(fun (KeyValue(kernel,closure)) -> 
                SpreadClosure.from(closure).items
                |> Seq.groupBy fst
                |> Seq.map(fun (la,sq) ->
                    sq
                    |> Seq.map snd
                    |> Set.ofSeq
                    |> Action.from
                    |> Pair.prepend la
                )
                |> Map.ofSeq
                |> Pair.prepend kernel
            )
            |> Map.ofSeq
        {
        productions = productions
        grammar = grammar
        kernels = kernels
        closures = closures
        actions = actions
        }
