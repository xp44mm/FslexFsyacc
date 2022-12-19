namespace FslexFsyacc.Runtime

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit

type PositionTest(output:ITestOutputHelper) =
    let show res = 
        res
        |> Literal.stringify
        |> output.WriteLine
    
    [<Fact>]
    member _.``01 - next index test``() =
        let x = {
            index = 0
            length = 1
            value = ()
        }

        let y = x.nextIndex

        Should.equal y 1

    [<Fact>]
    member _.``02 - raw test``() =
        let x = {
            index = 0
            length = 1
            value = ()
        }
        let source = "1234"
        let y = x.raw(source)
        let e = "1"

        Should.equal y e

    [<Fact>]
    member _.``03 - rest test``() =
        let x = {
            index = 0
            length = 1
            value = ()
        }
        let source = "1234"
        let y = x.rest(source)
        let e = "234"

        Should.equal y e

    [<Fact>]
    member _.``04 - totalLength test``() =
        let x = [
            {
                index = 0
                length = 1
                value = ()
            }
            //{
            //    index = 1
            //    length = 3
            //    value = ()
            //}
            {
                index = 4
                length = 2
                value = ()
            }
            ]
        let y = Position.totalLength(x)

        let e = 6

        Should.equal y e







