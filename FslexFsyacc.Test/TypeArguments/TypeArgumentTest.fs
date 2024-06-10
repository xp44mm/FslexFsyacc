namespace FslexFsyacc.TypeArguments

open System
open System.IO
open System.Text
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions

open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc.TypeArguments

type TypeArgumentTest(output:ITestOutputHelper) =
    [<Theory>]
    [<Natural(16)>]
    member _.``toCode test``(i) =
        let source = [
            "_",Anon
            "'a",TypeParam(false,"a")
            "^bc",TypeParam(true,"bc")
            "int",Ctor(["int"],[])
            "list<_>",Ctor(["list"],[Anon])
            "Map<_,_>",Ctor(["Map"],[Anon;Anon])
            "System.String",Ctor(["System";"String"],[])
            "int*float",Tuple(false,[Ctor(["int"],[]);Ctor(["float"],[])])
            "struct (int*float)",Tuple(true,[Ctor(["int"],[]);Ctor(["float"],[])])
            "int list",App(Ctor(["int"],[]),[LongIdent ["list"]])
            "int*string",Tuple(false,[Ctor(["int"],[]);Ctor(["string"],[])])
            "string->int",Fun [Ctor(["string"],[]);Ctor(["int"],[])]
            "string list*int []",Tuple(false,[App(Ctor(["string"],[]),[LongIdent ["list"]]);App(Ctor(["int"],[]),[ArrayTypeSuffix 1])])
            "string list->int list",Fun [App(Ctor(["string"],[]),[LongIdent ["list"]]);App(Ctor(["int"],[]),[LongIdent ["list"]])]
            "{|y:int;x:int|}",AnonRecd(false,["y",Ctor(["int"],[]);"x",Ctor(["int"],[])])
            "struct {|x:int|}",AnonRecd(true,["x",Ctor(["int"],[])])
            ]

        //验证总次数
        Should.equal source.Length 16
        //output.WriteLine($"{}")

        let code,ta = source.[i]
        let y = ta.toCode()
        output.WriteLine(stringify ta)
        output.WriteLine(y)

        Should.equal code y
