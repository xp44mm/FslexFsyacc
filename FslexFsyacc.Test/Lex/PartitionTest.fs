namespace FslexFsyacc.Lex

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type PartitionTest(output:ITestOutputHelper) =
    let show res = 
        res 
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``example 3-40 minimize test``() = 
        // fig 3-36
        let dtran = set [
            (set [0u;1u;2u;4u;7u],'a',set [1u;2u;3u;4u;6u;7u;8u]);
            (set [0u;1u;2u;4u;7u],'b',set [1u;2u;4u;5u;6u;7u]);
            (set [1u;2u;3u;4u;6u;7u;8u],'a',set [1u;2u;3u;4u;6u;7u;8u]);
            (set [1u;2u;3u;4u;6u;7u;8u],'b',set [1u;2u;4u;5u;6u;7u;9u]);
            (set [1u;2u;4u;5u;6u;7u],'a',set [1u;2u;3u;4u;6u;7u;8u]);
            (set [1u;2u;4u;5u;6u;7u],'b',set [1u;2u;4u;5u;6u;7u]);
            (set [1u;2u;4u;5u;6u;7u;9u],'a',set [1u;2u;3u;4u;6u;7u;8u]);
            (set [1u;2u;4u;5u;6u;7u;9u],'b',set [1u;2u;4u;5u;6u;7u;10u]);
            (set [1u;2u;4u;5u;6u;7u;10u],'a',set [1u;2u;3u;4u;6u;7u;8u]);
            (set [1u;2u;4u;5u;6u;7u;10u],'b',set [1u;2u;4u;5u;6u;7u])
            ]

        let dfinals = [
            set [set [1u;2u;4u;5u;6u;7u;10u]];
            ]

        let pdfa = {
            dtran = dtran
            allStates = Transition.allStates dtran
            dfinals = dfinals
        }

        let encodeDfa, decodes,_ = pdfa.encode()
        let mdfa = encodeDfa.minimize(decodes)

        //最小化的結果
        //let y = Partition.minimize dtran dfinals

        //show y
        let dtran = set [
            set [0u;1u;2u;4u;5u;6u;7u],'a',set [1u;2u;3u;4u;6u;7u;8u];
            set [0u;1u;2u;4u;5u;6u;7u],'b',set [0u;1u;2u;4u;5u;6u;7u];
            set [1u;2u;3u;4u;6u;7u;8u],'a',set [1u;2u;3u;4u;6u;7u;8u];
            set [1u;2u;3u;4u;6u;7u;8u],'b',set [1u;2u;4u;5u;6u;7u;9u];
            set [1u;2u;4u;5u;6u;7u;9u],'a',set [1u;2u;3u;4u;6u;7u;8u];
            set [1u;2u;4u;5u;6u;7u;9u],'b',set [1u;2u;4u;5u;6u;7u;10u];
            set [1u;2u;4u;5u;6u;7u;10u],'a',set [1u;2u;3u;4u;6u;7u;8u];
            set [1u;2u;4u;5u;6u;7u;10u],'b',set [0u;1u;2u;4u;5u;6u;7u]
            ]

        Should.equal mdfa.dtran dtran

        let dfinals = [
            set [set [1u;2u;4u;5u;6u;7u;10u]]
            ]

        Should.equal mdfa.dfinals dfinals

    [<Fact>]
    member _.``example 3-41 firstPartitions``() =
        let allStates = set [
            set [0u;1u;3u;7u];
            set [2u;4u;7u];
            set [5u;8u];
            set [6u;8u];
            set [7u];
            set [8u]
            ]
        let dfinals = [
            set [set [2u;4u;7u]];
            set [set [6u;8u]];
            set [set [5u;8u];
            set [8u]]
            ]

        // 输入参数
        let firstPartitions = 
            dfinals
            |> Set.ofList
            |> Set.add(allStates - Set.unionMany dfinals)
            |> Set.add(Set.singleton Set.empty)

        //show firstPartitions
        let y = set [
            set [set []];
            set [set [0u;1u;3u;7u];set [7u]];
            set [set [2u;4u;7u]];
            set [set [5u;8u];set [8u]];
            set [set [6u;8u]]
            ]
        Should.equal y firstPartitions

    [<Fact>]
    member _.``example 3-41 minimize test``() = 
        let dtran = set [
            (set [0u;1u;3u;7u],'a',set [2u;4u;7u]);
            (set [0u;1u;3u;7u],'b',set [8u]);
            (set [2u;4u;7u]   ,'a',set [7u]);
            (set [2u;4u;7u]   ,'b',set [5u;8u]);
            (set [5u;8u]      ,'b',set [6u;8u]);
            (set [6u;8u]      ,'b',set [8u]);
            (set [7u]         ,'a',set [7u]);
            (set [7u]         ,'b',set [8u]);
            (set [8u]         ,'b',set [8u]);
            ]

        let dfinals = [
            set [set [2u; 4u; 7u]];
            set [set [6u; 8u]];
            set [set [5u; 8u]; set [8u]];
            ]

        let pdfa = {
            dtran = dtran
            allStates = Transition.allStates dtran
            dfinals = dfinals
        }

        let encodeDfa, decodes,_ = pdfa.encode()
        let mdfa = encodeDfa.minimize(decodes)


        ////最小化的結果
        //let mdtran = 
        //    Partition.minimize dtran dfinals

        //本例的輸入本來就是最小狀態數目
        Should.equal mdfa.dtran dtran
        Should.equal mdfa.dfinals dfinals




