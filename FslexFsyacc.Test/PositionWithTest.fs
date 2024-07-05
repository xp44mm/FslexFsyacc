namespace FslexFsyacc

open Xunit
open Xunit.Abstractions

open FSharp.Idioms
open FSharp.xUnit

type PositionWithTest(output:ITestOutputHelper) =
    let show res = 
        res
        |> Literal.stringify
        |> output.WriteLine
    
    [<Fact>]
    member _.``01 - next index test``() =
        let src = {
            index = 0
            text = "123"
        }

        let y = src.skip 1
        let e = SourceText.just(1,"23")
        Should.equal e y

    [<Fact>]
    member _.``02 - string Substring 2 test``() =
        let source = "1234"
        let y = source.Substring(0,1)
        let e = "1"
        Should.equal e y

    [<Fact>]
    member _.``03 - string Substring 1 test``() =
        let source = "0123"
        let y = source.Substring(1)
        let e = "123"

        Should.equal e y

    [<Fact>]
    member _.``04 - totalLength test``() =
        let x = [
            {
                index = 0
                length = 1
                value = ()
            }
            {
                index = 4
                length = 2
                value = ()
            }
            ]
        let y = PositionWith.totalLength(x)

        let e = 6

        Should.equal y e







