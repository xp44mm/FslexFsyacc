namespace FslexFsyacc.Prototypes

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Reflection

open FSharp.Reflection
open FSharp.Idioms

type CodeGenerators(output:ITestOutputHelper) =

    [<Fact>]
    member _.``00 - print namespaces``() =
        let asm = Assembly.LoadFrom(Dir.dllFilePath)
        let tps = asm.GetExportedTypes()
        tps
        |> Seq.map(fun tp -> tp.Namespace)
        |> Seq.distinct
        |> Seq.iter(fun ns -> output.WriteLine(ns))

    [<Fact(
    Skip="按需生成类型定义"
    )>]
    member _.``01 - Yacc ItemCoreCrews Output Test``() =
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
    Skip="按需生成类型定义"
    )>]
    member _.``02 - Yacc AmbiguousCollectionCrews Output Test``() =
        let foldPath = "Yacc"
        let fileName = "AmbiguousCollectionCrews"
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
    Skip="按需生成类型定义"
    )>]
    member _.``03 - Yacc SemanticParseTableCrews Output Test``() =
        let foldPath = "Yacc"
        let fileName = "SemanticParseTableCrews"
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
    Skip="按需生成类型定义"
    )>]
    member _.``04 - Fsyacc FlatedFsyaccFileCrews Output Test``() =
        let foldPath = "Fsyacc"
        let fileName = "FlatedFsyaccFileCrews"
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





