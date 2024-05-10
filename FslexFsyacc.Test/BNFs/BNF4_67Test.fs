namespace FslexFsyacc.Runtime.BNFs

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores

type BNF4_67Test (output: ITestOutputHelper) =

    [<Fact>]
    member _.``productions``() =
        let grammar = Grammar.from BNF4_67.mainProductions
        //for i,p in Seq.indexed grammar.productions do
        //output.WriteLine($"let production_{i} = {stringify p}")
        Should.equal BNF4_67.productions grammar.productions


    [<Fact>]
    member _.``kernels``() =
        let grammar = Grammar.from BNF4_67.mainProductions
        let row = BNFRow.from grammar.productions
        //for i,k in Seq.indexed row.kernels do
        //let k =
        //    k
        //    |> Set.toList
        //    |> List.map(fun ic -> ic.production, ic.dot)
        //output.WriteLine($"let kernel_{i} = {stringify k}")
        Should.equal BNF4_67.kernels row.kernels

    [<Fact>]
    member _.``closures``() =
        let grammar = Grammar.from BNF4_67.mainProductions
        let row = BNFRow.from grammar.productions
        //for i,c in row.closures |> Map.values |> Seq.indexed do
        //let c =
        //    c
        //    |> Set.toList
        //    |> List.map(fun (ic,la) -> ic.production, ic.dot, Set.toList la)
        //output.WriteLine($"let closure_{i} = {stringify c}")
        Should.equal BNF4_67.closures row.closures
        
    [<Fact>]
    member _.``actions``() =
        let grammar = Grammar.from BNF4_67.mainProductions
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
        //output.WriteLine($"let action_{i}_{j} = kernel_{i},{stringify sym},[{sacts}]")
        Should.equal BNF4_67.actions row.actions
