namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals.Literal
open FSharp.Idioms

open System.IO
open System.Text

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

type FsyaccFileRenderTest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    [<Fact>]
    member _.``00 - rec ? test``() =
        let text = """
%{%}
defnBindings :
    | LET rec? localBindings {  }
    | cPrototype {  }
rec? :
    | rec {  }
    | (*empty*) {  }
"""

        let fsyacc = RawFsyaccFile.parse text
        output.WriteLine(stringify fsyacc.rules)
        output.WriteLine(fsyacc.render())
        ()
