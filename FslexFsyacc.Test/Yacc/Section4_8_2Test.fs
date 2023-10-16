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

        let collection = AmbiguousCollectionCrewUtils.newAmbiguousCollectionCrew mainProductions

        // 提取冲突的产生式
        let productions =
            AmbiguousCollectionUtils.collectConflictedProductions collection.conflictedItemCores

        show productions

        //产生式的优先级操作符: production -> symbol
        let productionSymbols = 
            ProductionListUtils.precedenceOfProductions
                collection.terminals
                productions

        show productionSymbols

