﻿namespace FslexFsyacc.Yacc
open FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Idioms.Literal

type ParseTableEncoder =
    {
        productions: Map<string list,int>
        kernels    : Map<Set<ItemCore>,int>
    }
    /// 产生式编码为负
    static member getProductions(productions:Set<string list>)=
        productions
        |> Set.toList
        |> List.mapi(fun i p -> p,-i)
        |> Map.ofList

    /// 具体数据编码成整数的表
    member this.encodeAction(action:Action) =
        match action with
        | Shift j -> this.kernels.[j]
        | Reduce p -> this.productions.[p]

    member this.encodeProduction(production:string list) =
            this.productions.[production]

    member this.encodeItemCore(itemCore: ItemCore) =
        let iprod = 
            this.productions.[itemCore.production]
        iprod,itemCore.dot

    /// return state -> symbol -> action, state等于list的index
    member encoder.getEncodedActions(actions: Map<int,Map<string,Action>>) =
        encoder.kernels
        |> Map.toList
        |> List.mapi(fun i (_,state) ->
            if i<>state then failwith $"state编码不同于自然数序列"
            if actions.ContainsKey state then
                actions.[state]
                |> Map.toList
                |> List.map(fun(la,action)->
                    //try
                    let iaction = encoder.encodeAction action
                    la,iaction
                    //with _ -> failwithf "%A" (la,action)
                )
            else []
        )

    member encoder.getEncodedClosures(closures: Map<int,Map<ItemCore,Set<string>>>) =
        encoder.kernels
        |> Map.toList
        |> List.mapi(fun i (_,state) ->
            if i<>state then failwith $"state编码不同于自然数序列"
            if closures.ContainsKey state then
                closures.[state]
                |> Map.toList
                |> List.map(fun(icore,las)->
                    let prod,dot = encoder.encodeItemCore icore
                    prod,dot,Set.toList las
                )
            else []
        )
