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

        let collection = AmbiguousCollectionCrewUtils.newAmbiguousCollectionCrew mainProductions

        // 提取冲突的产生式
        let productions =
            AmbiguousCollectionUtils.collectConflictedProductions collection.conflictedItemCores

        //show productions
        let y =set[
            ["E";"E";"*";"E"];
            ["E";"E";"+";"E"]]

        Should.equal y productions

    [<Fact>]
    member _.``grammar 4-1: ProductionPrecedence``() =
        let collection = AmbiguousCollectionCrewUtils.newAmbiguousCollectionCrew mainProductions

        // 提取冲突的产生式
        let productions =
            AmbiguousCollectionUtils.collectConflictedProductions collection.conflictedItemCores

        //产生式的优先级操作符
        let operators = 
            ProductionUtils.precedenceOfProductions 
                collection.terminals 
                productions
            
        //show operators
        let y = [
            ["E"; "E"; "*"; "E"],"*"
            ["E"; "E"; "+"; "E"],"+"
        ]

        Should.equal y operators

