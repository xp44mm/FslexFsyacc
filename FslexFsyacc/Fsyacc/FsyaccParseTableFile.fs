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

    /// 输入模块带名字空间的全名
    member this.generate(moduleName:string) =
        let types = Map.ofArray this.declarations // symbol -> type of symbol
        
        let fxRules =
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
        else failwith $"type annot `{startSymbol}` is required."

        //解析表数据
        let result =
            [
                $"module {moduleName}"
                $"let rules = {Literal.stringify this.rules}"
                $"let actions = {Literal.stringify this.actions}"
                $"let closures = {Literal.stringify this.closures}"
                $"let header = {Literal.stringify this.header}"
                $"let declarations = {Literal.stringify this.declarations}"
                this.header
                $"let fxRules:(string list*(obj[]->obj))[] = [|"
                fxRules |> Line.indentCodeBlock 4
                "|]"
                "open FslexFsyacc.Runtime"
                "let parser = Parser<token>(fxRules,actions,closures,getTag,getLexeme)"
                "let parse(tokens:seq<token>) ="
                "    tokens"
                "    |> parser.parse"
                $"    |> unbox<{types.[startSymbol]}>"
            ] |> String.concat Environment.NewLine
        result

