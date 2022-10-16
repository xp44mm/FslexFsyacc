namespace FslexFsyacc.Fsyacc

open System
open FSharp.Idioms
open FSharp.Literals
open FslexFsyacc.Runtime

type FsyaccParseTableFile =
    {
        rules:(string list*string)list
        actions:(string*int)list list
        closures:(int*int*string list)list list
        header: string
        declarations:(string*string)list
    }

    ///闭包就是状态
    member this.printClosures() =
        let startSymbol = (fst this.rules.[0]).[0]

        let rules =
            [
                yield (["";startSymbol], "")
                yield! this.rules
            ]
            |> List.sortBy fst
            |> List.mapi(fun i entry ->(-i, entry))
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

    /// 生成ParseTable Module
    /// 输入模块带名字空间的全名
    [<Obsolete("subs with generateModule")>]
    member this.generate(moduleName:string) =
        let types = Map.ofList this.declarations // symbol -> type of symbol
        
        let rules =
            this.rules
            |> List.map(fun(prod, semantic) ->
                let mapper = SemanticGenerator.decorateSemantic types prod semantic
                $"{Literal.stringify prod},{mapper}"
                )
            |> String.concat Environment.NewLine

        // 第0项是增广产生式，它是：s' -> s，其中第1项s是开始符号
        let startSymbol = (fst this.rules.[0]).[0]

        if types.ContainsKey startSymbol then
            ()
        else 
            failwith $"type annot `{startSymbol}` is required."

        //解析表数据
        let result =
            [
                $"module {moduleName}"
                $"let actions = {Literal.stringify this.actions}"
                $"let closures = {Literal.stringify this.closures}"
                this.header
                $"let rules:(string list*(obj list->obj))list = ["
                rules |> Line.indentCodeBlock 4
                "]"
                "let parser = Parser<token>(rules,actions,closures,getTag,getLexeme)"
                "let parse(tokens:seq<token>) ="
                "    tokens"
                $"    |> parser.parse"
                $"    |> unbox<{types.[startSymbol]}>"
            ] |> String.concat Environment.NewLine
        result


    /// 生成ParseTable Module
    /// 输入模块带名字空间的全名
    /// 删除Parse代码
    member this.generateModule(moduleName:string) =
        let types = Map.ofList this.declarations // symbol -> type of symbol
        
        let rules =
            this.rules
            |> List.map(fun(prod, semantic) ->
                let mapper = SemanticGenerator.decorateSemantic types prod semantic
                $"{Literal.stringify prod},{mapper}"
                )
            |> String.concat Environment.NewLine

        // 第0项是增广产生式，它是：s' -> s，其中第1项s是开始符号
        let startSymbol = (fst this.rules.[0]).[0]

        if types.ContainsKey startSymbol then
            ()
        else 
            failwith $"type annot `{startSymbol}` is required."

        //解析表数据
        let result =
            [
                $"module {moduleName}"
                $"let actions = {Literal.stringify this.actions}"
                $"let closures = {Literal.stringify this.closures}"
                this.header
                $"let rules:(string list*(obj list->obj))list = ["
                rules |> Line.indentCodeBlock 4
                "]"
                "let unboxRoot ="
                $"    unbox<{types.[startSymbol]}>"
            ] |> String.concat Environment.NewLine
        result



    /// 单独生成action code的源代码module
    member this.generateMappers() =
        let types = Map.ofList this.declarations // symbol -> type of symbol

        this.rules
        |> List.map(fun(prod, semantic) ->
            SemanticGenerator.decorateSemantic types prod semantic
            )
        |> String.concat Environment.NewLine
