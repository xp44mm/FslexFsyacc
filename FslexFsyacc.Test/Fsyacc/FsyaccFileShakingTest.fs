namespace FslexFsyacc.Fsyacc
open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals.Literal
open FSharp.Idioms

open System.IO
open System.Text


type FsyaccFileShakingTest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    let text = """
%{%}
defnBindings :
    | LET rec? localBindings {  }
    | cPrototype {  }
rec? :
    | rec {  }
    | (*empty*) {  }
"""

    let rawFsyacc = RawFsyaccFile.parse text
    let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc


    [<Fact>]
    member _.``00 - rec ? test``() =

        output.WriteLine(stringify fsyacc.rules)
        //output.WriteLine(fsyacc.render())
        ()

    [<Fact>]
    member _.``01 - getProductions test``() =
        let y = FsyaccFileShaking.getProductions fsyacc.rules
        show y
        //output.WriteLine(fsyacc.render())
        let e = [
            "defnBindings",["LET";"{rec?}";"localBindings"];
            "defnBindings",["cPrototype"];
            "{rec?}",["rec"];
            "{rec?}",[]]
        Should.equal e y
        ()

    [<Fact>]
    member _.``02 - getParentChildren test``() =
        let productions = [
            "defnBindings",["LET";"{rec?}";"localBindings"];
            "defnBindings",["cPrototype"];
            "{rec?}",["rec"];
            "{rec?}",[]]
        let y = FsyaccFileShaking.getParentChildren productions
        show y
        let e = Map [
            "defnBindings",["{rec?}"];
            "{rec?}",[]]
        Should.equal e y
        ()

    [<Fact>]
    member _.``03 - getParentChildren from implementationFile test``() =
        let filePath = Path.Combine(Dir.TestPath, @"FSharpGrammar\implementationFile.fsyacc")
        let text = File.ReadAllText(filePath)
        let rawFsyacc = RawFsyaccFile.parse text
        let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

        let productions = FsyaccFileShaking.getProductions fsyacc.rules
        let y = FsyaccFileShaking.getParentChildren productions
        show y.["fileNamespaceImpls"]

        //Should.equal e y
        ()

    [<Fact>]
    member _.``04 - deepFirstSort test``() =
        let productions = [
            "defnBindings",["LET";"{rec?}";"localBindings"];
            "defnBindings",["cPrototype"];
            "{rec?}",["rec"];
            "{rec?}",[]]
        let y = FsyaccFileShaking.deepFirstSort productions "defnBindings"
        show y
        let e = ["defnBindings";"{rec?}"]
        Should.equal e y
        ()


    [<Fact>]
    member _.``0401 - deepFirstSort from implementationFile test``() =
        let filePath = Path.Combine(Dir.TestPath, @"FSharpGrammar\implementationFile.fsyacc")
        let text = File.ReadAllText(filePath)
        let rawFsyacc = RawFsyaccFile.parse text
        let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

        let productions = FsyaccFileShaking.getProductions fsyacc.rules
        let mp =  FsyaccFileShaking.getParentChildren productions
        show mp
        let mpe = Map [
            "deprecated_opt_equals",[];
            "fileModuleImpl",[];
            "fileNamespaceImpl",["namespaceIntro";"deprecated_opt_equals";"fileModuleImpl"];
            "fileNamespaceImplList",["fileNamespaceImpl"];
            "fileNamespaceImpls",["fileModuleImpl";"fileNamespaceImplList"];
            "implementationFile",["fileNamespaceImpls"];
            "namespaceIntro",[]]

        //let y = FsyaccFileShaking.deepFirstSort productions "implementationFile"
        //show y

        //let e = [
        //    "implementationFile";
        //    "fileNamespaceImpls";
        //    "fileNamespaceImplList";
        //    "fileNamespaceImpl";
        //    "deprecated_opt_equals";
        //    "namespaceIntro";
        //    "fileModuleImpl"
        //    ]

        //Should.equal e y
        ()

