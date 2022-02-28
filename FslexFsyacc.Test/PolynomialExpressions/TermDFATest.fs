namespace PolynomialExpressions

open System
open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open FslexFsyacc.Fslex

type TermDFATest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"term.fslex")
    let text = File.ReadAllText(filePath)
    let fslex = FslexFile.parse text

    [<Fact>]
    member _.``0 = compiler test``() =
        let hdr,dfs,rls = FslexCompiler.parseToStructuralData text
        show hdr
        show dfs
        show rls
        
    [<Fact>]
    member _.``1 = verify``() =
        let y = fslex.verify()

        Assert.True(y.undeclared.IsEmpty)
        Assert.True(y.unused.IsEmpty)

    [<Fact>]
    member _.``2 = universal characters``() =
        let res = fslex.getRegularExpressions()

        let y = 
            res
            |> Array.collect(fun re -> re.getCharacters())
            |> Set.ofArray
        show y

    [<Fact(Skip="once and for all!")>] // 
    member _.``3 = generate DFA``() =
        let name = "TermDFA"
        let moduleName = $"PolynomialExpressions.{name}"

        let dfafile = fslex.toFslexDFAFile()
        let result = dfafile.generate(moduleName)
        let outputDir = Path.Combine(__SOURCE_DIRECTORY__, $"{name}.fs")

        File.WriteAllText(outputDir, result,System.Text.Encoding.UTF8)
        output.WriteLine("output lex:" + outputDir)

    [<Fact>]
    member _.``4 = valid DFA``() =
        let y = fslex.toFslexDFAFile()

        Should.equal y.nextStates TermDFA.nextStates
        Should.equal y.header     TermDFA.header
        Should.equal y.rules      TermDFA.rules


