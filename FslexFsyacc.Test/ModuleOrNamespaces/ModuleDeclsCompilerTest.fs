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
open FslexFsyacc

type ModuleDeclsCompilerTest (output:ITestOutputHelper) =
    let exit (rest:string) = Regex.IsMatch(rest,@"^\s*%\}$")

    [<Fact>]
    member _.``01 - multi lines test``() =
        let x = 
            [
            "open System.IO"
            "open type System.Math"
            "type SizeType = uint32"
            "type Transform<'a> = 'a -> 'a"
            ]
            |> String.concat "\r\n"

        output.WriteLine(stringify x.Length)

        let src = SourceText.just(9,x)
        let y,restSrc = 
            ModuleDeclsCompiler.compile exit src

            //|> Seq.toList
        output.WriteLine(stringify y)
        output.WriteLine(stringify restSrc.index)
        //output.WriteLine(stringify rest)
        Should.equal y [Open ["System";"IO"];OpenType(Ctor(["System";"Math"],[]));TypeAbb(["SizeType"],Ctor(["uint32"],[]));TypeAbb(["Transform";"'a"],Fun [TypeParam(false,"a");TypeParam(false,"a")])]
        Should.equal restSrc.index 101
        Should.equal restSrc.text ""

