namespace FslexFsyacc.Fsyacc

open System
open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.YACCs

/// 表示*.fsyacc生成的模块。
type FsyaccParseTableFile =
    {
        header: string
        rules: (string list*string)list // rename to reducers
        tokens: Set<string>
        actions: (string*int)list list
        closures: (int*int*string list)list list
        declarations: Map<string,string> // symbol -> type of symbol
    }

    static member from (fsyacc:FlatFsyaccFile) =
        let rules =
            fsyacc.rules
            |> Set.map(fun rule -> rule.production,rule.reducer)
            |> Set.toList

        let tbl = fsyacc.getYacc()

        id<FsyaccParseTableFile> {
            header = fsyacc.header
            tokens = tbl.bnf.terminals
            rules = rules
            actions = tbl.encodeActions
            closures = tbl.encodeClosures
            declarations = fsyacc.declarationsLines |> Declaration.types
        }

    /// 增广产生式，它是：s' -> s
    member this.augmentProduction = fst this.rules.Head

    member this.startSymbol =        
        match this.augmentProduction with
        | [ ""; startSymbol ] -> startSymbol
        | _ -> failwith "never"

    ///闭包就是状态
    member this.printClosures () =
        // code -> production * reducer
        let rules =
            this.rules
            //|> List.sortBy fst
            |> List.mapi(fun i entry -> -i, entry)
            |> Map.ofList

        let closures =
            this.closures
            |> List.mapi(fun state closure ->
                let closure =
                    closure
                    |> List.map(fun(prod,dot,las) ->
                        let prod = fst rules.[prod]
                        prod,dot,las
                    )
                state,closure
            )

        //打印
        closures
        |> List.map(fun(state,closure)->
            $"state {state}:\r\n{RenderUtils.renderClosure closure}"
        )
        |> String.concat "\r\n"

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
            $"let actions = {stringify this.actions}"
            $"let closures = {stringify this.closures}"
            this.header
            $"let rules:list<string list*(obj list->obj)> = ["
            reducers |> Line.indentCodeBlock 4
            "]"
            "let unboxRoot ="
            $"    unbox<{this.declarations.[this.startSymbol]}>"
            "let parser = FslexFsyacc.Runtime.TokenParser.create(rules, tokens, actions, closures)"
            "let stateSymbolPairs = parser.getStateSymbolPairs()"
            "let getParser<'t> getTag getLexeme ="
            "    FslexFsyacc.Runtime.CreditParser<'t>(rules, tokens, actions, closures, getTag, getLexeme)"
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
