namespace FslexFsyacc.Fsyacc

open System
open FSharp.Idioms
open FSharp.Literals
open FslexFsyacc.Runtime

type FsyaccParseTableFile =
    {
        rules:(string list*string)[]
        actions:(string*int)[][]
        closures:(int*int*string[])[][]
        header: string
        declarations:(string*string)[]
    }

    ///闭包就是状态
    member this.printClosures() =
        let startSymbol = (fst this.rules.[0]).[0]

        let rules =
            [|
                yield ["";startSymbol], ""
                yield! this.rules
            |]
            |> Array.sortBy fst
            |> Array.mapi(fun i entry -> -i, entry)
            |> Map.ofArray

        let closures =
            this.closures
            |> Array.mapi(fun state closure ->
                let closure =
                    closure
                    |> Array.map(fun(prod,dot,las) ->
                        let prod = fst rules.[prod]
                        prod,dot,las
                    )
                state,closure
            )
        closures
        |> Array.map(fun(state,closure)->
            $"closure {state}:\r\n{RenderUtils.renderClosure closure}"
        )
        |> String.concat "\r\n"

    /// 生成ParseTable Module
    /// 输入模块带名字空间的全名
    member this.generate(moduleName:string) =
        let types = Map.ofArray this.declarations // symbol -> type of symbol
        
        let rules =
            this.rules
            |> Array.map(fun(prod, semantic) ->
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
                $"let rules:(string list*(obj[]->obj))[] = [|"
                rules |> Line.indentCodeBlock 4
                "|]"
                "let parser = Parser<token>(rules,actions,closures,getTag,getLexeme)"
                "let parse(tokens:seq<token>) ="
                "    tokens"
                $"    |> parser.parse"
                $"    |> unbox<{types.[startSymbol]}>"
            ] |> String.concat Environment.NewLine
        result

    /// 单独生成action code的源代码module
    member this.generateMappers() =
        let types = Map.ofArray this.declarations // symbol -> type of symbol
        this.rules
        |> Array.map(fun(prod, semantic) ->
            SemanticGenerator.decorateSemantic types prod semantic
            )
        |> String.concat Environment.NewLine
