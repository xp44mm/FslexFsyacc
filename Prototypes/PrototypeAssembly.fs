module Prototypes.PrototypeAssembly

open FSharp.Reflection
open FSharp.Idioms.Literal

open System
open System.Text.RegularExpressions
open System.Collections.Generic
open System.Reflection

let getNamespaces (dllFilePath) =
    let asm = Assembly.LoadFrom(dllFilePath)
    let tps = asm.GetExportedTypes()

    tps
    |> Array.map(fun tp -> tp.Namespace)
    |> Array.distinct

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

let getCrewInfos (dllFilePath) =
    let asm = Assembly.LoadFrom(dllFilePath)
    let tps = asm.GetExportedTypes()

    tps
    |> Array.filter(fun tp -> FSharpType.IsRecord tp)
    |> Array.map(fun tp -> tp.Namespace,getCrewInfo tp)

let getPrototypeChains(crews:list<CrewInfo>) =
    // 假设是单链，没有分支
    let rec getAChain (acc:list<CrewInfo>) (crews:list<CrewInfo>) =
        let nexts,crews =
            crews
            |> List.partition(fun crew ->
                match crew.prototype with
                | Some crewA -> crewA = acc.Head
                | None -> false
            )
        match nexts with
        | [] -> acc,crews
        | [next] -> getAChain (next::acc) crews
        | _ -> failwith "假设是单链，没有分支"

    let rec loop (chains:list<list<CrewInfo>>) (roots:list<CrewInfo>) (derives:list<CrewInfo>) =
        match roots with
        | root::roots ->
            let chain,derives = getAChain [root] derives
            loop (chain::chains) roots derives
        | [] ->
            match derives with
            | [] -> chains
            | _ -> failwithf "不应该有多余的派生记录: %A" derives

    let roots,derives =
        crews
        |> List.partition(fun crew -> crew.prototype.IsNone)
    loop [] roots derives

/// Prototypes.xxx.yyy => xxx.yyy
let getHeadTailOfNamespace (tp:Type) =
    let x = tp.Namespace
    let i = x.IndexOf('.')
    x.Substring(0,i-1),x.Substring(i+1)

let rec getNamespacesOfRecord (tp:Type) =
    seq {
        yield tp.Namespace
        if FSharpType.IsRecord(tp) then
            for pi in FSharpType.GetRecordFields(tp,false) do
                yield! getNamespacesOfRecord pi.PropertyType
        if tp.IsGenericType then
            for atyp in tp.GetGenericArguments() do
                yield! getNamespacesOfRecord atyp
            
    }
