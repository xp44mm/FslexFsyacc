namespace FslexFsyacc.Prototypes

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Reflection

open FSharp.Reflection
open FSharp.Idioms
open FSharp.Literals
open FSharp.Literals.Literal

type CrewInfoTest(output:ITestOutputHelper) =
    let show res = 
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``01 - getCrewInfo test``() =
        let asm = Assembly.LoadFrom(Dir.dllFilePath)
        output.WriteLine(asm.FullName)

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

    [<Fact>]
    member _.``05 - print namespace Test``() =
        let asm = Assembly.LoadFrom(Dir.dllFilePath)
        let tps = asm.GetExportedTypes()
        tps
        |> Seq.map(fun tp -> tp.Namespace)
        |> Seq.distinct
        |> Seq.iter(fun ns -> output.WriteLine(ns))

    [<Fact(
    //Skip="按需生成类型定义"
    )>]
    member _.``08 - Yacc ItemCoreCrews Output Test``() =
        let foldPath = "Yacc"
        let fileName = "ItemCoreCrews"
        let crewInfos = CrewInfoUtils.getCrewInfos(Dir.dllFilePath)
        let fileCrewInfos =
            crewInfos
            |> CrewInfoUtils.filterCrewInfos foldPath fileName

        let fileText =
            [
                $"namespace FslexFsyacc.{foldPath}"
                "open FslexFsyacc.Runtime"
                ""
                yield!
                    fileCrewInfos
                    |> Seq.map(fun crew -> $"{CrewInfoUtils.generateClassDefinition crew}\r\n")
            ]
            |> String.concat "\r\n"
            
        let path = Path.Combine(Dir.fslexFsyaccPath,foldPath,fileName+".fs")

        File.WriteAllText(path,fileText,Encoding.UTF8)
        output.WriteLine("文件输出成功：")
        output.WriteLine(path)

    [<Fact(
    //Skip="按需生成类型定义"
    )>]
    member _.``09 - Yacc EncodedParseTableCrews Output Test``() =
        let foldPath = "Yacc"
        let fileName = "EncodedParseTableCrews"
        let crewInfos = CrewInfoUtils.getCrewInfos(Dir.dllFilePath)
        let fileCrewInfos =
            crewInfos
            |> CrewInfoUtils.filterCrewInfos foldPath fileName

        let fileText =
            [
                $"namespace FslexFsyacc.{foldPath}"
                "open FslexFsyacc.Runtime"
                ""
                yield!
                    fileCrewInfos
                    |> Seq.map(fun crew -> $"{CrewInfoUtils.generateClassDefinition crew}\r\n")
            ]
            |> String.concat "\r\n"
            
        let path = Path.Combine(Dir.fslexFsyaccPath,foldPath,fileName+".fs")

        File.WriteAllText(path,fileText,Encoding.UTF8)
        output.WriteLine("文件输出成功：")
        output.WriteLine(path)



