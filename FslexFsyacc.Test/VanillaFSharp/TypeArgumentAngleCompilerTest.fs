namespace FslexFsyacc.VanillaFSharp

open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals.Literal
open FSharp.Idioms
open FslexFsyacc.Brackets

type TypeArgumentAngleCompilerTest(output:ITestOutputHelper) =
    static let DataSource = SingleDataSource [
        "<>",Bounded(0,[],1)
        "<int> count",Bounded(0,[Tick "int"],4)
        "<int*int> pair",Bounded(0,[Tick "int";Tick "*";Tick "int"],8)
        "<list<int>> ls",Bounded(0,[Tick "list";Bounded(5,[Tick "int"],9)],10)
        ]

    static member keys = DataSource.keys

    [<Theory>]
    [<MemberData(nameof TypeArgumentAngleCompilerTest.keys)>]
    member _.``compile``(x:string) =
        let y = TypeArgumentAngleCompiler.compile 0 x
        output.WriteLine(stringify y)
        let e = DataSource.[x]
        Should.equal y e


