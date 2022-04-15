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

        let collection = AmbiguousCollection.create mainProductions

        // 显示冲突状态的冲突项目
        let conflictedClosures =
            collection.filterConflictedClosures() 

        // 提取冲突的产生式
        let productions =
            AmbiguousCollection.gatherProductions conflictedClosures
        show productions

        //产生式的优先级操作符: production -> symbol
        let productionSymbols = 
            ProductionUtils.precedenceOfProductions
                collection.grammar.terminals
                productions

        show productionSymbols

