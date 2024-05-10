namespace FslexFsyacc.Runtime.BNFs

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores

type BNF4_3Test (output: ITestOutputHelper) =

    [<Fact>]
    member _.``productions``() =
        let grammar = Grammar.from BNF4_3.mainProductions
        //let iproductions =
        //    grammar.productions
        //    |> Seq.mapi(fun i p -> p,i)
        //    |> Map.ofSeq

        //for i,p in Seq.indexed grammar.productions do
        //output.WriteLine($"let production_{i} = {stringify p}")
        Should.equal BNF4_3.productions grammar.productions

    [<Fact>]
    member _.``kernels``() =
        let grammar = Grammar.from BNF4_3.mainProductions
        let row = BNFRow.from grammar.productions
        //let iproductions =
        //    row.productions
        //    |> Seq.mapi(fun i p -> p,i)
        //    |> Map.ofSeq

        //let ikernels =
        //    row.kernels
        //    |> Seq.map(fun k ->
        //        k
        //        |> Set.toList
        //        |> List.map(fun ic -> ic.production, ic.dot)
        //    )
        //    |> Seq.indexed

        //for i,k in ikernels do
        //output.WriteLine($"let kernel_{i} = {stringify k}")
        Should.equal BNF4_3.kernels row.kernels

    [<Fact>]
    member _.``closures``() =
        let grammar = Grammar.from BNF4_3.mainProductions
        let row = BNFRow.from grammar.productions

        //let kc = 
        //    grammar
        //    |> Grammar.getKernelCollection
        //let toclosure = Grammar.kernelToClosure grammar
        //let closures = 
        //    kc.collection
        //    |> Set.toArray
        //    |> Array.map toclosure 
        //    |> Array.indexed

        //for i,c in closures do
        //let c =
        //    c
        //    |> Set.toList
        //    |> List.map(fun (ic,la) -> ic.production, ic.dot, Set.toList la)

        //output.WriteLine($"let closure_{i} = {stringify c}")
        Should.equal BNF4_3.closures row.closures
        
    [<Fact>]
    member _.``conflicts``() =
        let grammar = Grammar.from BNF4_3.mainProductions
        let kc = 
            grammar
            |> Grammar.getKernelCollection
        let toclosure = Grammar.kernelToClosure grammar
        let conflicts = 
            kc.collection
            |> Set.toArray
            |> Array.map(fun k ->
                k
                |> toclosure
                |> SpreadClosure.from
            )
            |> Array.indexed

        let render (scls:SpreadClosure) =
            scls.items
            |> Set.toList
            |> List.map(fun (la,ic) -> la, ic.production, ic.dot )

        for i,c in conflicts do
        let c = render c
        output.WriteLine($"let conflict_{i} = {stringify c}")

    [<Fact>]
    member _.``actions``() =
        let grammar = Grammar.from BNF4_3.mainProductions
        let row = BNFRow.from grammar.productions
        //let iproductions =
        //    row.productions
        //    |> Seq.mapi(fun i p -> p,i)
        //    |> Map.ofSeq

        //let ikernels =
        //    row.kernels
        //    |> Seq.mapi(fun i kernel -> kernel,i)
        //    |> Map.ofSeq

        //let isymbols =
        //    row.grammar.symbols
        //    //|> Set.add ""
        //    |> Seq.mapi(fun i sym -> sym,i)
        //    |> Map.ofSeq

        //for KeyValue(src,mp) in row.actions do
        //for KeyValue(sym,acts) in mp do
        //let i = ikernels.[src]
        //let j = isymbols.[sym]
        //let sact = function
        //    | Reduce p -> $"Reduce production_{iproductions.[p]}"
        //    | Shift  k -> $"shift kernel_{ikernels.[k]}"

        //let sacts =
        //    acts
        //    |> Seq.map(sact)
        //    |> String.concat ";"
        //    //|> sprintf "[%s]"
        //output.WriteLine($"let action_{i}_{j} = kernel_{i},{stringify sym},[{sacts}]")

        Should.equal BNF4_3.actions row.actions
