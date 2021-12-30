namespace FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type Section4_8_1Test(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine
    let E = "E"
    let id = "id"

    ///表达式文法(4.1)
    let mainProductions = [
        [ E; E; "+"; E ]
        [ E; E; "*"; E ]
        [ E; "("; E; ")" ]
        [ E; id ]
    ]

    [<Fact>]
    member _.``fig4-49: parsing table test``() =

        let tbl = AmbiguousTable.create mainProductions

        //show tbl.ambiguousTable

        let y = set [
            set [{production=["";"E"];dot=0}],"(",set [Shift(set [{production=["E";"(";"E";")"];dot=1}])];
            set [{production=["";"E"];dot=0}],"E",set [Shift(set [{production=["";"E"];dot=1};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1}])];
            set [{production=["";"E"];dot=0}],"id",set [Shift(set [{production=["E";"id"];dot=1}])];
            set [{production=["";"E"];dot=1};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1}],"",set [Reduce ["";"E"]];
            set [{production=["";"E"];dot=1};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1}],"*",set [Shift(set [{production=["E";"E";"*";"E"];dot=2}])];
            set [{production=["";"E"];dot=1};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1}],"+",set [Shift(set [{production=["E";"E";"+";"E"];dot=2}])];
            set [{production=["E";"(";"E";")"];dot=1}],"(",set [Shift(set [{production=["E";"(";"E";")"];dot=1}])];
            set [{production=["E";"(";"E";")"];dot=1}],"E",set [Shift(set [{production=["E";"(";"E";")"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1}])];
            set [{production=["E";"(";"E";")"];dot=1}],"id",set [Shift(set [{production=["E";"id"];dot=1}])];
            set [{production=["E";"(";"E";")"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1}],")",set [Shift(set [{production=["E";"(";"E";")"];dot=3}])];
            set [{production=["E";"(";"E";")"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1}],"*",set [Shift(set [{production=["E";"E";"*";"E"];dot=2}])];
            set [{production=["E";"(";"E";")"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1}],"+",set [Shift(set [{production=["E";"E";"+";"E"];dot=2}])];
            set [{production=["E";"(";"E";")"];dot=3}],"",set [Reduce ["E";"(";"E";")"]];
            set [{production=["E";"(";"E";")"];dot=3}],")",set [Reduce ["E";"(";"E";")"]];
            set [{production=["E";"(";"E";")"];dot=3}],"*",set [Reduce ["E";"(";"E";")"]];
            set [{production=["E";"(";"E";")"];dot=3}],"+",set [Reduce ["E";"(";"E";")"]];
            set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"*";"E"];dot=3};{production=["E";"E";"+";"E"];dot=1}],"",set [Reduce ["E";"E";"*";"E"]];
            set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"*";"E"];dot=3};{production=["E";"E";"+";"E"];dot=1}],")",set [Reduce ["E";"E";"*";"E"]];
            set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"*";"E"];dot=3};{production=["E";"E";"+";"E"];dot=1}],"*",set [Shift(set [{production=["E";"E";"*";"E"];dot=2}]);Reduce ["E";"E";"*";"E"]];
            set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"*";"E"];dot=3};{production=["E";"E";"+";"E"];dot=1}],"+",set [Shift(set [{production=["E";"E";"+";"E"];dot=2}]);Reduce ["E";"E";"*";"E"]];
            set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"+";"E"];dot=3}],"",set [Reduce ["E";"E";"+";"E"]];
            set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"+";"E"];dot=3}],")",set [Reduce ["E";"E";"+";"E"]];
            set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"+";"E"];dot=3}],"*",set [Shift(set [{production=["E";"E";"*";"E"];dot=2}]);Reduce ["E";"E";"+";"E"]];
            set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"+";"E"];dot=3}],"+",set [Shift(set [{production=["E";"E";"+";"E"];dot=2}]);Reduce ["E";"E";"+";"E"]];
            set [{production=["E";"E";"*";"E"];dot=2}],"(",set [Shift(set [{production=["E";"(";"E";")"];dot=1}])];
            set [{production=["E";"E";"*";"E"];dot=2}],"E",set [Shift(set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"*";"E"];dot=3};{production=["E";"E";"+";"E"];dot=1}])];
            set [{production=["E";"E";"*";"E"];dot=2}],"id",set [Shift(set [{production=["E";"id"];dot=1}])];
            set [{production=["E";"E";"+";"E"];dot=2}],"(",set [Shift(set [{production=["E";"(";"E";")"];dot=1}])];
            set [{production=["E";"E";"+";"E"];dot=2}],"E",set [Shift(set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"+";"E"];dot=3}])];
            set [{production=["E";"E";"+";"E"];dot=2}],"id",set [Shift(set [{production=["E";"id"];dot=1}])];
            set [{production=["E";"id"];dot=1}],"",set [Reduce ["E";"id"]];
            set [{production=["E";"id"];dot=1}],")",set [Reduce ["E";"id"]];
            set [{production=["E";"id"];dot=1}],"*",set [Reduce ["E";"id"]];
            set [{production=["E";"id"];dot=1}],"+",set [Reduce ["E";"id"]]]
        
        Should.equal y tbl.ambiguousTable

        let ambiguousTable = 
            tbl.ambiguousTable
            |> Set.map(fun(k,s,a)-> 
                let k = k |> Set.map(fun i -> i.production,i.dot)
                k,s,a)

        //固定产生式
        let pconflicts = 
            ambiguousTable
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
            set [["E";"E";"*";"E"]];
            set [["E";"E";"*";"E"];["E";"E";"+";"E"]];
            set [["E";"E";"+";"E"]]]


        Should.equal rsc rsconflicts

    [<Fact>]
    member _.``grammar 4-1: ProductionPrecedence``() =
        let tbl = AmbiguousTable.create mainProductions

        let operators = tbl.productionOperators

        //show operators
        let y = Map.ofList [
            ["E"; "("; "E"; ")"],")"
            ["E"; "E"; "*"; "E"],"*"
            ["E"; "E"; "+"; "E"],"+"
            ["E"; "id"         ],"id"
        ]

        Should.equal y operators

