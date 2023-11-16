namespace FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.Idioms
open FSharp.xUnit

type PrecedenceTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine
    let E = "E"
    let id = "id"

    ///表达式文法(fig 4-59)
    let mainProductions = [
        [ E; E; "+"; E ]
        [ E; E; "-"; E ]
        [ E; E; "*"; E ]
        [ E; E; "/"; E ]
        [ E; "("; E; ")" ]
        [ E;  "-"; E ]
        [ E; id ]
    ]

    let collection = AmbiguousCollectionCrewUtils.newAmbiguousCollectionCrew mainProductions 

    [<Fact>]
    member _.``整理优先级输入``() =
        let E = "E"

        //// 相同优先级的符号在同一行输入，且具有相同的结合性
        //let y = Associativity.from [
        //               LeftAssoc,[TerminalKey "+";TerminalKey "-"]
        //               LeftAssoc,[TerminalKey "*";TerminalKey "/"]
        //               RightAssoc,[ProductionKey [ E;  "-"; E ]]
        //           ]
        ////show y

        //// 每个符号单列优先级
        //let precedences = Map.ofList [
        //    TerminalKey "*",199;
        //    TerminalKey "+",99;
        //    TerminalKey "-",99;
        //    TerminalKey "/",199;
        //    ProductionKey ["E";"-";"E"],301]

        //Should.equal y precedences
        ()
    //[<Fact>]
    //member _.``项集符号是终结符号的项集``() =

    //    // 优先级运算符
    //    let ops = set [
    //        "*"
    //        "+"
    //        "-"
    //        "/"
    //    ]

    //    // 有冲突的操作符，和它所在的kernel
    //    let kernels = 
    //        collection.kernelSymbols
    //        |> Map.filter(fun kernel op -> ops.Contains op)

    //    //show kernels
    //    let y = Map.ofList [
    //        set [{production=["E";"-";"E"];dot=1}],"-";
    //        set [{production=["E";"E";"*";"E"];dot=2}],"*";
    //        set [{production=["E";"E";"+";"E"];dot=2}],"+";
    //        set [{production=["E";"E";"-";"E"];dot=2}],"-";
    //        set [{production=["E";"E";"/";"E"];dot=2}],"/"]

    //    Should.equal y kernels

    //[<Fact>]
    //member _.``precOfProd and precOfTerm``() =

    //    // 没有冲突的运算符不必输入优先级和结合性。
    //    let precedences = Map.ofList [
    //        TerminalKey "*",9979;
    //        TerminalKey "+",9969;
    //        TerminalKey "-",9969;
    //        TerminalKey "/",9979;
    //        ProductionKey ["E";"-";"E"],9991]

    //    let precOfProd = PrecedenceResolver.resolvePrecOfProd tbl.productionOperators precedences

    //    let precOfProd = precOfProd ["E";"-";"E"]
    //    Should.equal precOfProd 9991

    //    let precOfTerm = PrecedenceResolver.resolvePrecOfTerminal tbl.kernelProductions precedences
    //    let precOfTerms = 
    //        [
    //            set [{production=["E";"-";"E"];dot=1}],"-";
    //            set [{production=["E";"E";"*";"E"];dot=2}],"*";
    //            set [{production=["E";"E";"+";"E"];dot=2}],"+";
    //            set [{production=["E";"E";"-";"E"];dot=2}],"-";
    //            set [{production=["E";"E";"/";"E"];dot=2}],"/"
    //            ]
    //        |> List.map(fun (sj,t) -> precOfTerm sj t)

    //    //注意第一个`-`是负号，第二个`-`是减号。
    //    let y = [9991; 9979; 9969; 9969; 9979]

    //    Should.equal y precOfTerms


    //[<Fact>]
    //member _.``fig4-49: eliminate parsing table test``() =
    //    let tbl = AmbiguousCollection.create mainProductions

    //    //固定产生式，先把非移动/归纳冲突解决掉。
    //    let pconflicts = 
    //        tbl.filterConflictedClosures()
    //        |> AmbiguousCollection.gatherProductions

    //        //|> ConflictFactory.productionConflict 

    //    Assert.True(pconflicts.IsEmpty)

    //    ////如果出现移动/归纳冲突，可以用运算符优先级解决。
    //    ////用于优先级
    //    //let rsconflicts =
    //    //    tbl
    //    //    |> ConflictFactory.shiftReduceConflict

    //    ////show rsconflicts
    //    //let rsc = set [
    //    //    set [["E";"-";"E"];["E";"E";"*";"E"]];
    //    //    set [["E";"-";"E"];["E";"E";"+";"E"]];
    //    //    set [["E";"-";"E"];["E";"E";"-";"E"]];
    //    //    set [["E";"-";"E"];["E";"E";"/";"E"]];
    //    //    set [["E";"E";"*";"E"]];
    //    //    set [["E";"E";"*";"E"];["E";"E";"+";"E"]];
    //    //    set [["E";"E";"*";"E"];["E";"E";"-";"E"]];
    //    //    set [["E";"E";"*";"E"];["E";"E";"/";"E"]];
    //    //    set [["E";"E";"+";"E"]];
    //    //    set [["E";"E";"+";"E"];["E";"E";"-";"E"]];
    //    //    set [["E";"E";"+";"E"];["E";"E";"/";"E"]];
    //    //    set [["E";"E";"-";"E"]];
    //    //    set [["E";"E";"-";"E"];["E";"E";"/";"E"]];
    //    //    set [["E";"E";"/";"E"]]]
    //    //Should.equal rsc rsconflicts

    //    // 相同优先级的运算符写在同一行
    //    let inputPrecs = [
    //        LeftAssoc,[TerminalKey "+";TerminalKey "-"]
    //        LeftAssoc,[TerminalKey "*";TerminalKey "/"]
    //        RightAssoc,[ProductionKey [ E;  "-"; E ]]
    //    ]

    //    let precs = Associativity.from inputPrecs

    //    // 解决冲突后，歧义表中的元素只有单个了，可以消除歧义。
    //    let eliminate = 
    //        EliminatingAmbiguity.eliminateActions tbl.productionOperators tbl.kernelProductions precs

    //    let uniqueTable = Set.map eliminate tbl.ambiguousTable

    //    //show y

    //    let y = set [
    //        set [{production=["";"E"];dot=0}],"(",Shift(set [{production=["E";"(";"E";")"];dot=1}]);
    //        set [{production=["";"E"];dot=0}],"-",Shift(set [{production=["E";"-";"E"];dot=1}]);
    //        set [{production=["";"E"];dot=0}],"E",Shift(set [{production=["";"E"];dot=1};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}]);
    //        set [{production=["";"E"];dot=0}],"id",Shift(set [{production=["E";"id"];dot=1}]);
    //        set [{production=["";"E"];dot=1};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"",Reduce ["";"E"];
    //        set [{production=["";"E"];dot=1};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"*",Shift(set [{production=["E";"E";"*";"E"];dot=2}]);
    //        set [{production=["";"E"];dot=1};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"+",Shift(set [{production=["E";"E";"+";"E"];dot=2}]);
    //        set [{production=["";"E"];dot=1};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"-",Shift(set [{production=["E";"E";"-";"E"];dot=2}]);
    //        set [{production=["";"E"];dot=1};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"/",Shift(set [{production=["E";"E";"/";"E"];dot=2}]);
    //        set [{production=["E";"(";"E";")"];dot=1}],"(",Shift(set [{production=["E";"(";"E";")"];dot=1}]);
    //        set [{production=["E";"(";"E";")"];dot=1}],"-",Shift(set [{production=["E";"-";"E"];dot=1}]);
    //        set [{production=["E";"(";"E";")"];dot=1}],"E",Shift(set [{production=["E";"(";"E";")"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}]);
    //        set [{production=["E";"(";"E";")"];dot=1}],"id",Shift(set [{production=["E";"id"];dot=1}]);
    //        set [{production=["E";"(";"E";")"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],")",Shift(set [{production=["E";"(";"E";")"];dot=3}]);
    //        set [{production=["E";"(";"E";")"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"*",Shift(set [{production=["E";"E";"*";"E"];dot=2}]);
    //        set [{production=["E";"(";"E";")"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"+",Shift(set [{production=["E";"E";"+";"E"];dot=2}]);
    //        set [{production=["E";"(";"E";")"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"-",Shift(set [{production=["E";"E";"-";"E"];dot=2}]);
    //        set [{production=["E";"(";"E";")"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"/",Shift(set [{production=["E";"E";"/";"E"];dot=2}]);
    //        set [{production=["E";"(";"E";")"];dot=3}],"",Reduce ["E";"(";"E";")"];
    //        set [{production=["E";"(";"E";")"];dot=3}],")",Reduce ["E";"(";"E";")"];
    //        set [{production=["E";"(";"E";")"];dot=3}],"*",Reduce ["E";"(";"E";")"];
    //        set [{production=["E";"(";"E";")"];dot=3}],"+",Reduce ["E";"(";"E";")"];
    //        set [{production=["E";"(";"E";")"];dot=3}],"-",Reduce ["E";"(";"E";")"];
    //        set [{production=["E";"(";"E";")"];dot=3}],"/",Reduce ["E";"(";"E";")"];
    //        set [{production=["E";"-";"E"];dot=1}],"(",Shift(set [{production=["E";"(";"E";")"];dot=1}]);
    //        set [{production=["E";"-";"E"];dot=1}],"-",Shift(set [{production=["E";"-";"E"];dot=1}]);
    //        set [{production=["E";"-";"E"];dot=1}],"E",Shift(set [{production=["E";"-";"E"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}]);
    //        set [{production=["E";"-";"E"];dot=1}],"id",Shift(set [{production=["E";"id"];dot=1}]);
    //        set [{production=["E";"-";"E"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"",Reduce ["E";"-";"E"];
    //        set [{production=["E";"-";"E"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],")",Reduce ["E";"-";"E"];
    //        set [{production=["E";"-";"E"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"*",Reduce ["E";"-";"E"];
    //        set [{production=["E";"-";"E"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"+",Reduce ["E";"-";"E"];
    //        set [{production=["E";"-";"E"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"-",Reduce ["E";"-";"E"];
    //        set [{production=["E";"-";"E"];dot=2};{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"/",Reduce ["E";"-";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"*";"E"];dot=3};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"",Reduce ["E";"E";"*";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"*";"E"];dot=3};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],")",Reduce ["E";"E";"*";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"*";"E"];dot=3};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"*",Reduce ["E";"E";"*";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"*";"E"];dot=3};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"+",Reduce ["E";"E";"*";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"*";"E"];dot=3};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"-",Reduce ["E";"E";"*";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"*";"E"];dot=3};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"/",Reduce ["E";"E";"*";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"+";"E"];dot=3};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"",Reduce ["E";"E";"+";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"+";"E"];dot=3};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],")",Reduce ["E";"E";"+";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"+";"E"];dot=3};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"*",Shift(set [{production=["E";"E";"*";"E"];dot=2}]);
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"+";"E"];dot=3};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"+",Reduce ["E";"E";"+";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"+";"E"];dot=3};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"-",Reduce ["E";"E";"+";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"+";"E"];dot=3};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}],"/",Shift(set [{production=["E";"E";"/";"E"];dot=2}]);
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"-";"E"];dot=3};{production=["E";"E";"/";"E"];dot=1}],"",Reduce ["E";"E";"-";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"-";"E"];dot=3};{production=["E";"E";"/";"E"];dot=1}],")",Reduce ["E";"E";"-";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"-";"E"];dot=3};{production=["E";"E";"/";"E"];dot=1}],"*",Shift(set [{production=["E";"E";"*";"E"];dot=2}]);
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"-";"E"];dot=3};{production=["E";"E";"/";"E"];dot=1}],"+",Reduce ["E";"E";"-";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"-";"E"];dot=3};{production=["E";"E";"/";"E"];dot=1}],"-",Reduce ["E";"E";"-";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"-";"E"];dot=3};{production=["E";"E";"/";"E"];dot=1}],"/",Shift(set [{production=["E";"E";"/";"E"];dot=2}]);
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1};{production=["E";"E";"/";"E"];dot=3}],"",Reduce ["E";"E";"/";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1};{production=["E";"E";"/";"E"];dot=3}],")",Reduce ["E";"E";"/";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1};{production=["E";"E";"/";"E"];dot=3}],"*",Reduce ["E";"E";"/";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1};{production=["E";"E";"/";"E"];dot=3}],"+",Reduce ["E";"E";"/";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1};{production=["E";"E";"/";"E"];dot=3}],"-",Reduce ["E";"E";"/";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1};{production=["E";"E";"/";"E"];dot=3}],"/",Reduce ["E";"E";"/";"E"];
    //        set [{production=["E";"E";"*";"E"];dot=2}],"(",Shift(set [{production=["E";"(";"E";")"];dot=1}]);
    //        set [{production=["E";"E";"*";"E"];dot=2}],"-",Shift(set [{production=["E";"-";"E"];dot=1}]);
    //        set [{production=["E";"E";"*";"E"];dot=2}],"E",Shift(set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"*";"E"];dot=3};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}]);
    //        set [{production=["E";"E";"*";"E"];dot=2}],"id",Shift(set [{production=["E";"id"];dot=1}]);
    //        set [{production=["E";"E";"+";"E"];dot=2}],"(",Shift(set [{production=["E";"(";"E";")"];dot=1}]);
    //        set [{production=["E";"E";"+";"E"];dot=2}],"-",Shift(set [{production=["E";"-";"E"];dot=1}]);
    //        set [{production=["E";"E";"+";"E"];dot=2}],"E",Shift(set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"+";"E"];dot=3};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1}]);
    //        set [{production=["E";"E";"+";"E"];dot=2}],"id",Shift(set [{production=["E";"id"];dot=1}]);
    //        set [{production=["E";"E";"-";"E"];dot=2}],"(",Shift(set [{production=["E";"(";"E";")"];dot=1}]);
    //        set [{production=["E";"E";"-";"E"];dot=2}],"-",Shift(set [{production=["E";"-";"E"];dot=1}]);
    //        set [{production=["E";"E";"-";"E"];dot=2}],"E",Shift(set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"-";"E"];dot=3};{production=["E";"E";"/";"E"];dot=1}]);
    //        set [{production=["E";"E";"-";"E"];dot=2}],"id",Shift(set [{production=["E";"id"];dot=1}]);
    //        set [{production=["E";"E";"/";"E"];dot=2}],"(",Shift(set [{production=["E";"(";"E";")"];dot=1}]);
    //        set [{production=["E";"E";"/";"E"];dot=2}],"-",Shift(set [{production=["E";"-";"E"];dot=1}]);
    //        set [{production=["E";"E";"/";"E"];dot=2}],"E",Shift(set [{production=["E";"E";"*";"E"];dot=1};{production=["E";"E";"+";"E"];dot=1};{production=["E";"E";"-";"E"];dot=1};{production=["E";"E";"/";"E"];dot=1};{production=["E";"E";"/";"E"];dot=3}]);
    //        set [{production=["E";"E";"/";"E"];dot=2}],"id",Shift(set [{production=["E";"id"];dot=1}]);
    //        set [{production=["E";"id"];dot=1}],"",Reduce ["E";"id"];
    //        set [{production=["E";"id"];dot=1}],")",Reduce ["E";"id"];
    //        set [{production=["E";"id"];dot=1}],"*",Reduce ["E";"id"];
    //        set [{production=["E";"id"];dot=1}],"+",Reduce ["E";"id"];
    //        set [{production=["E";"id"];dot=1}],"-",Reduce ["E";"id"];
    //        set [{production=["E";"id"];dot=1}],"/",Reduce ["E";"id"]]
                        
    //    Should.equal y uniqueTable
