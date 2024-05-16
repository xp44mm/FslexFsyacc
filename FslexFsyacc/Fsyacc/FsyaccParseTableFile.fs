namespace FslexFsyacc.Fsyacc

open System
open FSharp.Idioms
open FslexFsyacc.Yacc
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ParseTables

/// 表示*.fsyacc生成的模块。
type FsyaccParseTableFile =
    {
        header: string
        rules: (string list*string)list // rename to reducers
        actions: (string*int)list list
        closures: (int*int*string list)list list
        declarations: Map<string,string> // symbol -> type of symbol
    }

    static member from (fsyacc:FlatFsyaccFile) =
        let rules =
            fsyacc.rules 
            |> Set.map(fun rule -> rule.production,rule.reducer)
            |> Set.toList

        let tbl = fsyacc.getParseTable()

        id<FsyaccParseTableFile> {
            header = fsyacc.header
            rules = rules
            actions = tbl.encodeActions
            closures = tbl.encodeClosures
            declarations = fsyacc.declarations
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
        let types = this.declarations
        let rules =
            this.rules
            |> List.map(fun(prod, reducer) ->
                let fns = SemanticGenerator.decorateSemantic types prod reducer
                $"{stringify prod},{fns}"
                )
            |> String.concat Environment.NewLine

        if types.ContainsKey this.startSymbol then
            ()
        else 
            failwith $"type annotation `{this.startSymbol}` is required."

        //
        [
            $"module {moduleName}"
            $"let actions = {stringify this.actions}"
            $"let closures = {stringify this.closures}"
            this.header
            $"let rules:list<string list*(obj list->obj)> = ["
            rules |> Line.indentCodeBlock 4
            "]"
            "let unboxRoot ="
            $"    unbox<{types.[this.startSymbol]}>"
            "let baseParser = FslexFsyacc.Runtime.BaseParser.create(rules, actions, closures)"
            "let stateSymbolPairs = baseParser.getStateSymbolPairs()"
        ] 
        |> String.concat "\r\n"

    /// 单独生成action code的源代码module
    member this.generateMappers () =
        let types = this.declarations // symbol -> type of symbol

        this.rules
        |> List.map(fun(prod, reducer) ->
            SemanticGenerator.decorateSemantic types prod reducer
            )
        |> String.concat Environment.NewLine
