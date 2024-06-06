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

type TypeArgumentCompilerTest(output:ITestOutputHelper) =
    [<Theory>]
    [<Natural(17)>]
    member _.``compile test``(i) =
        let source = [
            "_",Anon
            "'a",TypeParam(false,"a")
            "^bc",TypeParam(true,"bc")
            "int",Ctor(["int"],[])
            "list<_>",Ctor(["list"],[Anon])
            "Map<_,_>",Ctor(["Map"],[Anon;Anon])
            "System.String",Ctor(["String";"System"],[])
            "(int*float)",Tuple(false,[Ctor(["int"],[]);Ctor(["float"],[])])
            "struct(int*float)",Tuple(true,[Ctor(["int"],[]);Ctor(["float"],[])])
            "int list",App(Ctor(["int"],[]),[LongIdent ["list"]])
            "int * string",Tuple(false,[Ctor(["int"],[]);Ctor(["string"],[])])
            "string->int",Fun [Ctor(["string"],[]);Ctor(["int"],[])]
            "string list*int[]",Tuple(false,[App(Ctor(["string"],[]),[LongIdent ["list"]]);App(Ctor(["int"],[]),[ArrayTypeSuffix 1])])
            "string list->int list",Fun [App(Ctor(["string"],[]),[LongIdent ["list"]]);App(Ctor(["int"],[]),[LongIdent ["list"]])]
            "{|x:int;y:int|}",AnonRecd(false,["y",Ctor(["int"],[]);"x",Ctor(["int"],[])])
            "{|x:int;y:int;|}",AnonRecd(false,["y",Ctor(["int"],[]);"x",Ctor(["int"],[])])
            "struct{|x:int|}",AnonRecd(true,["x",Ctor(["int"],[])])
            ]

        //验证总次数
        Should.equal source.Length 17
        //output.WriteLine($"{}")

        let exit (rest:string) = Regex.IsMatch(rest, @"^\s*\>")
        let x,e = source.[i]
        let txt = $"{x}>"
        output.WriteLine(x)
        let y = TypeArgumentCompiler.compile exit 99 txt
        output.WriteLine(stringify y)
        //Should.equal e y

    [<Theory>]
    [<Natural(4)>]
    member _.``ArrayTypeSuffix``(i) =
        let ls = [
            "[]"
            "[,]"
            "[,,]"
            "[,,,]"
        ]
        //output.WriteLine($"{ls.Length}")
        Should.equal ls.Length 4

        let y = 
            ls.[i]
            |> TypeArgumentUtils.tokenize 0
            |> Seq.head
            |> (fun x -> x.value)

        let e = ARRAY_TYPE_SUFFIX (i+1)
        Should.equal e y

