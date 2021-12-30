namespace FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type Section4_8_2Test(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``fig4-51: conflicts test``() =
        let S = "S"
        let i = "i"
        let e = "e"
        let a = "a"

        let mainProductions = [
            [ S; i; S; e; S ]
            [ S; i; S;]
            [ S; a; ]
        ]

        let tbl = AmbiguousTable.create mainProductions

        show tbl.ambiguousTable

        let y = set [
            set [{production=["";"S"];dot=0}],"S",set [Shift(set [{production=["";"S"];dot=1}])];
            set [{production=["";"S"];dot=0}],"a",set [Shift(set [{production=["S";"a"];dot=1}])];
            set [{production=["";"S"];dot=0}],"i",set [Shift(set [{production=["S";"i";"S"];dot=1};{production=["S";"i";"S";"e";"S"];dot=1}])];
            set [{production=["";"S"];dot=1}],"",set [Reduce ["";"S"]];
            set [{production=["S";"a"];dot=1}],"",set [Reduce ["S";"a"]];
            set [{production=["S";"a"];dot=1}],"e",set [Reduce ["S";"a"]];
            set [{production=["S";"i";"S"];dot=1};{production=["S";"i";"S";"e";"S"];dot=1}],"S",set [Shift(set [{production=["S";"i";"S"];dot=2};{production=["S";"i";"S";"e";"S"];dot=2}])];
            set [{production=["S";"i";"S"];dot=1};{production=["S";"i";"S";"e";"S"];dot=1}],"a",set [Shift(set [{production=["S";"a"];dot=1}])];
            set [{production=["S";"i";"S"];dot=1};{production=["S";"i";"S";"e";"S"];dot=1}],"i",set [Shift(set [{production=["S";"i";"S"];dot=1};{production=["S";"i";"S";"e";"S"];dot=1}])];
            set [{production=["S";"i";"S"];dot=2};{production=["S";"i";"S";"e";"S"];dot=2}],"",set [Reduce ["S";"i";"S"]];
            set [{production=["S";"i";"S"];dot=2};{production=["S";"i";"S";"e";"S"];dot=2}],"e",set [Shift(set [{production=["S";"i";"S";"e";"S"];dot=3}]);Reduce ["S";"i";"S"]];
            set [{production=["S";"i";"S";"e";"S"];dot=3}],"S",set [Shift(set [{production=["S";"i";"S";"e";"S"];dot=4}])];
            set [{production=["S";"i";"S";"e";"S"];dot=3}],"a",set [Shift(set [{production=["S";"a"];dot=1}])];
            set [{production=["S";"i";"S";"e";"S"];dot=3}],"i",set [Shift(set [{production=["S";"i";"S"];dot=1};{production=["S";"i";"S";"e";"S"];dot=1}])];
            set [{production=["S";"i";"S";"e";"S"];dot=4}],"",set [Reduce ["S";"i";"S";"e";"S"]];
            set [{production=["S";"i";"S";"e";"S"];dot=4}],"e",set [Reduce ["S";"i";"S";"e";"S"]]]

        Should.equal y tbl.ambiguousTable

        //固定产生式
        let pconflicts = 
            tbl.ambiguousTable
            |> ConflictFactory.productionConflict 

        Assert.True(pconflicts.IsEmpty)

        // 符号多用警告
        let warning = ConflictFactory.overloadsWarning tbl

        //show warning
        Assert.True(warning.IsEmpty)

        //用于优先级
        let rsconflicts =
            tbl
            |> ConflictFactory.shiftReduceConflict

        //show rsconflicts
        let rsc = set [
            set [["S";"i";"S"];["S";"i";"S";"e";"S"]]]

        Should.equal rsc rsconflicts

