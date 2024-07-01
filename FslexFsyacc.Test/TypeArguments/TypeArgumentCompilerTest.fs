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
open FslexFsyacc

type TypeArgumentCompilerTest(output:ITestOutputHelper) =
    [<Theory>]
    [<Natural(17)>]
    member _.``compile test``(i:int) =
        let source = [
            "_",Anon
            "'a",TypeParam(false,"a")
            "^bc",TypeParam(true,"bc")
            "int",Ctor(["int"],[])
            "list<_>",Ctor(["list"],[Anon])
            "Map<int,_>",Ctor(["Map"],[Ctor(["int"],[]);Anon])
            "System.String",Ctor(["System";"String"],[])
            "(int*float)",Tuple(false,[Ctor(["int"],[]);Ctor(["float"],[])])
            "struct(int*float)",Tuple(true,[Ctor(["int"],[]);Ctor(["float"],[])])
            "int list",App(Ctor(["int"],[]),[LongIdent ["list"]])
            "int * string",Tuple(false,[Ctor(["int"],[]);Ctor(["string"],[])])
            "string->int",Fun [Ctor(["string"],[]);Ctor(["int"],[])]
            "string list*int[]",Tuple(false,[App(Ctor(["string"],[]),[LongIdent ["list"]]);App(Ctor(["int"],[]),[ArrayTypeSuffix 1])])
            "string list->int list",Fun [App(Ctor(["string"],[]),[LongIdent ["list"]]);App(Ctor(["int"],[]),[LongIdent ["list"]])]
            "{|x:int;y:string|}",AnonRecd(false,["x",Ctor(["int"],[]);"y",Ctor(["string"],[])])
            "{|x:int;y:int;|}",AnonRecd(false,["x",Ctor(["int"],[]);"y",Ctor(["int"],[])])
            "struct{|x:int|}",AnonRecd(true,["x",Ctor(["int"],[])])
            ]

        //验证总次数
        Should.equal source.Length 17
        //output.WriteLine($"{}")

        let x,e = source.[i]
        output.WriteLine(x)
        output.WriteLine(stringify e)
        let exit (rest:string) = Regex.IsMatch(rest, @"^\s*\>")
        let src = SourceText.just(99, $"{x}>")
        let ta,length = TypeArgumentCompiler.compile exit src
        output.WriteLine(stringify ta)
        output.WriteLine($"length={length}")
        Should.equal e ta

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
            SourceText.just(0, ls.[i])
            |> TypeArgumentTokenUtils.tokenize
            |> Seq.head
            |> (fun x -> x.value)

        let e = ARRAY_TYPE_SUFFIX (i+1)
        Should.equal e y

    [<Theory>]
    [<Natural(37)>]
    member _.``from types test``(i:int) =
        let lines = 
            let path = Path.Combine(__SOURCE_DIRECTORY__,"types.txt")
            File.ReadLines(path, Encoding.UTF8)
            |> Seq.toArray

        //验证总次数
        Should.equal lines.Length 37

        let x = lines.[i]

        output.WriteLine(x)


        //for x in lines do
        let exit (rest:string) = Regex.IsMatch(rest, @"^\s*$")
        let src = SourceText.just(0,x)
        let ta,length = TypeArgumentCompiler.compile exit src
        output.WriteLine(stringify ta)
        //Should.equal e ta

        //let uta = TypeArgumentUtils.uniform ta
        //let utas = uta.toCode()
        //output.WriteLine(stringify uta)

        //output.WriteLine($"typeof<{x}>,typeof<{utas}>")
