namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open FSharp.Idioms
open FSharp.xUnit

type FsyaccFileNameTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    //[<Fact>]
    //member _.``0 - names test``() =
    //    let x = Map.ofList [
    //        ["IfStatement";"if";"(";"Expression";")";"Statement"],"if"]
    //    let names = FsyaccFileName.productionToHeadBody x
    //    let y = Map ["IfStatement",Map [["if";"(";"Expression";")";"Statement"],"if"]]

    //    show names
    //    Should.equal y names

    //[<Fact>]
    //member _.``1 - names test``() =
    //    let x = 
    //        ["IfStatement",[
    //            ["if";"(";"Expression";")";"Statement"],"","{}"
    //            ["if";"(";"Expression";")";"Statement";"else";"Statement"],"","{}"
    //            ] ]

    //    let names = Map ["IfStatement",Map [["if";"(";"Expression";")";"Statement"],"if"]]


    //    let rules = FsyaccFileName.nameRules x names

    //    show rules
    //    let y = ["IfStatement",[
    //        ["if";"(";"Expression";")";"Statement"],"if","{}";
    //        ["if";"(";"Expression";")";"Statement";"else";"Statement"],"","{}"]]

    //    Should.equal y rules





