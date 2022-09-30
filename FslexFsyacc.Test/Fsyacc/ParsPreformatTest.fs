namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals

open System.IO
open System.Text

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

type ParsPreformatTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"pars.fsyacc")
    let text = File.ReadAllText(filePath)

    // 替换
    // /* -> (*
    // */ -> *)

    // 删除定义部分

    //let rawFsyacc = RawFsyaccFile.parse text
    //let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    [<Fact>]
    member _.``00 - compiler test``() =
        //show rawFsyacc.rules
        //show rawFsyacc.precedences
        //show rawFsyacc.declarations
        ()
