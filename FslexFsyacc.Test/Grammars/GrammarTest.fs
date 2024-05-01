namespace FslexFsyacc.Runtime.Grammars

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms
open FSharp.Idioms.Literal

type GrammarTest (output: ITestOutputHelper) =

    [<Fact>]
    member _.``grammar 4-1 test``() =
        let g = { productions = Grammar4_1.productions }
        Should.equal Grammar4_1.productions g.productions
        Should.equal Grammar4_1.startSymbol g.productions.MinimumElement.[1]
        Should.equal Grammar4_1.symbols g.symbols
        Should.equal Grammar4_1.nonterminals g.nonterminals
        Should.equal Grammar4_1.terminals g.terminals
        Should.equal Grammar4_1.nullables g.nullables
        Should.equal Grammar4_1.firsts g.firsts
        Should.equal Grammar4_1.lasts g.lasts
        Should.equal Grammar4_1.follows g.follows
        Should.equal Grammar4_1.precedes g.precedes

    [<Fact>]
    member _.``grammar 4-28 test``() =
        let g = { productions = Grammar4_28.productions }
        Should.equal Grammar4_28.productions g.productions
        Should.equal Grammar4_28.startSymbol g.productions.MinimumElement.[1]
        Should.equal Grammar4_28.symbols g.symbols
        Should.equal Grammar4_28.nonterminals g.nonterminals
        Should.equal Grammar4_28.terminals g.terminals
        Should.equal Grammar4_28.nullables g.nullables
        Should.equal Grammar4_28.firsts g.firsts
        Should.equal Grammar4_28.lasts g.lasts
        Should.equal Grammar4_28.follows g.follows
        Should.equal Grammar4_28.precedes g.precedes








