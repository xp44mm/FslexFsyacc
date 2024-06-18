namespace FslexFsyacc.ModuleOrNamespaces

open System
open System.IO
open System.Text
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions

open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc.TypeArguments

type ModuleDeclsCompilerTest (output:ITestOutputHelper) =
    let exit (rest:string) = Regex.IsMatch(rest,@"^\s*%\}$")

    [<Fact>]
    member _.``01 - multi lines test``() =
        let x = 
            [
            "open System.IO"
            "open type System.Math"
            ]
            |> String.concat "\r\n"

        let y,pos,rest = 
            ModuleDeclsCompiler.compile exit 9 x
            //|> Seq.toList
        output.WriteLine(stringify y)
        output.WriteLine(stringify pos)
        //output.WriteLine(stringify rest)
        Should.equal y [Open ["System";"IO"];OpenType(Ctor(["System";"Math"],[]))]
        Should.equal pos 47
        Should.equal rest ""

