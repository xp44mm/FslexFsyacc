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
        let x = Either(Either(Either(Either(Either(Either(Either(Either(Either(Atomic "0",Atomic "1"),Atomic "2"),Atomic "3"),Atomic "4"),Atomic "5"),Atomic "6"),Atomic "7"),Atomic "8"),Atomic "9")
        let y = LexFileNormalization.characterclass x
        let z = ["0";"1";"2";"3";"4";"5";"6";"7";"8";"9"]
        Should.equal y z