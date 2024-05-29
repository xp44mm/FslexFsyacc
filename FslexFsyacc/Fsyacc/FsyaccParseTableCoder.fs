namespace FslexFsyacc.Fsyacc

open System
open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.YACCs
open FslexFsyacc.Runtime.BNFs
open FslexFsyacc.Runtime.ItemCores

/// 表示*.fsyacc生成的模块。
type FsyaccParseTableCoder =
    {
        header: string
        rules: (string list*string)list // rename to reducers
        tokens: Set<string>
        kernels: list<list<int*int>> // Set<Set<ItemCore>>
        kernelSymbols: list<string>
        actions: (string*int)list list // kernel -> symbol -> kernel
        declarations: Map<string,string> // symbol -> type of symbol
    }

    static member from (fsyacc:FlatFsyaccFile) =
        let rules =
            fsyacc.rules
            |> Set.map(fun rule -> rule.production,rule.reducer)
            |> Set.toList

        let yacc = fsyacc.getYacc()
        let bnf = yacc.bnf
        let encoder = ParseTableEncoder.from(bnf.productions,bnf.kernels)

        let kernels = encoder.encodeKernels bnf.kernels
        let kernelSymbols = 
            bnf.kernelSymbols
            |> Map.values
            |> List.ofSeq

        let actions = encoder.encodeActions yacc.actions

        id<FsyaccParseTableCoder> {
            header = fsyacc.header
            rules = rules
            tokens = bnf.terminals
            kernels = kernels
            kernelSymbols = kernelSymbols
            actions = actions
            declarations = fsyacc.declarationsLines |> Declaration.types
        }

    /// 增广产生式，它是：s' -> s
    member this.augmentProduction = fst this.rules.Head

    member this.startSymbol =        
        match this.augmentProduction with
        | [ ""; startSymbol ] -> startSymbol
        | _ -> failwith "never"

    /// 生成 ParseTable Module
    /// 输入模块带名字空间的全名
    /// 删除Parse代码
    member this.generateModule (moduleName:string) =
        if this.declarations.ContainsKey this.startSymbol then
            ()
        else 
            failwith $"type annotation `{this.startSymbol}` is required."

        let augmentReducer =
            match this.rules.Head with
            | prod,reducer -> prod,  "fun(ss:obj list)-> ss.[0]" // List.exactlyOne

        let mainReducers = 
            this.rules.Tail
            |> List.map ( fun(prod, reducer) ->
                let fns = ReducerGenerator.decorateReducer this.declarations prod reducer
                prod, fns
                )

        let reducers = 
            augmentReducer :: mainReducers
            |> List.map(fun (prod, reducer) -> $"{stringify prod}, {reducer}")
            |> String.concat Environment.NewLine

        // output
        [
            $"module {moduleName}"
            $"let tokens = {stringify this.tokens}"
            $"let kernels = {stringify this.kernels}"
            $"let kernelSymbols = {stringify this.kernelSymbols}"
            $"let actions = {stringify this.actions}"

            this.header

            $"let rules: list<string list*(obj list->obj)> = ["
            reducers |> Line.indentCodeBlock 4
            "]"

            "let unboxRoot ="
            $"    unbox<{this.declarations.[this.startSymbol]}>"

        ]
        |> String.concat "\r\n"

    /// 单独生成action code的源代码module
    member this.generateMappers () =
        let types = this.declarations // symbol -> type of symbol

        this.rules
        |> List.map(fun(prod, reducer) ->
            ReducerGenerator.decorateReducer types prod reducer
            )
        |> String.concat Environment.NewLine
