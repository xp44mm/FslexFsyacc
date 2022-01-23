namespace FslexFsyacc.Fsyacc

open System
open FSharp.Idioms
open FSharp.Literals

type FsyaccParseTableFile =
    {
        rules:(string list*string)[]
        actions:(string*int)[][]
        closures:(int*int*string[])[][]
        header: string
        declarations:(string*string)[]
    }

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
                "let parser = Parser(fxRules, actions, closures)"
                "let parse (tokens:seq<_>) ="
                "    parser.parse(tokens, getTag, getLexeme)"
                $"    |> unbox<{types.[startSymbol]}>"
            ] |> String.concat Environment.NewLine
        result



