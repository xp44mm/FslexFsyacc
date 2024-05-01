namespace FslexFsyacc.Runtime.LALRs

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores

type GrammarForLalrTest(output: ITestOutputHelper) =

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    [<InlineData(4)>]
    [<InlineData(5)>]
    [<InlineData(6)>]
    member _.``getKernelCollection``(i:int) =
        let grammar = Grammar.from BNF4_55.mainProductions
        let kc = 
            grammar
            |> Grammar.getKernelCollection
        let kernels =
            kc.collection
            |> Set.map(fun k ->
                k
                |> Set.toList
                |> List.map(fun (ic,la) -> ic.production, ic.dot, Set.toList la)
            )
            |> Set.toArray
        let exp = [
            BNF4_55.kernel_0
            BNF4_55.kernel_1
            BNF4_55.kernel_2
            BNF4_55.kernel_3
            BNF4_55.kernel_4
            BNF4_55.kernel_5
            BNF4_55.kernel_6
        ]

        Should.equal exp.[i] kernels.[i]

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    [<InlineData(4)>]
    [<InlineData(5)>]
    [<InlineData(6)>]
    member _.``kernelToClosure``(i:int) =
        let grammar = Grammar.from BNF4_55.mainProductions
        let kc = 
            grammar
            |> Grammar.getKernelCollection
        let toclosure = Grammar.kernelToClosure grammar
        let closures = 
            kc.collection
            |> Set.toArray
            |> Array.map( toclosure )

        let render (scls:Set<ItemCore*Set<string>>) =
            scls
            |> Set.toList
            |> List.map(fun (i,la) -> i.production, i.dot, Set.toList la )
            
        let exp = [
            BNF4_55.closure_0
            BNF4_55.closure_1
            BNF4_55.closure_2
            BNF4_55.closure_3
            BNF4_55.closure_4
            BNF4_55.closure_5
            BNF4_55.closure_6
        ]
        Should.equal exp.[i] (render closures.[i])
        
    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    [<InlineData(4)>]
    [<InlineData(5)>]
    [<InlineData(6)>]
    member _.``spread Closure``(i:int) =
        let grammar = Grammar.from BNF4_55.mainProductions
        let kc = 
            grammar
            |> Grammar.getKernelCollection
        let toclosure = Grammar.kernelToClosure grammar
        let closures = 
            kc.collection
            |> Set.toArray
            |> Array.map(fun k ->
                k
                |> toclosure
                |> SpreadClosure.from
            )

        let render (scls:SpreadClosure) =
            scls.items
            |> Set.toList
            |> List.map(fun (la,ic) -> la, ic.production, ic.dot )

        let exp = [
            BNF4_55.spreadClosure_0
            BNF4_55.spreadClosure_1
            BNF4_55.spreadClosure_2
            BNF4_55.spreadClosure_3
            BNF4_55.spreadClosure_4
            BNF4_55.spreadClosure_5
            BNF4_55.spreadClosure_6
        ]

        Should.equal exp.[i] (render closures.[i])

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    [<InlineData(4)>]
    [<InlineData(5)>]
    [<InlineData(6)>]
    member _.``goto``(i:int) =
        let grammar = Grammar.from BNF4_55.mainProductions
        let kc = 
            grammar
            |> Grammar.getKernelCollection
        let toclosure = Grammar.kernelToClosure grammar
        let closures = 
            kc.collection
            |> Set.toArray
            |> Array.map(fun k ->
                k
                |> toclosure
                |> SpreadClosure.from
            )

        let gotos =
            closures
            |> Seq.collect(fun cl ->
                    GOTO.from cl
            )
            |> Seq.toArray

        // just for compare
        let gotos =
            gotos
            |> Array.map(fun x -> x.sourceKernel, x.symbol, x.targetKernel)

        let exp = BNF4_55.gotos
        Should.equal exp.[i] gotos.[i]
                

