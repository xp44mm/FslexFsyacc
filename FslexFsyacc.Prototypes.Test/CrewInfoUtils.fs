module FslexFsyacc.Prototypes.CrewInfoUtils

open FSharp.Reflection
//open FslexFsyacc.VanillaFSharp

open System
open System.Text.RegularExpressions
open System.Collections.Generic

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals.Literal
open FSharp.Idioms

/// 先查找

let rec getCrewInfo (tp:Type) =
    let props = FSharpType.GetRecordFields(tp,false)
    let fields =
        props
        |> Array.map(fun pi ->pi.Name, pi.PropertyType)
        |> Array.toList

    let prototype, fields =
        let x,y = fields.Head
        if x = "prototype" then
            let crew = getCrewInfo y
            Some crew, fields.Tail
        else
            None, fields

    {
        typeName = tp.Name
        prototype = prototype
        fields = fields
    }:CrewInfo


/// 第一行的声明
let generateClassDecl (crew:CrewInfo) =
    let declParams = 
        [
            match crew.prototype with
            | Some prototype -> yield $"prototype:{prototype.typeName}"
            | None -> ()

            yield!
                crew.fields
                |> List.map(fun (name,fieldType) ->
                    $"{name}:{stringifyTypeDynamic fieldType}"
                )
            
        ]
        |> String.concat ","
    $"type {crew.typeName}({declParams}) ="

/// 获得构造函数的
/// inherit语句需要递归生成
let rec generateClassCtor (crew:CrewInfo) =
    
    let otherArgs =
        crew.fields
        |> List.map fst

    let fullArgs =
        [
            match crew.prototype with Some crew1 -> yield "prototype" | None -> ()
            yield! otherArgs
        ]
        |> String.concat ","
    $"{crew.typeName}({fullArgs})"

/// 获得构造函数的
/// inherit语句需要递归生成
let rec generateClassInherit (crew:CrewInfo) =
    let fullArgs =
        [
            match crew.prototype with 
            | Some crew1 -> yield generateClassInherit crew1
            | None -> ()
            yield!
                crew.fields
                |> List.map(fun (nm,_) -> "prototype." + nm)
        ]
        |> String.concat ","
    $"{crew.typeName}({fullArgs})"

/// 获得构造函数的
/// inherit语句需要递归生成
let generateClassMembers (crew:CrewInfo) =
    crew.fields
    |> List.map(fun (name,tp) -> $"    member _.{name} = {name}")
    |> String.concat "\r\n"

let generateClassDefinition (crew:CrewInfo) =
    [   
        yield $"// {generateClassCtor crew}"
        yield generateClassDecl crew
        match crew.prototype with 
        | Some prototype -> 
            yield $"    inherit {generateClassInherit prototype}"
        | None -> ()
        yield generateClassMembers crew
    ]
    |> String.concat "\r\n"




