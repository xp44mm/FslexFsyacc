namespace FSharpAnatomy

open System
open System.IO
open System.Text

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open FSharpAnatomy

type TypeArgumentCompilerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine
    static let source = TheoryDataSource [
        "_",Anon
        "'a",TypeParam(false,"a")
        "^bc",TypeParam(true,"bc")
        "int",Ctor(["int"],[])
        "list<_>",Ctor(["list"],[Anon])
        "Map<_,_>",Ctor(["Map"],[Anon;Anon])
        "System.String",Ctor(["String";"System"],[])
        "{|x:int;y:int|}",AnonRecd(false,["y",Ctor(["int"],[]);"x",Ctor(["int"],[])])
        "{|x:int;y:int;|}",AnonRecd(false,["y",Ctor(["int"],[]);"x",Ctor(["int"],[])])
        "(int*float)",Tuple(false,[Ctor(["int"],[]);Ctor(["float"],[])])
        "struct(int*float)",Tuple(true,[Ctor(["int"],[]);Ctor(["float"],[])])
        "struct{|x:int|}",AnonRecd(true,["x",Ctor(["int"],[])])
        "int list",App(Ctor(["int"],[]),[LongIdent ["list"]])
        "int * string",Tuple(false,[Ctor(["int"],[]);Ctor(["string"],[])])
        "string->int",Fun [Ctor(["string"],[]);Ctor(["int"],[])]
        "string list*int[]",Tuple(false,[App(Ctor(["string"],[]),[LongIdent ["list"]]);App(Ctor(["int"],[]),[ArrayTypeSuffix 1])])
        "string list->int list",Fun [App(Ctor(["string"],[]),[LongIdent ["list"]]);App(Ctor(["int"],[]),[LongIdent ["list"]])]

        ]

    static member keys = source.keys

    [<Theory;MemberData(nameof TypeArgumentCompilerTest.keys)>]
    member _.``01 - compile``(x) =
        let y = TypeArgumentCompiler.compile x
        let e = source.[x]
        Should.equal e y

    [<Fact>]
    member _.``02 - compile``() =
        let x = "string list->int list"
        let y = TypeArgumentCompiler.compile x

        show (x,y)

    [<Theory>]
    [<InlineData("[]", 1)>]
    [<InlineData("[,]", 2)>]
    [<InlineData("[,,]", 3)>]
    [<InlineData("[,,,]", 4)>]
    member _.``03 - ArrayTypeSuffix``(x,e) =
        let y = 
            x 
            |> TypeArgumentUtils.tokenize 0
            |> ArrayTypeSuffixDFA.analyze
            |> Seq.head
            |> (fun x -> match x.value with ARRAY_TYPE_SUFFIX i -> i |_->failwith"")
        Should.equal y e

