namespace FslexFsyacc.Prototypes

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Reflection

open FSharp.Reflection
open FSharp.Idioms
open FSharp.Idioms.Literal

type CrewInfoTest(output:ITestOutputHelper) =

    [<Fact>]
    member _.``01 - getCrewInfo test``() =
        let asm = Assembly.LoadFrom(Dir.dllFilePath)
        //output.WriteLine(asm.FullName)

        let tps = asm.GetExportedTypes()

        for tp in tps do
            output.WriteLine(stringify tp)

        tps
        |> Seq.filter(fun tp -> FSharpType.IsRecord tp)
        |> Seq.map(fun tp ->
            CrewInfoUtils.getCrewInfo tp
        )
        |> Seq.iter(fun crew ->
            output.WriteLine(stringify crew)
        )

    [<Fact>]
    member _.``02 - generateClassDecl test``() =
        let asm = Assembly.LoadFrom(Dir.dllFilePath)
        let tps = asm.GetExportedTypes()
        tps
        |> Seq.filter(fun tp -> FSharpType.IsRecord tp)
        |> Seq.map(fun tp ->
            CrewInfoUtils.getCrewInfo tp
        )
        |> Seq.map(fun crew ->
            CrewInfoUtils.generateClassDecl crew
        )
        |> Seq.iter(fun decl ->
            output.WriteLine(decl)
        )

    [<Fact>]
    member _.``03 - generateClassCtor test``() =
        let asm = Assembly.LoadFrom(Dir.dllFilePath)
        let tps = asm.GetExportedTypes()
        tps
        |> Seq.filter(fun tp -> FSharpType.IsRecord tp)
        |> Seq.map(fun tp ->
            CrewInfoUtils.getCrewInfo tp
        )
        |> Seq.map(fun crew ->
            CrewInfoUtils.generateClassCtor crew
        )
        |> Seq.iter(fun ctor ->
            output.WriteLine(ctor)
        )

    [<Fact>]
    member _.``04 - generateClassDefinition test``() =
        let asm = Assembly.LoadFrom(Dir.dllFilePath)
        let tps = asm.GetExportedTypes()

        tps
        |> Seq.filter(fun tp -> FSharpType.IsRecord tp)
        |> Seq.map(fun tp ->
            CrewInfoUtils.getCrewInfo tp
        )
        |> Seq.map(fun crew ->
            CrewInfoUtils.generateClassDefinition crew
        )
        |> Seq.iter(fun def ->
            output.WriteLine(def)
        )




