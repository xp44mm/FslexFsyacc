namespace FslexFsyacc.Runtime.ItemCores

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms
open FSharp.Idioms.Literal

type ItemCoreRowUtilsTest(output:ITestOutputHelper) =
    [<Fact>]
    member _.``01-1 - empty production test``() =
        let production = ["e"]
        let y = ItemCoreRowUtils.getRow production 0
        output.WriteLine(stringify y)
        let e = {production= ["e"];dot= 0;backwards= [];forwards= [];dotmax= true;isKernel= false}
        Should.equal e y

    [<Fact>]
    member _.``01-2 - empty production error test``() =
        let production = ["e"]
        let e = Assert.Throws<_>(fun()->
            let _ = ItemCoreRowUtils.getRow production 1
            ()
        )
        output.WriteLine(e.Message)
        output.WriteLine(stringify ItemCoreRowUtils.rows.[production])

    [<Fact>]
    member _.``02 - augment production test``() =
        let production = ["";"s"]
        let e = [
            {production= ["";"s"];dot= 0;backwards= [];forwards= ["s"];dotmax= false;isKernel= true};
            {production= ["";"s"];dot= 1;backwards= ["s"];forwards= [];dotmax= true;isKernel= true}]
        let y0 = ItemCoreRowUtils.getRow production 0
        Should.equal e.[0] y0

        let y1 = ItemCoreRowUtils.getRow production 1
        Should.equal e.[1] y1
