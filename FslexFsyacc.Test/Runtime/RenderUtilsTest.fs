namespace FslexFsyacc.Runtime

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit

type RenderUtilsTest(output:ITestOutputHelper) =
    let show res = 
        res
        |> Literal.stringify
        |> output.WriteLine
    
    [<Theory>]
    [<InlineData("mutalbe", "?", "mutalbe{\\?}")>]
    [<InlineData("|"      , "+", "|{\\+}"      )>]
    member _.``internal string test``(m,q,e) =
        // fsyacc1.fsyacc
        let y = $"{m}{{\\{q}}}"
        Should.equal e y

    [<Theory>]
    [<InlineData("xyz"         , "xyz"     )>]
    [<InlineData("mutalbe{\\?}", "mutalbe?")>]
    [<InlineData("|{\\+}"      , "\"|\"+"  )>]
    member _.``render qua test``(x,e) =
        let y = RenderUtils.renderQuantifySymbol x
        output.WriteLine(y)
        Should.equal e y








