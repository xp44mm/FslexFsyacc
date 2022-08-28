namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit

type FsyaccFileStartTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``0 - getParentChildren test``() =
        let x:(string*((string list)list))list = 
            [
                "a",[
                    ["b";"c"]
                    ["d";"e"]
                ]
                "c",[
                    ["f"]
                ]
                "d",[
                    ["g"]
                ]

            ]
        let result = FsyaccFileStart.getParentChildren x
        show result

        let y = Map ["a",["c";"d"];"c",[];"d",[]]
        Should.equal y result
    [<Fact>]
    member _.``1 - dfsort test``() =
        let x:(string*((string list)list))list = 
            [
                "a",[
                    ["b";"c"]
                    ["d";"e"]
                ]
                "c",[
                    ["f"]
                ]
                "d",[
                    ["h"]
                ]
                "f",[["g"]]

            ]
        let result = FsyaccFileStart.deepFirstSort x "a"
        show result

        let y = ["a";"c";"f";"d"]
        Should.equal y result
