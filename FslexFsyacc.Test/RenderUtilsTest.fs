namespace FslexFsyacc

open Xunit
open Xunit.Abstractions

open FSharp.Idioms
open FSharp.xUnit

type RenderUtilsTest(output:ITestOutputHelper) =
    let show res = 
        res
        |> Literal.stringify
        |> output.WriteLine
    
    [<Theory>]
    [<InlineData("xyz"       , "xyz"     )>]
    [<InlineData("{mutalbe?}", "mutalbe?")>]
    [<InlineData("?"         , "\"?\""  )>]
    member _.``render qua test``(x,e) =
        let y = RenderUtils.renderSymbol x
        output.WriteLine(y)
        Should.equal e y








