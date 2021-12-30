namespace FslexFsyacc.Lex

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type LexFileNormalizationTest(output:ITestOutputHelper) =
    let show res = 
        res 
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``get digits``() =
        let x = Uion(Uion(Uion(Uion(Uion(Uion(Uion(Uion(Uion(Character "0",Character "1"),Character "2"),Character "3"),Character "4"),Character "5"),Character "6"),Character "7"),Character "8"),Character "9")
        let y = LexFileNormalization.characterclass x
        let z = ["0";"1";"2";"3";"4";"5";"6";"7";"8";"9"]
        Should.equal y z