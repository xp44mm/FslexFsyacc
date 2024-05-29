namespace FslexFsyacc.Runtime

open FslexFsyacc.Runtime.Precedences
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

open FSharp.Idioms
open FSharp.Idioms.Literal

type DecompressedParseTable =
    {
        productions: Map<int, string list>
        kernels: Map<int,Set<ItemCore>>
        kernelSymbols: Map<int,string> // kernel's symbol Map
        actions: Map<Set<ItemCore>,Map<string,ParseTableAction>>
    }

    /// 产生式编码为负
    static member decompress (
        rules: Map<int,string list*(obj list->obj)>,
        kernels: list<list<int*int>>,
        kernelSymbols: list<string>,
        actions: Map<int,Map<string,int>>
    ) =
        let productions =
            rules
            |> Map.map(fun code (prod,reducer) -> prod)

        let kernels =
            kernels
            |> List.map(fun kernel -> 
                kernel
                |> List.map(fun (prod,dot) ->
                    ItemCore.just(productions.[prod],dot)
                )
                |> Set.ofList
            )
            |> List.indexed
            |> Map.ofSeq

        let kernelSymbols =
            kernelSymbols
            |> List.mapi(fun i s -> i,s)
            |> Map.ofList

            //|> Map.map(fun index itemcores ->
            //    let ic = Seq.head itemcores
            //    if index = 0 then
            //        if itemcores.Count = 1 && ic.production.Head = "" && ic.dot = 0 then
            //            ""
            //        else 
            //            failwith $"开始状态有误：{index},{stringify itemcores}"
            //    else
            //        ic.prevSymbol
            //)

        /// 具体数据编码成整数的表
        let decodeAction (iaction:int) =
            if iaction > 0 then
                let kernel = kernels.[iaction]
                Shift kernel
            else
                Reduce productions.[iaction]

        let actions =
            actions
            |> Seq.map(fun(KeyValue(ik,mp)) ->
                mp
                |> Map.map(fun symbol iact ->
                    decodeAction iact
                )
                |> Pair.prepend kernels.[ik]
            )
            |> Map.ofSeq

        { 
            productions = productions
            kernels = kernels
            kernelSymbols = kernelSymbols
            actions = actions
        }

