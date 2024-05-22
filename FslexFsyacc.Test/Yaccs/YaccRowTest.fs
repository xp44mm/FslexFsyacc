namespace FslexFsyacc.Runtime.YACCs

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

type YaccRowTest (output: ITestOutputHelper) =

    [<Fact>]
    member _.``BNF4_55``() =
        let bnf = BNF.from BNF4_55.mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int*Associativity> = Map []

        let tbl = YaccRow.from(bnf.productions, dummyTokens, precedences)
        //let iproductions =
        //    tbl.bnf.grammar.productions
        //    |> Seq.mapi(fun i p -> p,i)
        //    |> Map.ofSeq

        //let ikernels =
        //    tbl.bnf.kernels
        //    |> Seq.mapi(fun i kernel -> kernel,i)
        //    |> Map.ofSeq

        //let isymbols =
        //    tbl.bnf.grammar.symbols
        //    |> Seq.mapi(fun i sym -> sym,i)
        //    |> Map.ofSeq

        //for KeyValue(src,mp) in tbl.actions do
        //for KeyValue(sym,actn) in mp do
        //let i = ikernels.[src]
        //let j = isymbols.[sym]
        //let sact = function
        //    | Reduce p -> $"Reduce production_{iproductions.[p]}"
        //    | Shift  k -> $"shift kernel_{ikernels.[k]}"

        //output.WriteLine($"let uaction_{i}_{j} = kernel_{i},{stringify sym},{sact actn}")
        Should.equal BNF4_55.uactions tbl.actions

    [<Fact>]
    member _.``BNF4_3 output``() =
        let bnf = BNF.from BNF4_3.mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int*Associativity> = BNF4_3.precedences

        let tbl = YaccRow.from(bnf.productions, dummyTokens, precedences)
        let iproductions =
            tbl.bnf.grammar.productions
            |> Seq.mapi(fun i p -> p,i)
            |> Map.ofSeq

        let ikernels =
            tbl.bnf.kernels
            |> Seq.mapi(fun i kernel -> kernel,i)
            |> Map.ofSeq

        let isymbols =
            tbl.bnf.grammar.symbols
            |> Seq.mapi(fun i sym -> sym,i)
            |> Map.ofSeq

        for KeyValue(src,mp) in tbl.actions do
        for KeyValue(sym,actn) in mp do
        let i = ikernels.[src]
        let j = isymbols.[sym]
        let sact = function
            | Reduce p -> $"Reduce production_{iproductions.[p]}"
            | Shift  k -> $"shift kernel_{ikernels.[k]}"

        output.WriteLine($"let uaction_{i}_{j} = kernel_{i},{stringify sym},{sact actn}")

    [<Fact>]
    member _.``BNF4_3``() =
        let bnf = BNF.from BNF4_3.mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int*Associativity> = BNF4_3.precedences

        let tbl = YaccRow.from(bnf.productions, dummyTokens, precedences)
        Should.equal BNF4_3.uactions tbl.actions

    [<Fact>]
    member _.``BNF4_67``() =
        let bnf = BNF.from BNF4_67.mainProductions
        let dummyTokens:Map<string list,string> = Map []
        let precedences:Map<string,int*Associativity> = Map []

        let tbl = YaccRow.from(bnf.productions, dummyTokens, precedences)
        //let iproductions =
        //    tbl.bnf.grammar.productions
        //    |> Seq.mapi(fun i p -> p,i)
        //    |> Map.ofSeq

        //let ikernels =
        //    tbl.bnf.kernels
        //    |> Seq.mapi(fun i kernel -> kernel,i)
        //    |> Map.ofSeq

        //let isymbols =
        //    tbl.bnf.grammar.symbols
        //    |> Seq.mapi(fun i sym -> sym,i)
        //    |> Map.ofSeq

        //for KeyValue(src,mp) in tbl.actions do
        //for KeyValue(sym,actn) in mp do
        //let i = ikernels.[src]
        //let j = isymbols.[sym]
        //let sact = function
        //    | Reduce p -> $"Reduce production_{iproductions.[p]}"
        //    | Shift  k -> $"shift kernel_{ikernels.[k]}"

        //output.WriteLine($"let uaction_{i}_{j} = kernel_{i},{stringify sym},{sact actn}")
        Should.equal BNF4_67.uactions tbl.actions



