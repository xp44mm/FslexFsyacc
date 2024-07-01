namespace FslexFsyacc.ModuleOrNamespaces

open System
open System.IO
open System.Text
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions

open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc

type ModuleOrNamespaceTokenTest(output:ITestOutputHelper) =
    let exit (rest:string) = Regex.IsMatch(rest,@"^\s*%\}$")

    [<Fact>]
    member _.``01 - first test``() =

        let x = SourceText.just(9, "open System.IO")
        let y = 
            ModuleOrNamespaceTokenUtils.tokenize exit x
            |> Seq.toList
        output.WriteLine(stringify y)

    [<Fact>]
    member _.``02 - second test``() =
        let x = SourceText.just(9, "open type System.Math")
        let y = 
            ModuleOrNamespaceTokenUtils.tokenize exit x
            |> Seq.toList
        output.WriteLine(stringify y)

    [<Fact>]
    member _.``03 - multi lines test``() =
        let x = 
            SourceText.just(9, [
            "open System.IO"
            "open type System.Math"
            "type SizeType = uint32"
            "type Transform<'a> = 'a -> 'a"
            ]
            |> String.concat "\r\n")

        let y = 
            ModuleOrNamespaceTokenUtils.tokenize exit x
            |> Seq.toList
        output.WriteLine(stringify y)





