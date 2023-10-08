namespace FslexFsyacc.Lex

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type NFAOperationsTest(output:ITestOutputHelper) =
    let show res = 
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``example 3-21 closures & moves``() =
        ///figure 3.34
        let ntran = set [
            (0, None, 1);
            (0, None, 7);
            (1, None, 2);
            (1, None, 4);
            (2, Some 'a', 3);
            (4, Some 'b', 5);
            (3, None, 6);
            (5, None, 6);
            (6, None, 1);
            (6, None, 7);
            (7, Some 'a', 8);
            (8, Some 'b', 9);
            (9, Some 'b', 10)
        ]

        let ops =  NFAOperationsUtils.create(ntran)

        let closures = Map.ofList [
            (0,set [0;1;2;4;7]);
            (1,set [1;2;4]);
            (2,set [2]);
            (3,set [1;2;3;4;6;7]);
            (4,set [4]);
            (5,set [1;2;4;5;6;7]);
            (6,set [1;2;4;6;7]);
            (7,set [7]);
            (8,set [8]);
            (9,set [9]);
            (10,set [10])
            ]

        let moves = Map.ofList [
            (2,Map.ofList [('a',set [3])]);
            (4,Map.ofList [('b',set [5])]);
            (7,Map.ofList [('a',set [8])]);
            (8,Map.ofList [('b',set [9])]);
            (9,Map.ofList [('b',set [10])])
            ]

        Should.equal ops.closures closures
        Should.equal ops.moves moves

