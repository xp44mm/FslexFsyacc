module FslexFsyacc.Program
open FslexFsyacc.VanillaFSharp

open System
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Idioms.Literal
open FSharp.Idioms

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open System.Reflection

[<EntryPoint>]
let main _ =
    let bindFlags = BindingFlags.DeclaredOnly ||| BindingFlags.Instance ||| BindingFlags.Public
    let typ = typeof<AmbiguousCollectionCrew>
    let prs = typ.GetProperties(bindFlags)
    for pr in prs do
        Console.WriteLine($"{pr.Name}:{stringifyTypeDynamic pr.PropertyType}")
         
    let xs = typ.GetConstructors(bindFlags)

    for x in xs do
        let pas = x.GetParameters()
        for pa in pas do
            Console.WriteLine($"{pa.Name}:{stringifyTypeDynamic pa.ParameterType}")
    //let render (collection:AmbiguousCollectionCrew) =
    //    for pr in prs do
    //        let v = pr.GetValue(collection) :?> 

        //$"{typ.Name}({})"
    0
