namespace FSharpAnatomy

open System
open System.IO
open System.Text

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open FSharpAnatomy

type PostfixTyparDeclsCompilerTest (output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    static let source = TheoryDataSource [
        "<'a when 'a : null>",([NamedTypar(false,"a")],[NullConstraint(NamedTypar(false,"a"))])
        "<'a when 'a : struct>",([NamedTypar(false,"a")],[ValueTypeConstraint(NamedTypar(false,"a"))])
        "<'a when 'a : not struct>",([NamedTypar(false,"a")],[RefTypeConstraint(NamedTypar(false,"a"))])
        "<'a when 'a : unmanaged>",([NamedTypar(false,"a")],[UnmanagedConstraint(NamedTypar(false,"a"))])
        "<'a when 'a : equality>",([NamedTypar(false,"a")],[EqualityConstraint(NamedTypar(false,"a"))])
        "<'T when 'T :> System.Exception>",([NamedTypar(false,"T")],[SubtypeConstraint(NamedTypar(false,"T"),Ctor(["Exception";"System"],[]))])
        "<^a, ^b, ^c when (^a or ^b) : (static member (+) : ^a * ^b -> ^c)>",([NamedTypar(true,"c");NamedTypar(true,"b");NamedTypar(true,"a")],[MemberConstraint([NamedTypar(true,"b");NamedTypar(true,"a")],true,true,"+",Fun [Tuple(false,[TypeParam(true,"a");TypeParam(true,"b")]);TypeParam(true,"c")])])
        "<'T when 'T : (new : unit -> 'T)>",([NamedTypar(false,"T")],[DefaultConstructorConstraint(NamedTypar(false,"T"),Fun [Ctor(["unit"],[]);TypeParam(false,"T")])])
        "<'c when 'c : enum<int>>",([NamedTypar(false,"c")],[EnumConstraint(NamedTypar(false,"c"),Ctor(["int"],[]))])
        "<'T when 'T : delegate<obj * System.EventArgs, unit>>",([NamedTypar(false,"T")],[DelegateConstraint(NamedTypar(false,"T"),Tuple(false,[Ctor(["obj"],[]);Ctor(["EventArgs";"System"],[])]),Ctor(["unit"],[]))])
        "<'T,'U when 'T : equality and 'U : equality>",([NamedTypar(false,"U");NamedTypar(false,"T")],[EqualityConstraint(NamedTypar(false,"U"));EqualityConstraint(NamedTypar(false,"T"))])

        ]

    static member keys = source.keys

    [<Theory;MemberData(nameof PostfixTyparDeclsCompilerTest.keys)>]
    member _.``01 - compile``(x) =
        let y = PostfixTyparDeclsCompiler.compile x
        let e = source.[x]
        Should.equal e y

    [<Fact>]
    member _.``02 - each compile``() =
        let x = "<'T,'U when 'T : equality and 'U : equality>"
        let y = PostfixTyparDeclsCompiler.compile x
        show (x,y)


