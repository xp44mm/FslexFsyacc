namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open System.IO

open FSharp.xUnit
open FSharp.Literals
open FslexFsyacc.Fsyacc

type FsyaccParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine
    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.Parent.FullName
    let sourcePath = Path.Combine(solutionPath, @"FslexFsyacc\Fsyacc")
    let filePath = Path.Combine(sourcePath, @"fsyacc.fsyacc")
    let text = File.ReadAllText(filePath)
    let fsyacc = FsyaccFile.parse text

    [<Fact>]
    member this.``0 - compiler test``() =
        let result = FsyaccCompiler.compile text
        show result

    [<Fact(Skip="once for all!")>] // 
    member this.``1 - fsyacc generateParseTable``() =
        let parseTbl = fsyacc.toFsyaccParseTable()

        let name = "FsyaccParseTable"
        let moduleName = $"FslexFsyacc.Fsyacc.{name}"

        //解析表数据
        let fsharpCode = parseTbl.generateParseTable(moduleName)

        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine("output yacc:"+outputDir)

    [<Fact>]
    member this.``2 - valid ParseTable``() =
        let t = fsyacc.toFsyaccParseTable()

        Should.equal t.header        FsyaccParseTable.header
        Should.equal t.productions   FsyaccParseTable.productions
        Should.equal t.actions       FsyaccParseTable.actions
        Should.equal t.kernelSymbols FsyaccParseTable.kernelSymbols
        Should.equal t.semantics     FsyaccParseTable.semantics
        Should.equal t.declarations  FsyaccParseTable.declarations

